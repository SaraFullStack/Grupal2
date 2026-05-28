using Cinemachine;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Localization;
using UnityEngine.UIElements;

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

    private Dialog dialog;
    private bool isDialogShown;
    private bool isClosingDialog;
    private bool isReady;
    private int actualPage;

    private AudioSource audioSource;
    private CinemachineInputProvider camInput;
    private Tween messageTween;

    public static DialogController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        audioSource = GetComponent<AudioSource>();

        if (audioSource != null)
        {
            audioSource.playOnAwake = false;
            audioSource.loop = false;
        }

        camInput = FindFirstObjectByType<CinemachineInputProvider>();

        UIDocument document = GetComponent<UIDocument>();

        if (document == null)
        {
            Debug.LogError("DialogController necesita un UIDocument.");
            enabled = false;
            return;
        }

        var root = document.rootVisualElement;

        _background = root.Q<VisualElement>(background);
        _content = root.Q<VisualElement>(content);
        _npc = root.Q<VisualElement>(npc);
        _labelClose = root.Q<Label>(labelClose);
        _labelNPC = root.Q<Label>(labelNPC);
        _message = root.Q<Label>(message);
        _avatar = root.Q<VisualElement>(avatar);
        _next = root.Q<VisualElement>(next);
        _close = root.Q<VisualElement>(close);

        root.style.display = DisplayStyle.None;

        if (_labelClose != null)
            _labelClose.style.display = DisplayStyle.None;

        if (_next != null)
            _next.style.display = DisplayStyle.None;

        if (_close != null)
            _close.style.display = DisplayStyle.None;
    }

    private void Update()
    {
        if (!isDialogShown || isClosingDialog)
            return;

        if (InputManager.submitWasPressed || InputManager.clickWasPressed)
        {
            if (isReady)
            {
                actualPage++;
                NextMessage(actualPage);
            }
        }
    }

    private void OnInputActionChange(object obj, InputActionChange change)
    {
        if (change != InputActionChange.ActionPerformed)
            return;

        InputAction inputAction = obj as InputAction;

        if (inputAction == null || inputAction.activeControl == null)
            return;

        if (_labelClose == null)
            return;

        InputDevice lastDevice = inputAction.activeControl.device;

        if (lastDevice is Gamepad)
        {
            var btnDialog = new LocalizedString("Dialogs", "btn_accept");
            _labelClose.SetBinding("text", btnDialog);
        }
        else
        {
            var btnDialog = new LocalizedString("Dialogs", "btn_click");
            _labelClose.SetBinding("text", btnDialog);
        }
    }

    public static void LaunchDialog(DialogType type)
    {
        if (Instance == null)
            return;

        Instance.dialog = type.GetDialog();
        Instance.ShowDialog();
    }

    private void ShowDialog()
    {
        if (isDialogShown)
            return;

        if (InputManager.Instance != null)
            InputManager.Instance.OpenUI();
        else
            Time.timeScale = 0f;

        InputManager.ResetAllMobileInput();

        SetCameraInput(false);

        actualPage = 0;
        isReady = false;
        isClosingDialog = false;

        if (_message != null)
            _message.text = "";

        if (_next != null)
        {
            _next.RemoveFromClassList(nextAnimate);
            _next.AddToClassList(nextAnimate);
        }

        UIDocument document = GetComponent<UIDocument>();
        document.rootVisualElement.style.display = DisplayStyle.Flex;

        isDialogShown = true;

        _background?.AddToClassList(backgroundShow);
        _content?.AddToClassList(contentShow);
        _npc?.AddToClassList(npcShow);

        StartMessages();
    }

    private void StartMessages()
    {
        if (dialog != null && dialog.messages != null && dialog.messages.Length > 0)
        {
            NextMessage(0);
        }
        else
        {
            HideDialog();
        }
    }

    private void NextMessage(int page)
    {
        isReady = false;

        messageTween?.Kill();
        messageTween = null;

        if (dialog == null || dialog.messages == null || dialog.messages.Length <= page)
        {
            HideDialog();
            return;
        }

        if (_labelClose != null)
            _labelClose.style.display = DisplayStyle.None;

        if (_next != null)
            _next.style.display = DisplayStyle.None;

        if (_close != null)
            _close.style.display = DisplayStyle.None;

        if (_message != null)
            _message.text = "";

        bool isLastPage = dialog.messages.Length - 1 == page;

        DialogMessage messageDialog = dialog.messages[page];

        string npcKey = messageDialog.CurrentNPC.GetNameKey();
        string npcAvatar = messageDialog.CurrentNPC.GetAvatarName();
        string messageKey = messageDialog.CurrentMessageKey;

        if (_labelNPC != null)
        {
            var npcName = new LocalizedString("Dialogs", npcKey);
            _labelNPC.SetBinding("text", npcName);
        }

        if (_avatar != null)
        {
            Sprite sprite = Resources.Load<Sprite>("Sprites/" + npcAvatar);
            _avatar.style.backgroundImage = new StyleBackground(sprite);
        }

        var localizedMessage = new LocalizedString("DialogMessages", messageKey);
        string msg = localizedMessage.GetLocalizedString();

        if (_message == null)
        {
            FinishPage(isLastPage);
            return;
        }

        float timeMessage = Mathf.Max(0.25f, msg.Length / 20.0f);
        int lastLength = 0;

        messageTween = DOTween.To(
                () => _message.text,
                x => _message.text = x,
                msg,
                timeMessage
            )
            .SetEase(Ease.Linear)
            .SetUpdate(true)
            .OnUpdate(() =>
            {
                if (_message.text.Length > lastLength && _message.text[_message.text.Length - 1] != ' ')
                {
                    if (audioSource != null && typingSound != null)
                    {
                        audioSource.pitch = Random.Range(0.8f, 1.2f);
                        audioSource.PlayOneShot(typingSound);
                    }

                    lastLength = _message.text.Length;
                }
            })
            .OnComplete(() => FinishPage(isLastPage));
    }

    private void FinishPage(bool isLastPage)
    {
        if (_labelClose != null)
            _labelClose.style.display = DisplayStyle.Flex;

        if (isLastPage)
        {
            if (_close != null)
                _close.style.display = DisplayStyle.Flex;
        }
        else
        {
            if (_next != null)
                _next.style.display = DisplayStyle.Flex;
        }

        isReady = true;
    }

    private void HideDialog()
    {
        if (!isDialogShown || isClosingDialog)
            return;

        isClosingDialog = true;
        ResetDialog();
    }

    private void ResetDialog()
    {
        messageTween?.Kill();
        messageTween = null;

        UIDocument document = GetComponent<UIDocument>();

        if (document != null)
            document.rootVisualElement.style.display = DisplayStyle.None;

        if (_labelClose != null)
            _labelClose.style.display = DisplayStyle.None;

        if (_next != null)
            _next.style.display = DisplayStyle.None;

        if (_close != null)
            _close.style.display = DisplayStyle.None;

        _content?.RemoveFromClassList(contentShow);
        _background?.RemoveFromClassList(backgroundShow);
        _npc?.RemoveFromClassList(npcShow);

        if (_message != null)
            _message.text = string.Empty;

        isDialogShown = false;
        isClosingDialog = false;
        isReady = false;
        actualPage = 0;

        SetCameraInput(true);

        InputManager.ResetAllMobileInput();

        if (InputManager.Instance != null)
            InputManager.Instance.CloseUI();
        else
            Time.timeScale = 1f;

        if (EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(null);
    }

    private void SetCameraInput(bool enabled)
    {
        if (camInput != null)
            camInput.enabled = enabled;
    }

    private void OnEnable()
    {
        InputSystem.onActionChange += OnInputActionChange;
    }

    private void OnDisable()
    {
        InputSystem.onActionChange -= OnInputActionChange;

        messageTween?.Kill();
        messageTween = null;

        SetCameraInput(true);
    }
}