using UnityEngine;

public class TutoExample : MonoBehaviour
{
    
    [SerializeField] public TutorialType tutorialType;

    
    void OnTriggerEnter(Collider other)
    {
        TutorialController.LaunchTutorial(tutorialType);
    }

}
