using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using DG.Tweening;
using UnityEngine.Localization;
using UnityEngine.Video;

public class TutorialControllerOLD : MonoBehaviour
{
    
    [SerializeField] RenderTexture videoRenderTexture1;
    [SerializeField] public VideoClip[] playList;
    [SerializeField] public RenderTexture  renderTexture;
    
    private const string tutorialContainer = "Tutorial";
    private const string tutorialContent = "TutorialContent";
    private const string tutorialTitle = "TutorialTitle";
    private const string tutorialIcon = "TutorialIcon";
    private const string tutorialName = "TutorialName";
    private const string tutorialMessage = "TutorialMessage";
    private const string tutorialInside = "TutorialInside";
    private const string tutorialVideo = "TutorialVideo";
    
    private VisualElement _tutorialContainer;
    private VisualElement _tutorialContent;
    private Label _tutorialTitle;
    private VisualElement _tutorialIcon;
    private Label _tutorialName;
    private Label _tutorialMessage;
    private VisualElement _tutorialInside;
    private VisualElement _tutorialVideo;

    private string messageTutorial = "";
    
    private VideoPlayer videoPlayer;

    void Awake()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        _tutorialContainer = root.Q<VisualElement>(tutorialContainer);
        _tutorialContent = root.Q<VisualElement>(tutorialContent);
        _tutorialTitle = root.Q<Label>(tutorialTitle);
        _tutorialIcon = root.Q<VisualElement>(tutorialIcon);
        _tutorialName = root.Q<Label>(tutorialName);
        _tutorialMessage = root.Q<Label>(tutorialMessage);
        _tutorialInside = root.Q<VisualElement>(tutorialInside);
        _tutorialVideo = root.Q<VisualElement>(tutorialVideo);
        
        _tutorialContainer.style.display = DisplayStyle.None; // Hide tutorial
    }

    void Start()
    {
        videoPlayer = gameObject.AddComponent<VideoPlayer>();
        videoPlayer.targetTexture = renderTexture;
        videoPlayer.playOnAwake = false;
        videoPlayer.isLooping = true;
        videoPlayer.renderMode = VideoRenderMode.RenderTexture;
    }

    void Update()
    {
        if (Keyboard.current.digit1Key.isPressed)
        {
            // Show Tutorial 1
            _tutorialMessage.text = string.Empty;
            _tutorialContainer.style.display = DisplayStyle.Flex;
            
            // Set texts localized
            var nameTutorial = new LocalizedString("Tutorials", "tutorial1_title");
            _tutorialName.SetBinding("text", nameTutorial);
            
            var msgTutorial = new LocalizedString("Tutorials", "tutorial1_message");
            messageTutorial = msgTutorial.GetLocalizedString();
            
            // Prepare video
            videoPlayer.clip = playList[0];
            videoPlayer.targetTexture = renderTexture;
            
            
            // Start animation
            Invoke("AnimateTutorial", 0.1f);
        }
        else if (Keyboard.current.digit2Key.isPressed)
        {
            // Show Tutorial 2
            _tutorialMessage.text = string.Empty;
            _tutorialContainer.style.display = DisplayStyle.Flex;
            
            // Set texts localized
            var nameTutorial = new LocalizedString("Tutorials", "tutorial2_title");
            _tutorialName.SetBinding("text", nameTutorial);
            
            var msgTutorial = new LocalizedString("Tutorials", "tutorial2_message");
            messageTutorial = msgTutorial.GetLocalizedString();
            
            // Prepare video
            videoPlayer.clip = playList[1];
            videoPlayer.targetTexture = renderTexture;
            
            // Start animation
            Invoke("AnimateTutorial", 0.1f);
        }
        else if (Keyboard.current.digit0Key.isPressed)
        {
            // End animation
            Invoke("AnimateHideTutorial", 0.1f);
        }
    }
    
    private void OnEnable()
    {
        _tutorialContainer.RegisterCallback<TransitionEndEvent>(OnTutorialShown);
        _tutorialContent.RegisterCallback<TransitionEndEvent>(OnContentShown);
    }
    
    private void OnDisable()
    {
        _tutorialContainer.UnregisterCallback<TransitionEndEvent>(OnTutorialShown);
        _tutorialContent.UnregisterCallback<TransitionEndEvent>(OnContentShown);
    }
    
    private void AnimateTutorial()
    {
        _tutorialContainer.AddToClassList("tutorial-show");
    }
    
    private void OnTutorialShown(TransitionEndEvent evn)
    {
        if (_tutorialContainer.ClassListContains("tutorial-show") && !_tutorialContent.ClassListContains("content-full") && !_tutorialContent.ClassListContains("content-hide"))
        {
            _tutorialContent.AddToClassList("content-full");
            _tutorialTitle.AddToClassList("title-show");
            _tutorialName.AddToClassList("tutorial-name-show");
            //_tutorialIcon.AddToClassList("icon-show");
            
            _tutorialIcon.schedule.Execute(() =>
            {
                _tutorialIcon.ToggleInClassList("icon-rotate-left");
            });
            
            Invoke("ActivateTextAndIcon", 3f);
            /*
            _tutorialMessage.text = string.Empty;
            string m = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.";
            DOTween.To(()=> _tutorialMessage.text, x => _tutorialMessage.text = x, m, 3f).SetEase(Ease.Linear);
            */
            
            _tutorialIcon.RegisterCallback<TransitionEndEvent>(
                evt =>
                {
                    _tutorialIcon.ToggleInClassList("icon-rotate-left");
                });
            
        } 
        else if (_tutorialContainer.ClassListContains("tutorial-hide"))
        {
            Invoke("DisableTutorial", 5f);
            
        }
       
    }

    private void DisableTutorial()
    {
        _tutorialContainer.style.display = DisplayStyle.None;
        _tutorialContainer.RemoveFromClassList("tutorial-hide");
        _tutorialContainer.RemoveFromClassList("tutorial-show");
        
        _tutorialTitle.RemoveFromClassList("title-show");
        
        _tutorialContent.RemoveFromClassList("content-hide");
        _tutorialContent.RemoveFromClassList("content-full");
        
        _tutorialIcon.RemoveFromClassList("icon-rotate-left");
        _tutorialIcon.RemoveFromClassList("icon-show");
        
        _tutorialInside.RemoveFromClassList("tutorial-inside-hide");

    }

    private void OnContentShown(TransitionEndEvent evn)
    {
        _tutorialVideo.AddToClassList("tutorial-video-show");
        videoPlayer.Play();
        
        if (_tutorialContent.ClassListContains("content-hide"))
        {
            Invoke("HideTutorial", 1.5f);
        }
        
    }

    private void ActivateTextAndIcon()
    {
        
        
        _tutorialMessage.text = string.Empty;
        DOTween.To(()=> _tutorialMessage.text, x => _tutorialMessage.text = x, messageTutorial, 6f).SetEase(Ease.Linear)
            .OnComplete(() => {
            _tutorialIcon.AddToClassList("icon-show");
        });;
    }
    
    private void AnimateHideTutorial()
    {
        _tutorialContent.AddToClassList("content-hide");
        _tutorialInside.AddToClassList("tutorial-inside-hide");
    }

    private void HideTutorial()
    {
        
        _tutorialContainer.AddToClassList("tutorial-hide");
    }

}
