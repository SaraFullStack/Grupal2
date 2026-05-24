using UnityEngine;
using UnityEngine.InputSystem; 

public class TestDamageEnemy : MonoBehaviour
{
   public EnemyHealth enemyHealth; 
    public float damageAmount = 10f;

    void Update()
    {
        if (Keyboard.current.gKey.wasPressedThisFrame) 
        {
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damageAmount);
}
        }
    }
}
