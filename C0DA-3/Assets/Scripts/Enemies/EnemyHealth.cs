using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   public float health = 100f;
    public bool hit = false;

    public void TakeDamage(float amount)
    {
        health -= amount;
        hit = true;
        if (health <= 0)
        {
            // Lógica de muerte si fuera necesaria aquí
        }
    }
}
