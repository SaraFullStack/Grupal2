using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Localization;
using DG.Tweening;
using UnityEngine.InputSystem;
using Cinemachine;
using Random = UnityEngine.Random;

public class DialogController : MonoBehaviour
{
    [SerializeField] public AudioClip typingSound;

    private const string background = "Background";
    private const string content = "Dialog_Content";
    private const string npc = "NPC";
    private const string labelClose = "Label_Close";
    private const string labelNPC = "Label_NPC";
    private const string message = "Message";
    private const string avatar = "Avatar";
    private const string next = "Next";
    private const string close = "Close";


    private VisualElement _background;
    private VisualElement _content;
    private VisualElement _npc;
    private Label _labelClose;
    private Label _labelNPC;
    private Label _message;
    private VisualElement _avatar;
    private VisualElement _next;
    private VisualElement _close;


    private const string backgroundShow = "background--show";
    private const string contentShow = "content--show";
    private const string npcShow = "npc--show";
    private const string nextAnimate = "next--right";


    private DialogType selectedDialog;
    private Dialog dialog;
    private bool isDialogShown = false;
    private bool isReady = false;
    private int actualPage = 0;

    private AudioSource audioSource;
    private CinemachineInputProvider camInput;

    public static DialogController Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;

        camInput = FindFirstObjectByType<CinemachineInputProvider>();

        var root = GetComponent<UIDocument>().rootVisualElement;
        _background = root.Q<VisualElement>(background);
        _content = root.Q<VisualElement>(content);
        _npc = root.Q<VisualElement>(npc);
        _labelClose = root.Q<Label>(labelClose);
        _labelNPC = root.Q<Label>(labelNPC);
        _message = root.Q<Label>(message);
        _avatar = root.Q<VisualElement>(avatar);
        _next = root.Q<VisualElement>(next);
        _close = root.Q<VisualElement>(close);

        root.style.display = DisplayStyle.None; // Hide Dialog

