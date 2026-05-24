using UnityEngine;

public class RiverDamage : MonoBehaviour
{
    [SerializeField] private int damageAmount = 1; 
    [SerializeField] private float damageInterval = 1.0f; 

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