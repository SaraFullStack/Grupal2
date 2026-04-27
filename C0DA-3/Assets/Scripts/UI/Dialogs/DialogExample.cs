using UnityEngine;

public class DialogExample : MonoBehaviour
{
    [SerializeField] public DialogType dialogType;

    
    void OnTriggerEnter(Collider other)
    {
        DialogController.LaunchDialog(dialogType);
    }
}
