using UnityEngine;

public class Collectible : MonoBehaviour
{
    [Header("Collectible Settings")]
    [SerializeField] private int amount = 1;
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private AudioClip collectSound;
    [SerializeField] private GameObject collectEffect;

    private bool collected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (collected) return;

        if (!other.CompareTag(playerTag)) return;

        PlayerCollectibles playerCollectibles = other.GetComponent<PlayerCollectibles>();

        if (playerCollectibles != null)
        {
            collected = true;
            playerCollectibles.AddCollectible(amount);

            if (collectEffect != null)
            {
                Instantiate(collectEffect, transform.position, Quaternion.identity);
            }

            if (collectSound != null)
            {
                AudioSource.PlayClipAtPoint(collectSound, transform.position);
            }

            Destroy(gameObject);
        }
        else
        {
            Debug.LogWarning("El Player no tiene PlayerCollectibles.");
        }
    }
}