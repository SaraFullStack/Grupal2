using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Localization;
using UnityEngine.UIElements;
using UnityEngine.Video;
using Cursor = UnityEngine.Cursor;

public class TutorialController : MonoBehaviour
{
    [SerializeField] private GameDataSO gameData;
    [SerializeField] public VideoClip[] playList;
    [SerializeField] public RenderTexture renderTexture;

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

    private bool isClosingTutorial;
    private bool contentStarted;

    private string messageTutorial = "";
    private VideoPlayer videoPlayer;
    private Tween messageTween;

    private static TutorialController _instance;
    public static TutorialController Instance => _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;

        UIDocument document = GetComponent<UIDocument>();

        if (document == null)
        {
            Debug.LogError("TutorialController necesita un UIDocument.");
            enabled = false;
            return;
        }

        var root = document.rootVisualElement;

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

        if (_video != null)
            _video.style.display = DisplayStyle.None;
    }

    private void Start()
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

    private void Update()
    {
        if (!isTutorialShown || isClosingTutorial)
            return;

        bool closePressed =
            InputManager.cancelWasPressed ||
            InputManager.submitWasPressed ||
            InputManager.clickWasPressed;

        if (!closePressed)
            return;

        bool canClose =
            _button == null ||
            _textButton == null ||
            _button.ClassListContains(buttonShow) ||
            _textButton.ClassListContains(textButtonShow);

        if (canClose)
            HideTutorial();
    }

    private void OnInputActionChange(object obj, InputActionChange change)
    {
        if (change != InputActionChange.ActionPerformed)
            return;

        InputAction inputAction = obj as InputAction;

        if (inputAction == null || inputAction.activeControl == null)
            return;

        if (_button == null || _textButton == null)
            return;

        InputDevice lastDevice = inputAction.activeControl.device;

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

    public static void LaunchTutorial(TutorialType type)
    {
        if (_instance == null)
            return;

        _instance.selectedTutorial = type;
        _instance.ShowTutorial();
    }

    public static void CloseTutorial()
    {
        if (_instance == null)
            return;

        _instance.HideTutorial();
    }

    private void ShowTutorial()
    {
        int tutorialIndex = (int)selectedTutorial;

        if (isTutorialShown)
            return;

        if (gameData != null && gameData.watchedTutorials.Contains(tutorialIndex))
            return;

        if (InputManager.Instance != null)
            InputManager.Instance.OpenUI();
        else
            Time.timeScale = 0f;

        InputManager.ResetAllMobileInput();

        if (gameData != null)
            gameData.watchedTutorials.Add(tutorialIndex);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        UIDocument document = GetComponent<UIDocument>();
        document.rootVisualElement.style.display = DisplayStyle.Flex;

        isTutorialShown = true;
        isClosingTutorial = false;
        contentStarted = false;

        _button?.RemoveFromClassList(buttonShow);
        _textButton?.RemoveFromClassList(textButtonShow);

        if (_message != null)
            _message.text = string.Empty;

        if (_header != null)
        {
            var headerTutorial = new LocalizedString("Tutorials", "tutorial_header");
            _header.SetBinding("text", headerTutorial);
        }

        if (_title != null)
        {
            var titleTutorial = new LocalizedString("Tutorials", selectedTutorial.GetTitle());
            _title.SetBinding("text", titleTutorial);
        }

        if (_textButton != null)
        {
            var closeTutorial = new LocalizedString("Tutorials", "text_close");
            _textButton.SetBinding("text", closeTutorial);
        }

        if (_button != null)
        {
            var btnText = new LocalizedString("Tutorials", "btn_close");
            _button.SetBinding("text", btnText);
        }

        var msgTutorial = new LocalizedString("Tutorials", selectedTutorial.GetMessage());
        messageTutorial = msgTutorial.GetLocalizedString();

        PrepareVideo();

        _backgroundContainer?.AddToClassList(backgroundShow);
        _frame?.AddToClassList(frameShow);
        _content?.AddToClassList(contentShow);

        StartTutorialContent();
    }

    private void PrepareVideo()
    {
        int index = (int)selectedTutorial;

        if (videoPlayer == null)
            return;

        videoPlayer.Stop();
        videoPlayer.targetTexture = renderTexture;

        if (playList != null && index >= 0 && index < playList.Length)
        {
            videoPlayer.clip = playList[index];
            videoPlayer.Prepare();
        }
    }

    private void StartTutorialContent()
    {
        if (contentStarted)
            return;

        contentStarted = true;

        _title?.AddToClassList(titleShow);

        if (_message != null)
            _message.text = string.Empty;

        if (_video != null)
            _video.style.display = DisplayStyle.Flex;

        if (videoPlayer != null && videoPlayer.clip != null)
        {
            if (videoPlayer.isPrepared)
            {
                videoPlayer.Play();
            }
            else
            {
                videoPlayer.prepareCompleted -= OnVideoPrepared;
                videoPlayer.prepareCompleted += OnVideoPrepared;
            }
        }

        messageTween?.Kill();

        if (_message == null)
        {
            ShowCloseButtons();
            return;
        }

        messageTween = DOTween.To(
                () => _message.text,
                x => _message.text = x,
                messageTutorial,
                3f
            )
            .SetEase(Ease.Linear)
            .SetUpdate(true)
            .OnComplete(ShowCloseButtons);
    }

    private void ShowCloseButtons()
    {
        _button?.AddToClassList(buttonShow);
        _textButton?.AddToClassList(textButtonShow);
    }

    private void HideTutorial()
    {
        if (!isTutorialShown || isClosingTutorial)
            return;

        isClosingTutorial = true;
        ResetTutorial();
    }

    private void ResetTutorial()
    {
        messageTween?.Kill();
        messageTween = null;

        if (videoPlayer != null)
        {
            videoPlayer.prepareCompleted -= OnVideoPrepared;
            videoPlayer.Stop();
        }

        UIDocument document = GetComponent<UIDocument>();

        if (document != null)
            document.rootVisualElement.style.display = DisplayStyle.None;

        _backgroundContainer?.RemoveFromClassList(backgroundShow);

        _frame?.RemoveFromClassList(frameHide);
        _frame?.RemoveFromClassList(frameShow);

        _content?.RemoveFromClassList(contentShow);
        _title?.RemoveFromClassList(titleShow);
        _inside?.RemoveFromClassList(insideHide);

        _button?.RemoveFromClassList(buttonShow);
        _textButton?.RemoveFromClassList(textButtonShow);

        if (_message != null)
            _message.text = string.Empty;

        if (_video != null)
            _video.style.display = DisplayStyle.None;

        isTutorialShown = false;
        isClosingTutorial = false;
        contentStarted = false;

        InputManager.ResetAllMobileInput();

        if (InputManager.Instance != null)
            InputManager.Instance.CloseUI();
        else
            Time.timeScale = 1f;

        if (EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(null);
    }

    private void OnVideoPrepared(VideoPlayer vp)
    {
        vp.prepareCompleted -= OnVideoPrepared;
        vp.Play();
    }

    private void OnEnable()
    {
        InputSystem.onActionChange += OnInputActionChange;

        if (_button != null)
            _button.RegisterCallback<ClickEvent>(OnCloseButtonClicked, TrickleDown.TrickleDown);
    }

    private void OnDisable()
    {
        InputSystem.onActionChange -= OnInputActionChange;

        if (_button != null)
            _button.UnregisterCallback<ClickEvent>(OnCloseButtonClicked, TrickleDown.TrickleDown);

        messageTween?.Kill();
        messageTween = null;

        if (videoPlayer != null)
            videoPlayer.prepareCompleted -= OnVideoPrepared;
    }

    private void OnCloseButtonClicked(ClickEvent e)
    {
        HideTutorial();
    }
}