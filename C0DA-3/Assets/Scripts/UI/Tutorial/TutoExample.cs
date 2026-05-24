using UnityEngine;

public class TutoExample : MonoBehaviour
{
    
    [SerializeField] public TutorialType tutorialType;

    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
TutorialController.LaunchTutorial(tutorialType);
        }
    }

}
