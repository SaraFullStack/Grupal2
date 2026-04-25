using UnityEngine;

public class TutoExample : MonoBehaviour
{
    
    [SerializeField] public TutorialType tutorialType;

    static private bool hasBeenShowed = false;
    
    void OnTriggerEnter(Collider other)
    {
        if (!hasBeenShowed)
        {
            Time.timeScale = 0;

            TutorialController.LaunchTutorial(tutorialType);
            hasBeenShowed = true;
        }
    }

}