        _labelClose.style.display = DisplayStyle.None;
        _next.style.display = DisplayStyle.None;
        _close.style.display = DisplayStyle.None;
    }

    private void Update()
    {
         InputSystem.onActionChange += (obj, change) =>
        {
            if (change == InputActionChange.ActionPerformed)
            {
                var inputAction = (InputAction) obj;
                var lastControl = inputAction.activeControl;
                var lastDevice = lastControl.device;


                if (lastDevice is Gamepad)
                {
                    // Set texts localized
                    var btnDialog = new LocalizedString("Dialogs", "btn_accept");
                    _labelClose.SetBinding("text", btnDialog);

                }
                else
                {
                    // Set texts localized
                    var btnDialog = new LocalizedString("Dialogs", "btn_click");
                    _labelClose.SetBinding("text", btnDialog);

                }

            }
        };
        
        if (InputManager.submitWasPressed || InputManager.clickWasPressed)
        {
            if (isReady)
            {
                actualPage += 1;
                NextMessage(actualPage);
            }
        }

    }

    #region static methods

    public static void LaunchDialog(DialogType type)
    {
        if (Instance == null)
        {
            Debug.LogError("No hay DialogController activo en la escena.");
            return;
        }

        Instance.selectedDialog = type;
        Instance.dialog = type.GetDialog();
        Instance.ShowDialog();
    }
        
    #endregion
    
    #region private methods

    private void ShowDialog()
    {
        if (isDialogShown)
        {
            return;
        }

        InputManager.Instance.OpenUI();
        SetCameraInput(false);
        actualPage = 0;
        _message.text = "";
        _next.ToggleInClassList(nextAnimate);

        var root = GetComponent<UIDocument>().rootVisualElement;
        root.style.display = DisplayStyle.Flex;
        isDialogShown = true;

        _background.AddToClassList(backgroundShow);
        _content.AddToClassList(contentShow);
    }
        
    

    private void HideDialog()
    {
        if (!isDialogShown)
        {
            return;
        }

        _npc.RemoveFromClassList(npcShow);
        _background.RemoveFromClassList(backgroundShow);
        _content.RemoveFromClassList(contentShow);

    }

    private void ResetDialog()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        root.style.display = DisplayStyle.None;

        _labelClose.style.display = DisplayStyle.None;
        _next.style.display = DisplayStyle.None;
        _close.style.display = DisplayStyle.None;

        _content.RemoveFromClassList(contentShow);
        _background.RemoveFromClassList(backgroundShow);
        _message.text = string.Empty;

        isDialogShown = false;

        SetCameraInput(true);
        InputManager.Instance.CloseUI();
    }

    private void SetCameraInput(bool enabled)
    {
        if (camInput != null)
            camInput.enabled = enabled;
    }

    private void OnContentShown(TransitionEndEvent evn)
    {
        VisualElement element = evn.target as VisualElement;

        if (element.name == content && _content.ClassListContains(contentShow))
        {
            StartMessages();
        }
        else if (element.name == content)
        {
            ResetDialog();
        }
    }

    private void OnNextAnimate(TransitionEndEvent evn)
    {
        VisualElement element = evn.target as VisualElement;

        if (element.name == next && _content.ClassListContains(contentShow))
        {
            _next.ToggleInClassList(nextAnimate);
        }
    }

    private void StartMessages()
    {
        if (dialog.messages.Length > 0)
        {
            // Set first message
            NextMessage(0);
            _npc.AddToClassList(npcShow);

        }
        else
        {
            HideDialog();
        }
    }

    private void NextMessage(int page)
    {
        isReady = false;

        if (dialog.messages.Length <= page)
        {
            HideDialog();
            return;
        }

        _labelClose.style.display = DisplayStyle.None;
        _next.style.display = DisplayStyle.None;
        _close.style.display = DisplayStyle.None;

        _message.text = "";

        bool isLastPage = (dialog.messages.Length -1 == page);

        DialogMessage messageDialog = dialog.messages[page];
        string npcKey = messageDialog.CurrentNPC.GetNameKey();
        string npcAvatar = messageDialog.CurrentNPC.GetAvatarName();
        string messageKey = messageDialog.CurrentMessageKey;

        var npcName = new LocalizedString("Dialogs", npcKey);
        _labelNPC.SetBinding("text", npcName);

        // Imagen avatar
        Sprite spriteCargado = Resources.Load<Sprite>("Sprites/"+npcAvatar);
        _avatar.style.backgroundImage = new StyleBackground(spriteCargado);

        var msgTutorial = new LocalizedString("DialogMessages", messageKey);
        string msg = msgTutorial.GetLocalizedString();

        float timeMessage = msg.Length / 20.0f;
        int lastLength = 0;

        DOTween.To(()=> _message.text, x => _message.text = x, msg, timeMessage).SetEase(Ease.Linear)
            .SetUpdate(true)
            .OnUpdate(() => {
                if (_message.text.Length > lastLength && _message.text[_message.text.Length - 1] != ' ') {
                    audioSource.pitch = Random.Range(0.8f, 1.2f);
                    audioSource.PlayOneShot(typingSound);
                    lastLength = _message.text.Length;
                }
            })
            .OnComplete(() => {
                _labelClose.style.display = DisplayStyle.Flex;
                if (isLastPage)
                {
                    _close.style.display = DisplayStyle.Flex;
                }
                else
                {
                    _next.style.display = DisplayStyle.Flex;
                }

                isReady = true;
            }).SetUpdate(true);
        
        
        
    }

    private void OnEnable()
    {
        _content.RegisterCallback<TransitionEndEvent>(OnContentShown);
        _next.RegisterCallback<TransitionEndEvent>(OnNextAnimate);
    }

    private void OnDisable()
    {
        _content.UnregisterCallback<TransitionEndEvent>(OnContentShown);
        _next.UnregisterCallback<TransitionEndEvent>(OnNextAnimate);

        SetCameraInput(true);
    }
    #endregion
    
}

