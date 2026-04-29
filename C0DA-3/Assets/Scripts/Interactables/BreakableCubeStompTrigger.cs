using UnityEngine;

public class BreakableCubeStompTrigger : MonoBehaviour
{
    [SerializeField] private BreakableCube owner;
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private float minDownwardSpeed = -0.75f;

    private bool usedThisJump;

    private void Reset()
    {
        BoxCollider col = GetComponent<BoxCollider>();
        if (col != null)
            col.isTrigger = true;
    }

    private void Awake()
    {
        if (owner == null)
            owner = GetComponentInParent<BreakableCube>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (usedThisJump)
            return;

        if (!other.CompareTag(playerTag))
            return;

        PlayerController player = other.GetComponentInParent<PlayerController>();
        if (player == null)
            return;

        if (player.VerticalVelocity > minDownwardSpeed)
            return;

        usedThisJump = true;

        if (owner != null)
            owner.TryStomp();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(playerTag))
            return;

        usedThisJump = false;
    }
}