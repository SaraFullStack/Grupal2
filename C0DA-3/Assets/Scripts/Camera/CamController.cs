using UnityEngine;
using UnityEngine.InputSystem;

public class CamController : MonoBehaviour
{
    [SerializeField] private float camSensitivity = 1f;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private InputActionReference moveCam;

    private float xRot;
    private float yRot;

    private void Update()
    {
        if (playerTransform != null)
            transform.position = playerTransform.position;

        Vector2 look = Vector2.zero;

        if (moveCam != null && moveCam.action != null)
            look += moveCam.action.ReadValue<Vector2>();

        look += InputManager.lookDelta;

        xRot += -look.y * camSensitivity;
        yRot += look.x * camSensitivity;

        xRot = Mathf.Clamp(xRot, -35f, 60f);

        transform.eulerAngles = new Vector3(xRot, yRot, 0f);
    }
}