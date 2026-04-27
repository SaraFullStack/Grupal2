using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
   public float health = 100f;
    public float maxHealth;
    public bool hit = false;

    void Awake() 
    {
        maxHealth = health;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        hit = true;
        if (health <= 0)
        {
            health = 0; // Evita valores negativos
        }
    }
}
