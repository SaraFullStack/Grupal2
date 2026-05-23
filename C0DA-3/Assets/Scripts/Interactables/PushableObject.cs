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

    private bool isPushing;
    private Vector3 pushVelocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
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
        if (isBlocked)
            return;

        if (IsStopZone(other))
        {
            LockOnStopZone();
            return;
        }

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

        if (!data.allowPushX)
            moveDir.x = 0f;

        if (!data.allowPushZ)
            moveDir.z = 0f;

        if (moveDir.sqrMagnitude < 0.001f)
            return;

        moveDir.Normalize();

        // Solo guardamos la velocidad objetivo, la física se aplica en FixedUpdate
        isPushing = true;
        pushVelocity = moveDir * data.moveSpeed;
    }

    private void FixedUpdate()
    {
        if (isBlocked || !isPushing)
        {
            isPushing = false;
            return;
        }

        Vector3 currentVelocity = new Vector3(
            rb.linearVelocity.x,
            0f,
            rb.linearVelocity.z
        );

        Vector3 smoothedVelocity = Vector3.Lerp(
            currentVelocity,
            pushVelocity,
            data.smoothness * Time.fixedDeltaTime
        );

        rb.linearVelocity = new Vector3(
            smoothedVelocity.x,
            rb.linearVelocity.y,
            smoothedVelocity.z
        );

        isPushing = false; // reset hasta el próximo OnTriggerStay
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

        transform.position = new Vector3(
            stopZone.transform.position.x,
            transform.position.y,
            stopZone.transform.position.z
        );

        stopZone.gameObject.GetComponent<Renderer>().material = greenMaterial;

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