using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerPush : MonoBehaviour
{
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        PushableObject pushable = hit.collider.GetComponentInParent<PushableObject>();

        if (pushable == null)
            return;

        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0f, hit.moveDirection.z);

        if (pushDir.sqrMagnitude < 0.01f)
            return;

        pushable.Push(pushDir);
    }
}