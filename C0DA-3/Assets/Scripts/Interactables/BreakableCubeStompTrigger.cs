using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BreakableCubeStompTrigger : MonoBehaviour
{
    [SerializeField] private BreakableCube owner;
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private float minDownwardSpeed = -0.1f;

    private bool hasHitThisFall;

    private void Awake()
    {
        if (owner == null)
            owner = GetComponentInParent<BreakableCube>();

        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag(playerTag))
            return;

        PlayerController player = other.GetComponentInParent<PlayerController>();

        if (player == null)
            return;

        if (player.VerticalVelocity > 0.1f)
        {
            hasHitThisFall = false;
            return;
        }

        if (hasHitThisFall)
            return;

        if (player.VerticalVelocity > minDownwardSpeed)
            return;

        hasHitThisFall = true;

        if (owner != null)
            owner.TryStomp(player.transform);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(playerTag))
            return;

        hasHitThisFall = false;
    }
}