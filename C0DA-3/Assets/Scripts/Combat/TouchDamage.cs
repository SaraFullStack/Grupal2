using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TouchDamage : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] private int damage = 1;
    [SerializeField] private LayerMask targetLayers;
    [SerializeField] private bool damageOncePerContact = true;
    [SerializeField] private float repeatDamageCooldown = 0.5f;

    [Header("Push")]
    [SerializeField] private bool applyKnockback = false;
    [SerializeField] private float knockbackForce = 5f;

    private readonly Dictionary<Collider, float> lastDamageTime = new();

    private void Reset()
    {
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        TryDamage(other);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!damageOncePerContact)
        {
            TryDamage(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (lastDamageTime.ContainsKey(other))
            lastDamageTime.Remove(other);
    }

    private void TryDamage(Collider other)
    {
        if ((targetLayers.value & (1 << other.gameObject.layer)) == 0)
            return;

        if (damageOncePerContact && lastDamageTime.ContainsKey(other))
            return;

        if (!damageOncePerContact &&
            lastDamageTime.TryGetValue(other, out float time) &&
            Time.time < time + repeatDamageCooldown)
            return;

        IDamageable damageable = other.GetComponentInParent<IDamageable>();
        if (damageable == null)
            return;

        Vector3 dir = (other.transform.position - transform.position).normalized;

        damageable.TakeDamage(damage);

        if (applyKnockback)
        {
            Rigidbody rb = other.GetComponentInParent<Rigidbody>();
            if (rb != null)
            {
                Vector3 forceDir = new Vector3(dir.x, 0.35f, dir.z).normalized;
                rb.AddForce(forceDir * knockbackForce, ForceMode.Impulse);
            }
        }

        lastDamageTime[other] = Time.time;
    }
}