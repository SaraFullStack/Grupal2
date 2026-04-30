using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private Enemy enemy;

    public float health = 100f;
    public float maxHealth;
    public bool hit = false;

    void Awake()
    {
        enemy = GetComponent<Enemy>();

        if (enemy != null && enemy.Data != null)
            health = enemy.Data.maxHealth;

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