using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerPush : MonoBehaviour
{
    [SerializeField] private float pushStrength = 2f;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rb = hit.collider.attachedRigidbody;

        if (rb == null || rb.isKinematic)
            return;

        PushableObject pushable = hit.collider.GetComponent<PushableObject>();
        if (pushable == null)
            return;

        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0f, hit.moveDirection.z);

        if (pushDir.sqrMagnitude < 0.01f)
            return;

        rb.AddForce(pushDir.normalized * pushStrength, ForceMode.Force);
    }
}