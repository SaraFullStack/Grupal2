using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private bool destroyOnDeath = false;

    private int currentHealth;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
        HUDController.SetLife(currentHealth);
    }

    private void Start()
    {
        HUDController.Instance.OnHealing += OnFinishHealing;
    }

    private void OnDisable()
    {
        HUDController.Instance.OnHealing -= OnFinishHealing;
    }
    
    public void TakeDamage(int amount)
    {
        if (amount <= 0)
            return;

        currentHealth -= amount;
        Debug.Log("vida actual" + currentHealth);
        currentHealth = Mathf.Max(currentHealth, 0);

        Debug.Log($"{gameObject.name} recibió {amount} de daño. Vida: {currentHealth}/{maxHealth}");
        
        HUDController.LoseLife(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void OnFinishHealing(int lifeAfterHealing)
    {
        if (lifeAfterHealing <= maxHealth){
            currentHealth = lifeAfterHealing;
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} ha muerto.");

        if (destroyOnDeath)
        {
            Destroy(gameObject);
        }
    }
}