using UnityEngine;
using UnityEngine.InputSystem; 

public class TestDamageEnemy : MonoBehaviour
{
   public EnemyHealth enemyHealth; 
    public float damageAmount = 10f;

    void Update()
    {
        // Esta es la forma rápida del nuevo Input System para detectar una tecla
        if (Keyboard.current.gKey.wasPressedThisFrame) 
        {
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damageAmount);
                Debug.Log("¡Daño aplicado! Vida: " + enemyHealth.health);
            }
        }
    }
}
