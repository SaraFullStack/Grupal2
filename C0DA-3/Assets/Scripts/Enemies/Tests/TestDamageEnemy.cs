using UnityEngine;
using UnityEngine.InputSystem;

public class TestDamageEnemy : MonoBehaviour
{
    public EnemyHealth enemyHealth;
    public float damageAmount = 10f;

    private void Update()
    {
#if UNITY_EDITOR
        if (Keyboard.current != null && Keyboard.current.gKey.wasPressedThisFrame)
        {
            if (enemyHealth != null)
                enemyHealth.TakeDamage(damageAmount);
        }
#endif
    }
}