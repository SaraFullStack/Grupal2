using UnityEngine;

public class RiverDamage : MonoBehaviour
{
    [SerializeField] private int damageAmount = 1; // Cuánta vida quita
    [SerializeField] private float damageInterval = 1.0f; // Cada cuántos segundos quita vida

    private float nextDamageTime;

    private void OnTriggerStay(Collider other)
    {
        // Buscamos si el objeto que entró tiene tu script de Health
        Health playerHealth = other.GetComponent<Health>();
        Debug.Log("vida actual" + playerHealth);
        if (playerHealth != null)
        {
            // Verificamos si ha pasado el tiempo suficiente para volver a hacer daño
            if (Time.time >= nextDamageTime)
            {
                playerHealth.TakeDamage(damageAmount);
                nextDamageTime = Time.time + damageInterval;
            }
        }
    }
}