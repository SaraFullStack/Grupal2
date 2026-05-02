using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Image fillImage; 
    [SerializeField] private EnemyHealth healthSystem; 
    
    private Transform mainCamera;

    void Start()
    {
       // mainCamera = Camera.main.transform;
    }

    // Aquí actualizamos el valor de la barra (el llenado)
    void Update()
    {
        if (healthSystem != null && fillImage != null)
    {
        // Dividimos la vida actual entre la máxima para obtener el porcentaje (0.0 a 1.0)
        fillImage.fillAmount = healthSystem.health / healthSystem.maxHealth;
    }
    }

    // Aquí nos aseguramos de que la barra mire a la cámara sin vibraciones
    void LateUpdate()
    {
        if (mainCamera != null)
        {
            // Mira hacia la cámara pero manteniendo la rotación frontal
            transform.LookAt(transform.position + mainCamera.forward);
        }
    }
}
