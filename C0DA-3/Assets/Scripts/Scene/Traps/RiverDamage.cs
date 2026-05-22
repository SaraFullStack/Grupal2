using UnityEngine;

public class RiverDamage : MonoBehaviour
{
    [SerializeField] private int damageAmount = 1; // Cuánta vida quita
    [SerializeField] private float damageInterval = 1.0f; // Cada cuántos segundos quita vida

    private float nextDamageTime;

    private void OnTriggerStay(Collider other)
    {
        Health playerHealth = other.GetComponent<Health>();
        if (playerHealth != null)
        {
            if (Time.time >= nextDamageTime)
            {
                playerHealth.TakeDamage(damageAmount);
                nextDamageTime = Time.time + damageInterval;
            }
        }
    }
}