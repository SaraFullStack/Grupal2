using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PushableObject : MonoBehaviour
{
    [SerializeField] private PushableObjectData data;

    [Header("Optional Stop Zone")]
    [SerializeField] private Collider stopZone;

    private Rigidbody rb;
    public bool isBlocked;

    [Header("Materiales")]
    [SerializeField] private Material greenMaterial;

    private Vector3 pushDirection;
    private bool isBeingPushed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (isBlocked)
            return;

        if (!isBeingPushed)
        {
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
            return;
        }

        Vector3 targetVelocity = pushDirection * data.moveSpeed;

        rb.linearVelocity = new Vector3(
            targetVelocity.x,
            rb.linearVelocity.y,
            targetVelocity.z
        );

        isBeingPushed = false;
    }

    public void Push(Vector3 direction)
    {
        if (isBlocked)
            return;

        if (data == null)
            return;

        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
            return;

        direction.Normalize();

        Vector3 moveDir = GetDominantAxis(direction);

        if (!data.allowPushX)
            moveDir.x = 0f;

        if (!data.allowPushZ)
            moveDir.z = 0f;

        if (moveDir.sqrMagnitude < 0.001f)
            return;

        pushDirection = moveDir.normalized;
        isBeingPushed = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsStopZone(other))
        {
            LockOnStopZone();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (IsStopZone(other))
        {
            LockOnStopZone();
        }
    }

    private bool IsStopZone(Collider other)
    {
        if (stopZone == null)
            return false;

        return other == stopZone;
    }

    private void LockOnStopZone()
    {
        if (isBlocked)
            return;

        isBlocked = true;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        Vector3 targetPosition = rb.position;
        targetPosition.x = stopZone.bounds.center.x;
        targetPosition.z = stopZone.bounds.center.z;

        transform.position = new Vector3(stopZone.transform.position.x, transform.position.y, stopZone.transform.position.z);

        if (greenMaterial != null && stopZone.TryGetComponent(out Renderer renderer))
            renderer.material = greenMaterial;

        rb.position = targetPosition;

        rb.constraints =
            RigidbodyConstraints.FreezePositionX |
            RigidbodyConstraints.FreezePositionZ |
            RigidbodyConstraints.FreezeRotation;
    }

    private Vector3 GetDominantAxis(Vector3 dir)
    {
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.z))
            return new Vector3(Mathf.Sign(dir.x), 0f, 0f);

        return new Vector3(0f, 0f, Mathf.Sign(dir.z));
    }
}