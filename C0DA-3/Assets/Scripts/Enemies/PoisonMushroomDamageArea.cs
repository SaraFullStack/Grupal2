using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PoisonMushroomDamageArea : MonoBehaviour
{
    [SerializeField] private PoisonMushroom owner;

    private readonly HashSet<GameObject> objectsInside = new();

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void Awake()
    {
        if (owner == null)
            owner = GetComponentInParent<PoisonMushroom>();
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.GetComponentInParent<IDamageable>();
        if (damageable == null)
            return;

        GameObject rootObject = other.transform.root.gameObject;

        if (objectsInside.Contains(rootObject))
            return;

        objectsInside.Add(rootObject);

        int damage = 1;

        if (owner != null && owner.Data != null)
            damage = owner.Data.touchDamage;
        damageable.TakeDamage(damage);
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject rootObject = other.transform.root.gameObject;

        if (objectsInside.Contains(rootObject))
            objectsInside.Remove(rootObject);
    }
}