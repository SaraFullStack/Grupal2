using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ClawAttackHitbox : MonoBehaviour
{
    [Header("Ataque")]
    [SerializeField] private float activeTime = 0.15f;
    [SerializeField] private int enemyDamage = 10;

    [Header("Sonido")]
    [SerializeField] private AudioClip clawHitSound;
    [SerializeField] private float hitSoundVolume = 1f;

    private Collider hitbox;
    private Coroutine attackCoroutine;

    private void Awake()
    {
        hitbox = GetComponent<Collider>();

        if (hitbox == null)
        {
            return;
        }

        hitbox.isTrigger = true;
        hitbox.enabled = false;
    }

    private void Update()
    {
        PlayerController player = GetComponentInParent<PlayerController>();

        if (player == null || !player.hasClaw)
            return;

        if (InputManager.attackWasPressed || Keyboard.current.eKey.wasPressedThisFrame)
            ActivateAttack();
    }

    private void ActivateAttack()
    {
        if (attackCoroutine != null)
            StopCoroutine(attackCoroutine);

        attackCoroutine = StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        hitbox.enabled = true;

        yield return new WaitForSeconds(activeTime);

        hitbox.enabled = false;
        attackCoroutine = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        BreakableCube cube = other.GetComponentInParent<BreakableCube>();

        if (cube != null)
        {
            cube.TryClaw(transform.root);
            PlayHitSound();
            return;
        }

        EnemyHealth enemy = other.GetComponentInParent<EnemyHealth>();

        if (enemy != null)
        {
            enemy.TakeDamage(enemyDamage);
            PlayHitSound();
        }
    }

    private void PlayHitSound()
    {
        if (clawHitSound == null)
            return;

        AudioSource.PlayClipAtPoint(clawHitSound, transform.position, hitSoundVolume);
    }
}