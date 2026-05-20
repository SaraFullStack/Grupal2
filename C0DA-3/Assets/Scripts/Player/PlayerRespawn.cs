using System.Collections;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private int waterDamage = 1;

    [Header("Audio de Muerte")]
    [SerializeField] private AudioClip splashClip;
    [SerializeField] private AudioSource audioSource;

    public void Respawn()
    {
        isDying = false; // añade esto al principio
    
        CharacterController cc = GetComponent<CharacterController>();

        if (cc != null)
        {
            cc.enabled = false;
            transform.position = spawnPoint.position;
            transform.rotation = spawnPoint.rotation;
            cc.enabled = true;
        }
        else
        {
            transform.position = spawnPoint.position;
            transform.rotation = spawnPoint.rotation;
        }
    }

    private bool isDying = false;
    public void TriggerDeath()
    {
        if (isDying) return;
        isDying = true;
        StartCoroutine(DeathSequence());
    }

    IEnumerator DeathSequence()
    {
        // 1. Primero el audio
        if (audioSource != null && splashClip != null)
        {
            audioSource.pitch = 1f;
            audioSource.volume = 1f;
            audioSource.PlayOneShot(splashClip);
        }

        float duration = 1.2f;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            if (audioSource != null)
            {
                audioSource.pitch = Mathf.Lerp(1f, 0.3f, elapsed / duration);
                audioSource.volume = Mathf.Lerp(1f, 0f, elapsed / duration);
            }
            yield return null;
        }

        // 2. Luego el daño y el respawn
        IDamageable damageable = GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(waterDamage);
        }

        Respawn();

        if (audioSource != null)
        {
            audioSource.pitch = 1f;
            audioSource.volume = 1f;
        }
    }
}