using UnityEngine;

public class DialogExample : MonoBehaviour
{
    [SerializeField] public DialogType dialogType;


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DialogController.LaunchDialog(dialogType);
        }
    }
}
