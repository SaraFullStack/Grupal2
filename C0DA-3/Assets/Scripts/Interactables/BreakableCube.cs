using System.Collections;
using UnityEngine;

public class BreakableCube : MonoBehaviour
{
    [Header("Datos")]
    [SerializeField] private BreakableCubeDataSO data;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform visualRoot;

    [Header("Jaula")]
    [SerializeField] private GameObject containedNpc;
    [SerializeField] private Transform playerExitPoint;
    [SerializeField] private float playerExitYOffset = 0.1f;

    [Header("Golpe visual")]
    [SerializeField] private float hitShakeAngle = 20f;
    [SerializeField] private float hitShakeDuration = 0.14f;
    [SerializeField] private float hitShakeSpeed = 95f;

    [Header("Rotura visual")]
    [SerializeField] private float breakSpinDuration = 0.25f;
    [SerializeField] private float breakShrinkMultiplier = 0.05f;

    [Header("Control")]
    [SerializeField] private float hitCooldown = 0.25f;
    [SerializeField] private Collider[] collidersToDisableOnBreak;

    [Header("Dialogo")]
    [SerializeField] private DialogType clawUnlockDialog = DialogType.BearClawUnlock;
    [SerializeField] private float dialogDelayAfterBreak = 1.2f;

    private int currentHits;
    private bool broken;
    private bool isHitAnimating;
    private float lastHitTime = -999f;

    private Vector3 startLocalPosition;
    private Quaternion startLocalRotation;
    private Vector3 startLocalScale;

    private Coroutine currentAnimation;

    private void Awake()
    {
        if (visualRoot == null)
            visualRoot = transform;

        startLocalPosition = visualRoot.localPosition;
        startLocalRotation = visualRoot.localRotation;
        startLocalScale = visualRoot.localScale;

        FreezeContainedNpc();
    }

    public void TryStomp(Transform player)
    {
        if (!CanReceiveHit())
            return;

        if (!data.isCage && data.material != BreakableCubeMaterial.Wood)
            return;

        ApplyHit(player);
    }

    public void TryClaw(Transform player = null)
    {
        if (!CanReceiveHit())
            return;

        ApplyHit(player);
    }

    private bool CanReceiveHit()
    {
        if (broken || data == null)
            return false;

        if (currentHits >= data.hitsToBreak)
            return false;

        if (isHitAnimating)
            return false;

        if (Time.time < lastHitTime + hitCooldown)
            return false;

        return true;
    }

    private void ApplyHit(Transform player)
    {
        lastHitTime = Time.time;
        currentHits++;

        if (!data.isCage)
            SpawnCollectible();

        PlayHitAnimation();

        if (currentHits >= data.hitsToBreak)
            StartCoroutine(BreakRoutine(player));
    }

    private IEnumerator BreakRoutine(Transform player)
    {
        if (broken)
            yield break;

        broken = true;

        if (currentAnimation != null)
            StopCoroutine(currentAnimation);

        DisableColliders();

        bool shouldLaunchDialog = false;

        if (data.isCage)
        {
            ReleaseContainedNpc();
            shouldLaunchDialog = GiveClawToPlayer(player);
        }

        SpawnBreakEffect();

        yield return PlayBreakAnimation();

        HideVisuals();

        if (shouldLaunchDialog)
        {
            yield return new WaitForSeconds(dialogDelayAfterBreak);

            if (DialogController.Instance != null)
                DialogController.LaunchDialog(clawUnlockDialog);
}

        Destroy(gameObject);
    }

