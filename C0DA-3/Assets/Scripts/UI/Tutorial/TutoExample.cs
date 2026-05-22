using UnityEngine;

public class TutoExample : MonoBehaviour
{
    
    [SerializeField] public TutorialType tutorialType;

    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log(transform.gameObject.name);
            TutorialController.LaunchTutorial(tutorialType);
        }
    }

}
