using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialExample : MonoBehaviour
{
    
    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            TutorialController.LaunchTutorial(TutorialType.DobleJump);
        }
        else if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            TutorialController.LaunchTutorial(TutorialType.Rush);
        }
        else if (Keyboard.current.digit0Key.wasPressedThisFrame)
        {
            TutorialController.CloseTutorial();
        }
    }
}
