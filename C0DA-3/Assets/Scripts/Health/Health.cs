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
        if (HUDController.Instance != null)
            HUDController.Instance.OnHealing -= OnFinishHealing;
    }

    public void TakeDamage(int amount)
    {
        if (currentHealth <= 0)
            return;

        if (amount <= 0)
            return;

        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);


        HUDController.LoseLife(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void OnFinishHealing(int lifeAfterHealing)
    {
        if (lifeAfterHealing <= maxHealth)
        {
            currentHealth = lifeAfterHealing;
        }
    }

    private void Die()
    {
        if (CompareTag("Player"))
        {
            GameOverController.LaunchGameOver();

            Collider[] colliders = GetComponentsInChildren<Collider>();
            foreach (Collider col in colliders)
            {
                col.enabled = false;
            }

            PlayerController playerController = GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.enabled = false;
            }

            return;
        }

        if (destroyOnDeath)
        {
            Destroy(gameObject);
        }
    }
}