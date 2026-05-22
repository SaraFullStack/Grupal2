using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Localization;
using DG.Tweening;
using UnityEngine.Video;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class TutorialController : MonoBehaviour
{
    [SerializeField] private GameDataSO gameData;

    [SerializeField] public VideoClip[] playList;
    [SerializeField] public RenderTexture renderTexture;

    private List<int> tutorialShowed;

    private const string background = "Background";
    private const string frame = "Frame";
    private const string content = "Content";
    private const string headerLabel = "HeaderLabel";
    private const string title = "Title";
    private const string message = "Message";
    private const string inside = "Inside";
    private const string video = "InsideVideo";
    private const string button = "Button";
    private const string textButton = "TextButton";

    private VisualElement _backgroundContainer;
    private VisualElement _frame;
    private VisualElement _content;
    private Label _header;
    private Label _title;
    private Label _message;
    private VisualElement _inside;
    private VisualElement _video;
    private Button _button;
    private Label _textButton;

    private const string backgroundShow = "background--show";
    private const string frameShow = "frame--show";
    private const string frameHide = "frame--hide";
    private const string contentShow = "content--show";
    private const string titleShow = "title--show";
    private const string insideHide = "inside--hide";
    private const string buttonShow = "button--show";
    private const string textButtonShow = "text-button--show";

    private TutorialType selectedTutorial;

    private static bool isTutorialShown;

    private string messageTutorial = "";
    private VideoPlayer videoPlayer;

    private static TutorialController _instance;
    public static TutorialController Instance { get { return _instance; } }

    void Awake()
    {
        tutorialShowed = new List<int>();

        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        var root = GetComponent<UIDocument>().rootVisualElement;
        _backgroundContainer = root.Q<VisualElement>(background);
        _frame = root.Q<VisualElement>(frame);
        _content = root.Q<VisualElement>(content);
        _header = root.Q<Label>(headerLabel);
        _title = root.Q<Label>(title);
        _message = root.Q<Label>(message);
        _inside = root.Q<VisualElement>(inside);
        _video = root.Q<VisualElement>(video);
        _button = root.Q<Button>(button);
        _textButton = root.Q<Label>(textButton);

        root.style.display = DisplayStyle.None;
        _video.style.display = DisplayStyle.None;
    }

    void Start()
    {
        videoPlayer = gameObject.AddComponent<VideoPlayer>();
        videoPlayer.playOnAwake = false;
        videoPlayer.isLooping = true;
        videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        videoPlayer.targetTexture = renderTexture;
        videoPlayer.timeUpdateMode = VideoTimeUpdateMode.UnscaledGameTime;
        videoPlayer.waitForFirstFrame = true;
        videoPlayer.skipOnDrop = false;

    }

    void Update()
    {
        InputSystem.onActionChange += (obj, change) =>
        {
            if (change == InputActionChange.ActionPerformed)
            {
                var inputAction = (InputAction)obj;
                var lastControl = inputAction.activeControl;
                var lastDevice = lastControl.device;

                if (lastDevice is Gamepad)
                {
                    _button.style.display = DisplayStyle.None;
                    _textButton.style.display = DisplayStyle.Flex;
                }
                else
                {
                    _button.style.display = DisplayStyle.Flex;
                    _textButton.style.display = DisplayStyle.None;
                }
            }
        };

        if (InputManager.cancelWasPressed)
        {
            if (_textButton.ClassListContains(textButtonShow))
            {
                _button.RemoveFromClassList(buttonShow);
                _textButton.RemoveFromClassList(textButtonShow);
                HideTutorial();
            }
        }
    }

    private void ShowTutorial()
    {
        int tutorialIndex = (int)selectedTutorial;

        if (isTutorialShown || gameData.watchedTutorials.Contains(tutorialIndex))
        {
            return;
        }

        InputManager.Instance.OpenUI();

        gameData.watchedTutorials.Add(tutorialIndex);

        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;

        var root = GetComponent<UIDocument>().rootVisualElement;
        root.style.display = DisplayStyle.Flex;
        isTutorialShown = true;

        var headerTutorial = new LocalizedString("Tutorials", "tutorial_header");
        _header.SetBinding("text", headerTutorial);

        var titleTutorial = new LocalizedString("Tutorials", selectedTutorial.GetTitle());
        _title.SetBinding("text", titleTutorial);

        var closeTutorial = new LocalizedString("Tutorials", "text_close");
        _textButton.SetBinding("text", closeTutorial);

        var btnText = new LocalizedString("Tutorials", "btn_close");
        _button.SetBinding("text", btnText);

        var msgTutorial = new LocalizedString("Tutorials", selectedTutorial.GetMessage());
        messageTutorial = msgTutorial.GetLocalizedString();

        int index = (int)selectedTutorial;

        videoPlayer.Stop();
        videoPlayer.clip = playList[index];
        videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        videoPlayer.targetTexture = renderTexture;
        videoPlayer.Prepare();

        _backgroundContainer.AddToClassList(backgroundShow);
        _frame.AddToClassList(frameShow);
    }

    private void HideTutorial()
    {
        if (!isTutorialShown)
        {
            return;
        }

        _button.RemoveFromClassList(buttonShow);
        _textButton.RemoveFromClassList(textButtonShow);
        _inside.AddToClassList(insideHide);
    }

    private void ResetTutorial()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        root.style.display = DisplayStyle.None;

        _frame.RemoveFromClassList(frameHide);
        _frame.RemoveFromClassList(frameShow);
        _title.RemoveFromClassList(titleShow);
        _inside.RemoveFromClassList(insideHide);
        _message.text = string.Empty;
        _video.style.display = DisplayStyle.None;
        isTutorialShown = false;

        if (videoPlayer != null)
            videoPlayer.Stop();

        InputManager.Instance.CloseUI();

        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
    }

    private void OnEnable()
    {
        _backgroundContainer.RegisterCallback<TransitionEndEvent>(OnBackroundShown);
        _content.RegisterCallback<TransitionEndEvent>(OnContentShown);
        _inside.RegisterCallback<TransitionEndEvent>(OnInsideHiden);
        _button.RegisterCallback<ClickEvent>(OnCloseButtonClicked, TrickleDown.TrickleDown);
    }

    private void OnDisable()
    {
        if (_backgroundContainer != null)
            _backgroundContainer.UnregisterCallback<TransitionEndEvent>(OnBackroundShown);

        if (_content != null)
            _content.UnregisterCallback<TransitionEndEvent>(OnContentShown);

        if (_inside != null)
            _inside.UnregisterCallback<TransitionEndEvent>(OnInsideHiden);

        if (_button != null)
            _button.UnregisterCallback<ClickEvent>(OnCloseButtonClicked, TrickleDown.TrickleDown);
    }

    private void OnCloseButtonClicked(ClickEvent e)
    {
        _button.RemoveFromClassList(buttonShow);
        _textButton.RemoveFromClassList(textButtonShow);
        HideTutorial();
    }

    public static void LaunchTutorial(TutorialType type)
    {
        _instance.selectedTutorial = type;
        _instance.ShowTutorial();
    }

    public static void CloseTutorial()
    {
        _instance.HideTutorial();
    }

    private void OnBackroundShown(TransitionEndEvent evn)
    {
        if (evn.AffectsProperty(new StylePropertyName("background-color")))
        {
            if (_backgroundContainer.ClassListContains(backgroundShow))
            {
                _content.AddToClassList(contentShow);
            }
            else
            {
                ResetTutorial();
            }
        }
    }

    private void OnContentShown(TransitionEndEvent evn)
    {
        if (evn.AffectsProperty(new StylePropertyName("height")))
        {
            if (_content.ClassListContains(contentShow))
            {
                _title.AddToClassList(titleShow);

                _message.text = string.Empty;
                _video.style.display = DisplayStyle.Flex;

                if (videoPlayer.isPrepared)
                {
                    videoPlayer.Play();
                }
                else
                {
                    videoPlayer.prepareCompleted += OnVideoPrepared;
                }

                DOTween.To(() => _message.text, x => _message.text = x, messageTutorial, 6f).SetEase(Ease.Linear)
                    .OnComplete(() => {
                        _button.AddToClassList(buttonShow);
                        _textButton.AddToClassList(textButtonShow);
                    }).SetUpdate(true);
            }
            else
            {
                _backgroundContainer.RemoveFromClassList(backgroundShow);
                _frame.AddToClassList(frameHide);
            }
        }
    }

    private void OnVideoPrepared(VideoPlayer vp)
    {
        vp.prepareCompleted -= OnVideoPrepared;
        vp.Play();
    }

    private void OnInsideHiden(TransitionEndEvent evn)
    {
        if (evn.AffectsProperty(new StylePropertyName("opacity")))
        {
            if (_inside.ClassListContains(insideHide))
            {
                _content.RemoveFromClassList(contentShow);
            }
        }
    }
}