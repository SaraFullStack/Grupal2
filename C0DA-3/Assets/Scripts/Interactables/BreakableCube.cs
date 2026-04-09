using System.Collections;
using UnityEngine;

public class BreakableCube : MonoBehaviour
{
    [SerializeField] private BreakableCubeDataSO data;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform visualRoot;

    [Header("Golpe exagerado")]
    [SerializeField] private float hitMoveAmount = 0.8f;
    [SerializeField] private float hitMoveDuration = 0.12f;
    [SerializeField] private float hitRotateAmount = 45f;
    [SerializeField] private float squashAmount = 0.28f;
    [SerializeField] private int shakeFrames = 5;
    [SerializeField] private float shakeAmount = 0.08f;

    [Header("Rotura")]
    [SerializeField] private float breakSpinDuration = 0.25f;
    [SerializeField] private float breakSpinDegrees = 720f;
    [SerializeField] private float breakShrinkMultiplier = 0.05f;

    [Header("Control")]
    [SerializeField] private float stompCooldown = 0.15f;

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

    private void PlayHitAnimation()
    {
        if (currentAnim != null)
            StopCoroutine(currentAnim);

        currentAnim = StartCoroutine(HitReaction());
    }

    private IEnumerator HitReaction()
    {
        float dir = Random.value > 0.5f ? 1f : -1f;

        float duration = 0.2f; // MÁS LENTO
        float moveAmount = 0.15f; // MÁS AMPLIO pero suave
        float rotAmount = hitRotateAmount;

        Vector3 targetPos = startLocalPos + new Vector3(moveAmount * dir, 0f, 0f);
        Quaternion targetRot = startLocalRot * Quaternion.Euler(0f, 0f, rotAmount * dir);

        float t = 0f;

        // Empuje inicial (suave)
        while (t < duration * 0.5f)
        {
            t += Time.deltaTime;
            float k = Mathf.SmoothStep(0f, 1f, t / (duration * 0.5f));

            visualRoot.localPosition = Vector3.Lerp(startLocalPos, targetPos, k);
            visualRoot.localRotation = Quaternion.Lerp(startLocalRot, targetRot, k);
            yield return null;
        }

        t = 0f;

        // Vuelve lento
        while (t < duration)
        {
            t += Time.deltaTime;
            float k = Mathf.SmoothStep(0f, 1f, t / duration);

            visualRoot.localPosition = Vector3.Lerp(targetPos, startLocalPos, k);
            visualRoot.localRotation = Quaternion.Lerp(targetRot, startLocalRot, k);
            yield return null;
        }

        // Pequeño rebote final (muy sutil)
        t = 0f;
        Vector3 smallOffset = startLocalPos + new Vector3(-moveAmount * 0.25f * dir, 0f, 0f);

        while (t < duration * 0.3f)
        {
            t += Time.deltaTime;
            float k = t / (duration * 0.3f);

            visualRoot.localPosition = Vector3.Lerp(startLocalPos, smallOffset, k);
            yield return null;
        }

        t = 0f;
        while (t < duration * 0.3f)
        {
            t += Time.deltaTime;
            float k = t / (duration * 0.3f);

            visualRoot.localPosition = Vector3.Lerp(smallOffset, startLocalPos, k);
            yield return null;
        }

        visualRoot.localPosition = startLocalPos;
        visualRoot.localRotation = startLocalRot;

        currentAnim = null;
    }

    private void SpawnCollectible()
    {
        if (data.collectiblePrefab == null || spawnPoint == null)
            return;

        GameObject core = Instantiate(data.collectiblePrefab, spawnPoint.position, Quaternion.identity);

        CoreLaunchToPlayer mover = core.GetComponent<CoreLaunchToPlayer>();
        if (mover == null)
            mover = core.AddComponent<CoreLaunchToPlayer>();

        mover.Init(data.launchHeight, data.launchDuration, data.magnetSpeed, data.targetYOffset);
    }

    private IEnumerator BreakRoutine()
    {
        if (broken)
            yield break;

        broken = true;

        if (currentAnim != null)
            StopCoroutine(currentAnim);

        Vector3 breakStartPos = visualRoot.localPosition;
        Quaternion breakStartRot = visualRoot.localRotation;
        Vector3 breakStartScale = visualRoot.localScale;

        Vector3 breakUpPos = startLocalPos + Vector3.up * (hitMoveAmount * 1.2f);
        Vector3 breakEndScale = startLocalScale * breakShrinkMultiplier;

        float t = 0f;
        while (t < breakSpinDuration)
        {
            t += Time.deltaTime;
            float k = t / breakSpinDuration;

            visualRoot.localPosition = Vector3.Lerp(breakStartPos, breakUpPos, k);
            visualRoot.localRotation = breakStartRot * Quaternion.Euler(0f, breakSpinDegrees * k, 0f);
            visualRoot.localScale = Vector3.Lerp(breakStartScale, breakEndScale, k);

            yield return null;
        }

        Destroy(gameObject);
    }
}