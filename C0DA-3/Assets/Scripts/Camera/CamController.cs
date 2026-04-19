using UnityEngine;
using UnityEngine.InputSystem;

public class CamController : MonoBehaviour
{
    // Cam variables
    [SerializeField] float camSensitivity;
    float xRot;
    float yRot;
    public Transform playerTransform;

    public InputActionReference moveCam;

    // Update is called once per frame
    void Update()
    {
        transform.position = playerTransform.position;

        xRot += -moveCam.action.ReadValue<Vector2>().y * camSensitivity;
        yRot += moveCam.action.ReadValue<Vector2>().x * camSensitivity;
        xRot = Mathf.Clamp(xRot, -89, 89);
        transform.eulerAngles = new Vector3(xRot, yRot);
    }
}
