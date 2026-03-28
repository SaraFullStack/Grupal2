using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PushableObject : MonoBehaviour
{
    [SerializeField] private PushableObjectData data;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 velocity = rb.linearVelocity;
        velocity.y = rb.linearVelocity.y;
        rb.linearVelocity = velocity;
    }

    private void OnTriggerStay(Collider other)
    {
        if (data == null)
            return;

        if (!other.CompareTag("Player"))
            return;

        Vector3 directionToBox = transform.position - other.transform.position;
        directionToBox.y = 0f;

        if (directionToBox.sqrMagnitude < 0.001f)
            return;

        directionToBox.Normalize();

        Vector3 moveDir = GetDominantAxis(directionToBox);

        if (!data.allowPushX) moveDir.x = 0f;
        if (!data.allowPushZ) moveDir.z = 0f;

        if (moveDir.sqrMagnitude < 0.001f)
            return;

        moveDir.Normalize();

        Vector3 targetVelocity = moveDir * data.moveSpeed;
        Vector3 currentVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        Vector3 smoothedVelocity = Vector3.Lerp(currentVelocity, targetVelocity, data.smoothness * Time.fixedDeltaTime);

        rb.linearVelocity = new Vector3(smoothedVelocity.x, rb.linearVelocity.y, smoothedVelocity.z);
    }

    private Vector3 GetDominantAxis(Vector3 dir)
    {
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.z))
        {
            return new Vector3(Mathf.Sign(dir.x), 0f, 0f);
        }
        else
        {
            return new Vector3(0f, 0f, Mathf.Sign(dir.z));
        }
    }
}