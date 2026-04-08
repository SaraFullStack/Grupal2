using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class TutorialController : MonoBehaviour
{
    private const string tutorialContainer = "Tutorial";
    private const string tutorialContent = "TutorialContent";
    private const string tutorialTitle = "TutorialTitle";
    private const string tutorialIcon = "TutorialIcon";
    
    private VisualElement _tutorialContainer;
    private VisualElement _tutorialContent;
    private Label _tutorialTitle;
    private VisualElement _tutorialIcon;

    void Awake()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        _tutorialContainer = root.Q<VisualElement>(tutorialContainer);
        _tutorialContent = root.Q<VisualElement>(tutorialContent);
        _tutorialTitle = root.Q<Label>(tutorialTitle);
        _tutorialIcon = root.Q<VisualElement>(tutorialIcon);
        
        _tutorialContainer.style.display = DisplayStyle.None; // Hide tutorial
    }

    void Update()
    {
        if (Keyboard.current.digit1Key.isPressed)
        {
            // Show Tutorial 1
            
            _tutorialContainer.style.display = DisplayStyle.Flex;
            
            // Start animation
            Invoke("AnimateTutorial", 1f);
        }
    }
    
    private void OnEnable()
    {
        _tutorialContainer.RegisterCallback<TransitionEndEvent>(OnTutorialShown);

    }
    
    private void OnDisable()
    {
        _tutorialContainer.UnregisterCallback<TransitionEndEvent>(OnTutorialShown);
    }
    
    private void AnimateTutorial()
    {
        _tutorialContainer.AddToClassList("tutorial-show");
    }
    
    private void OnTutorialShown(TransitionEndEvent evn)
    {
        if (_tutorialContainer.ClassListContains("tutorial-show") && !_tutorialContent.ClassListContains("content-full"))
        {
            _tutorialContent.AddToClassList("content-full");
            _tutorialTitle.AddToClassList("title-show");
            _tutorialIcon.AddToClassList("icon-show");
            
            _tutorialIcon.schedule.Execute(() =>
            {
                _tutorialIcon.ToggleInClassList("icon-rotate-left");
            });
            
            
            _tutorialIcon.RegisterCallback<TransitionEndEvent>(
                evt =>
                {
                    Debug.Log("ROTATE END");
                    Debug.Log(_tutorialIcon.ClassListContains("icon-rotate-left"));
                    _tutorialIcon.ToggleInClassList("icon-rotate-left");
                });
            
        }
    }

}
