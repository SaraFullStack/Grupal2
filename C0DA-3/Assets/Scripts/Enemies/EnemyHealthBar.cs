using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Image fillImage; 
    [SerializeField] private EnemyHealth healthSystem; 
    
    private Transform mainCamera;

    void Start()
    {
    }
    void Update()
    {
        if (healthSystem != null && fillImage != null)
    {
        fillImage.fillAmount = healthSystem.health / healthSystem.maxHealth;
    }
    }
    void LateUpdate()
    {
        if (mainCamera != null)
        {
            transform.LookAt(transform.position + mainCamera.forward);
        }
    }
}
