using UnityEngine;

public class BreakableCubeStompTrigger : MonoBehaviour
{
    [SerializeField] private BreakableCube owner;
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private float minDownwardSpeed = -0.1f;

    private bool alreadyTriggeredThisContact = false;

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
        if (alreadyTriggeredThisContact)
            return;

        if (!other.CompareTag(playerTag))
            return;

        PlayerController player = other.GetComponentInParent<PlayerController>();
        if (player == null)
            return;

        if (player.VerticalVelocity > minDownwardSpeed)
            return;

        alreadyTriggeredThisContact = true;
        owner.TryStomp();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(playerTag))
            return;

        alreadyTriggeredThisContact = false;
    }
}