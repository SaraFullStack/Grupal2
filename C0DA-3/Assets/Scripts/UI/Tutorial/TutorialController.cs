using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Localization;
using DG.Tweening;
using UnityEngine.Video;
using System.Collections.Generic;


public class TutorialController : MonoBehaviour
{

    [SerializeField] public VideoClip[] playList;
    [SerializeField] public RenderTexture  renderTexture;
    
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

    private VisualElement _backgroundContainer;
    private VisualElement _frame;
    private VisualElement _content;
    private Label _header;
    private Label _title;
    private Label _message;
    private VisualElement _inside;
    private VisualElement _video;
    private Button _button;
    
    private const string backgroundShow = "background--show";
    private const string frameShow = "frame--show";
    private const string frameHide = "frame--hide";
    private const string contentShow = "content--show";
    private const string titleShow = "title--show";
    private const string insideHide = "inside--hide";
    private const string buttonShow = "button--show";

    private TutorialType selectedTutorial;

    private static bool isTutorialShown;
    
    private string messageTutorial = "";
    private VideoPlayer videoPlayer;
    
    // Singleton
    private static TutorialController _instance;
    public static TutorialController Instance { get { return _instance; } }
    
    void Awake()
    {
        // TODO: get saved tutorial shown
        tutorialShowed = new List<int>();
        
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
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
        
        root.style.display = DisplayStyle.None; // Hide tutorial
        _video.style.display = DisplayStyle.None;
    }
    
    void Start()
    {
        videoPlayer = gameObject.AddComponent<VideoPlayer>();
        videoPlayer.targetTexture = renderTexture;
        videoPlayer.playOnAwake = false;
        videoPlayer.isLooping = true;
        videoPlayer.renderMode = VideoRenderMode.RenderTexture;
    }

    private void ShowTutorial()
    {
        if (isTutorialShown || tutorialShowed.Contains((int)selectedTutorial))
        {
            return;
        }
        
        Time.timeScale = 0;
        
        tutorialShowed.Add((int)selectedTutorial);
        
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
        
        var root = GetComponent<UIDocument>().rootVisualElement;
        root.style.display = DisplayStyle.Flex;
        isTutorialShown = true;
        
        // Set texts localized
        var headerTutorial = new LocalizedString("Tutorials", "tutorial_header");
        _header.SetBinding("text", headerTutorial);
        
        var titleTutorial = new LocalizedString("Tutorials", selectedTutorial.GetTitle());
        _title.SetBinding("text", titleTutorial);
        
        var btnText = new LocalizedString("Tutorials", "btn_close");
        _button.SetBinding("text", btnText);
        
        var msgTutorial = new LocalizedString("Tutorials", selectedTutorial.GetMessage());
        messageTutorial = msgTutorial.GetLocalizedString();
        
        // Prepare video
        int index = (int)selectedTutorial;
        videoPlayer.clip = playList[index];
        videoPlayer.targetTexture = renderTexture;

        
        
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
        
        Time.timeScale = 1;
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
        _backgroundContainer.UnregisterCallback<TransitionEndEvent>(OnBackroundShown);
        _content.UnregisterCallback<TransitionEndEvent>(OnContentShown);
        _inside.UnregisterCallback<TransitionEndEvent>(OnInsideHiden);
        _button.RegisterCallback<ClickEvent>(OnCloseButtonClicked, TrickleDown.TrickleDown);
    }
    
    private void OnCloseButtonClicked(ClickEvent e)
    {
        _button.RemoveFromClassList(buttonShow);
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
                // Inside
                _message.text = string.Empty;
                _video.style.display = DisplayStyle.Flex;
                videoPlayer.Play();
                
                DOTween.To(()=> _message.text, x => _message.text = x, messageTutorial, 6f).SetEase(Ease.Linear)
                    .OnComplete(() => {
                        // Siguiente paso
                        //_tutorialIcon.AddToClassList("icon-show");
                        _button.AddToClassList(buttonShow);
                    }).SetUpdate(true);
                
                
                
            }
            else
            {
                // Final animation
                _backgroundContainer.RemoveFromClassList(backgroundShow);
                _frame.AddToClassList(frameHide);
            }
        }
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