    private void HideVisuals()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        foreach (Renderer r in renderers)
            r.enabled = false;
    }

    private void FreezeContainedNpc()
    {
        if (containedNpc == null)
            return;

        Rigidbody rb = containedNpc.GetComponent<Rigidbody>();

        if (rb == null)
            return;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.useGravity = false;
        rb.isKinematic = true;
    }

    private void ReleaseContainedNpc()
    {
        if (containedNpc == null)
            return;

        containedNpc.transform.SetParent(null, true);
        containedNpc.SetActive(true);

        Rigidbody rb = containedNpc.GetComponent<Rigidbody>();

        if (rb == null)
            rb = containedNpc.AddComponent<Rigidbody>();

        rb.useGravity = true;
        rb.isKinematic = false;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void MovePlayerToExitPoint(Transform player)
    {
        if (player == null || playerExitPoint == null)
            return;

        PlayerController playerController = player.GetComponent<PlayerController>();
        CharacterController characterController = player.GetComponent<CharacterController>();

        Vector3 targetPosition = playerExitPoint.position + Vector3.up * playerExitYOffset;

        if (playerController != null)
            playerController.enabled = false;

        if (characterController != null)
            characterController.enabled = false;

        player.position = targetPosition;

        if (containedNpc != null)
        {
            Vector3 dir = containedNpc.transform.position - player.position;
            dir.y = 0f;

            if (dir.sqrMagnitude > 0.001f)
                player.rotation = Quaternion.LookRotation(dir);
        }

        if (characterController != null)
            characterController.enabled = true;

        if (playerController != null)
        {
            playerController.enabled = true;
            playerController.ForceJump(-2f);
        }
    }
    
    private void DisableColliders()
    {
        foreach (Collider col in collidersToDisableOnBreak)
        {
            if (col != null)
                col.enabled = false;
        }

        Collider ownCollider = GetComponent<Collider>();

        if (ownCollider != null)
            ownCollider.enabled = false;
    }

    private void SpawnCollectible()
    {
        if (spawnPoint == null)
            return;

        if (data.collectiblePrefabs == null || data.collectiblePrefabs.Length == 0)
            return;

        GameObject prefab = data.collectiblePrefabs[Random.Range(0, data.collectiblePrefabs.Length)];
        GameObject collectible = Instantiate(prefab, spawnPoint.position, Quaternion.identity);

        Rigidbody rb = collectible.GetComponent<Rigidbody>();

        if (rb != null)
            Destroy(rb);

        CoreLaunchToPlayer magnet = collectible.GetComponent<CoreLaunchToPlayer>();

        if (magnet == null)
            magnet = collectible.AddComponent<CoreLaunchToPlayer>();

        magnet.Init(data.launchUpForce, 0.35f, 8f, 1.2f);
    }

    private void SpawnBreakEffect()
    {
        if (data == null || data.breakEffectPrefab == null)
            return;

        Instantiate(data.breakEffectPrefab, transform.position, Quaternion.identity);
    }

    private void PlayHitAnimation()
    {
        if (visualRoot == null)
            return;

        if (currentAnimation != null)
            StopCoroutine(currentAnimation);

        currentAnimation = StartCoroutine(HitShakeRoutine());
    }

    private IEnumerator HitShakeRoutine()
    {
        isHitAnimating = true;

        float t = 0f;

        while (t < hitShakeDuration)
        {
            t += Time.deltaTime;

            float normalized = t / hitShakeDuration;
            float fade = 1f - normalized;
            float shake = Mathf.Sin(t * hitShakeSpeed) * hitShakeAngle * fade;

            visualRoot.localPosition = startLocalPosition;
            visualRoot.localRotation = startLocalRotation * Quaternion.Euler(0f, 0f, shake);

            yield return null;
        }

        visualRoot.localPosition = startLocalPosition;
        visualRoot.localRotation = startLocalRotation;

        isHitAnimating = false;
        currentAnimation = null;
    }

    private IEnumerator PlayBreakAnimation()
    {
        if (visualRoot == null)
            yield break;

        Vector3 breakStartPosition = visualRoot.localPosition;
        Quaternion breakStartRotation = visualRoot.localRotation;
        Vector3 breakStartScale = visualRoot.localScale;

        Vector3 breakEndPosition = startLocalPosition + Vector3.up * 0.6f;
        Vector3 breakEndScale = startLocalScale * breakShrinkMultiplier;
        Quaternion breakEndRotation = startLocalRotation;

        float t = 0f;

        while (t < breakSpinDuration)
        {
            t += Time.deltaTime;
            float k = Mathf.Clamp01(t / breakSpinDuration);

            visualRoot.localPosition = Vector3.Lerp(breakStartPosition, breakEndPosition, k);
            visualRoot.localRotation = Quaternion.Lerp(breakStartRotation, breakEndRotation, k);
            visualRoot.localScale = Vector3.Lerp(breakStartScale, breakEndScale, k);

            yield return null;
        }

        visualRoot.localPosition = breakEndPosition;
        visualRoot.localRotation = breakEndRotation;
        visualRoot.localScale = breakEndScale;
    }

    private bool GiveClawToPlayer(Transform player)
    {
       if (player == null)
        {
            return false;
        }

        PlayerController pc = player.GetComponent<PlayerController>();

         if (pc == null)
        {
            return false;
        }
        
        pc.hasClaw = true;
        pc.gameData.hasClaw = true;

        pc.claw1.SetActive(true);
        pc.claw2.SetActive(true);

        return true;
    }
}