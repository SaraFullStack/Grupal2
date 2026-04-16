using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] private int amount = 1;
    [SerializeField] private string playerTag = "Player";

    private bool collected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (collected) return;
        if (!other.CompareTag(playerTag)) return;

        PlayerCollectibles playerCollectibles = other.GetComponentInParent<PlayerCollectibles>();
        if (playerCollectibles == null) return;

        collected = true;
        playerCollectibles.AddCollectible(amount);
        Destroy(gameObject);
    }
}