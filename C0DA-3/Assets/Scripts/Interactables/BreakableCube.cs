using System.Collections;
using UnityEngine;

public class BreakableCube : MonoBehaviour
{
    [SerializeField] private BreakableCubeDataSO data;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform visualRoot;

    [Header("Golpe visual")]
    [SerializeField] private float hitMoveAmount = 0.15f;
    [SerializeField] private float hitDuration = 0.18f;
    [SerializeField] private float hitRotateAmount = 25f;

    [Header("Rotura visual")]
    [SerializeField] private float breakSpinDuration = 0.25f;
    [SerializeField] private float breakSpinDegrees = 720f;
    [SerializeField] private float breakShrinkMultiplier = 0.05f;

    [Header("Control")]
    [SerializeField] private float stompCooldown = 0.2f;
    [SerializeField] private Collider[] collidersToDisableOnBreak;

    private int currentHits;
    private bool broken;
    private float lastHitTime = -999f;

    private Vector3 startLocalPos;
    private Quaternion startLocalRot;
    private Vector3 startLocalScale;

    private Coroutine currentAnim;

    private void Awake()
    {
        if (visualRoot == null)
            visualRoot = transform;

        startLocalPos = visualRoot.localPosition;
        startLocalRot = visualRoot.localRotation;
        startLocalScale = visualRoot.localScale;
    }

    public void TryStomp()
    {
        if (broken || data == null)
            return;

        if (Time.time < lastHitTime + stompCooldown)
            return;

        lastHitTime = Time.time;
        RegisterHit();
    }

    private void RegisterHit()
    {
        currentHits++;

        SpawnCollectible();
        PlayHitAnimation();

        if (currentHits >= data.hitsToBreak)
            StartCoroutine(BreakRoutine());
    }

    private void SpawnCollectible()
    {
        if (data.collectiblePrefab == null || spawnPoint == null)
            return;

        GameObject collectible = Instantiate(
            data.collectiblePrefab,
            spawnPoint.position,
            Quaternion.identity
        );

        Rigidbody rb = collectible.GetComponent<Rigidbody>();
        if (rb == null)
            rb = collectible.AddComponent<Rigidbody>();

        rb.useGravity = true;
        rb.isKinematic = false;
        rb.linearVelocity = Vector3.zero;

        Vector3 launchDirection = Vector3.up * data.launchUpForce;
        launchDirection += transform.right * Random.Range(-data.launchSideForce, data.launchSideForce);
        launchDirection += transform.forward * Random.Range(-data.launchSideForce, data.launchSideForce);

        rb.AddForce(launchDirection, ForceMode.Impulse);
    }

    private void PlayHitAnimation()
    {
        if (visualRoot == null)
            return;

        if (currentAnim != null)
            StopCoroutine(currentAnim);

        currentAnim = StartCoroutine(HitReaction());
    }

    private IEnumerator HitReaction()
    {
        float dir = Random.value > 0.5f ? 1f : -1f;

        Vector3 targetPos = startLocalPos + new Vector3(hitMoveAmount * dir, 0f, 0f);
        Quaternion targetRot = startLocalRot * Quaternion.Euler(0f, 0f, hitRotateAmount * dir);

        float t = 0f;

        while (t < hitDuration)
        {
            t += Time.deltaTime;
            float k = Mathf.SmoothStep(0f, 1f, t / hitDuration);

            visualRoot.localPosition = Vector3.Lerp(startLocalPos, targetPos, k);
            visualRoot.localRotation = Quaternion.Lerp(startLocalRot, targetRot, k);

            yield return null;
        }

        t = 0f;

        while (t < hitDuration)
        {
            t += Time.deltaTime;
            float k = Mathf.SmoothStep(0f, 1f, t / hitDuration);

            visualRoot.localPosition = Vector3.Lerp(targetPos, startLocalPos, k);
            visualRoot.localRotation = Quaternion.Lerp(targetRot, startLocalRot, k);

            yield return null;
        }

        visualRoot.localPosition = startLocalPos;
        visualRoot.localRotation = startLocalRot;

        currentAnim = null;
    }

    private IEnumerator BreakRoutine()
    {
        if (broken)
            yield break;

        broken = true;

        if (currentAnim != null)
            StopCoroutine(currentAnim);

        foreach (Collider col in collidersToDisableOnBreak)
        {
            if (col != null)
                col.enabled = false;
        }

        if (data.breakEffectPrefab != null)
            Instantiate(data.breakEffectPrefab, transform.position, Quaternion.identity);

        Vector3 breakStartPos = visualRoot.localPosition;
        Quaternion breakStartRot = visualRoot.localRotation;
        Vector3 breakStartScale = visualRoot.localScale;

        Vector3 breakUpPos = startLocalPos + Vector3.up * 0.6f;
        Vector3 breakEndScale = startLocalScale * breakShrinkMultiplier;

        float t = 0f;

        while (t < breakSpinDuration)
        {
            t += Time.deltaTime;
            float k = Mathf.Clamp01(t / breakSpinDuration);

            visualRoot.localPosition = Vector3.Lerp(breakStartPos, breakUpPos, k);
            visualRoot.localRotation = breakStartRot * Quaternion.Euler(0f, breakSpinDegrees * k, 0f);
            visualRoot.localScale = Vector3.Lerp(breakStartScale, breakEndScale, k);

            yield return null;
        }

        Destroy(gameObject, data.destroyDelay);
    }
}