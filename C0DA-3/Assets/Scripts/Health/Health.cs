using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 5;
    [SerializeField] private bool destroyOnDeath = false;

    private int currentHealth;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

<<<<<<< Updated upstream
=======
    private void Start()
    {
        if (HUDController.Instance != null)
        HUDController.Instance.OnHealing += OnFinishHealing;
    }

    private void OnDisable()
    {
        if (HUDController.Instance != null)
        HUDController.Instance.OnHealing -= OnFinishHealing;
    }
    
>>>>>>> Stashed changes
    public void TakeDamage(int amount)
    {
        if (amount <= 0)
            return;

        currentHealth -= amount;
        Debug.Log("vida actual" + currentHealth);
        currentHealth = Mathf.Max(currentHealth, 0);

        Debug.Log($"{gameObject.name} recibió {amount} de daño. Vida: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
        {
            Die();
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