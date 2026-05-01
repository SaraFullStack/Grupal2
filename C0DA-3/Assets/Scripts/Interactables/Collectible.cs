using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] private int amount = 1;
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private CollectibleType collectibleType = CollectibleType.EnergyCore;

    [Header("Recogida")]
    [SerializeField] private float pickupDelay = 0.35f;

    private bool collected;
    private float spawnTime;

    private void Awake()
    {
        spawnTime = Time.time;
    }

    private void OnTriggerEnter(Collider other)
    {
        TryCollect(other);
    }

    private void OnTriggerStay(Collider other)
    {
        TryCollect(other);
    }

    private void TryCollect(Collider other)
    {
        if (collected)
            return;

        if (Time.time < spawnTime + pickupDelay)
            return;

        if (!other.CompareTag(playerTag))
            return;

        PlayerCollectibles playerCollectibles = other.GetComponentInParent<PlayerCollectibles>();
        if (playerCollectibles == null)
            return;

        collected = true;
        playerCollectibles.AddCollectible(amount, collectibleType);
        Destroy(gameObject);
    }
    public void ForceCollect(PlayerCollectibles playerCollectibles)
    {
        if (collected)
            return;

        if (playerCollectibles == null)
            return;

        collected = true;
        playerCollectibles.AddCollectible(amount, collectibleType);
        Destroy(gameObject);
    }
}