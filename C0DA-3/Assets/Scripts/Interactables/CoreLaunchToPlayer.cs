using System.Collections;
using UnityEngine;

public class CoreLaunchToPlayer : MonoBehaviour
{
    [SerializeField] private float collectDistance = 0.25f;

    private Transform player;
    private PlayerCollectibles playerCollectibles;
    private Collectible collectible;

    private float launchHeight;
    private float launchDuration;
    private float magnetSpeed;

    public void Init(float launchHeight, float launchDuration, float magnetSpeed, float targetYOffset)
    {
        this.launchHeight = launchHeight;
        this.launchDuration = launchDuration;
        this.magnetSpeed = magnetSpeed;

        PlayerCollectibles pc = FindFirstObjectByType<PlayerCollectibles>();

        if (pc != null)
        {
            playerCollectibles = pc;
            player = pc.transform;
        }
        else
        {
            return;
        }

        collectible = GetComponent<Collectible>();

        StopAllCoroutines();
        StartCoroutine(LaunchThenGoToPlayer());
    }

    private IEnumerator LaunchThenGoToPlayer()
    {
        Vector3 startPos = transform.position;
        Vector3 upPos = startPos + Vector3.up * launchHeight;

        float t = 0f;

        while (t < launchDuration)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, upPos, Mathf.Clamp01(t / launchDuration));
            yield return null;
        }

        while (player != null)
        {
            Vector3 target = player.position + Vector3.up * 0.2f;

            transform.position = Vector3.MoveTowards(
                transform.position,
                target,
                magnetSpeed * Time.deltaTime
            );

            if (Vector3.Distance(transform.position, target) <= collectDistance)
            {
                if (collectible != null)
                    collectible.ForceCollect(playerCollectibles);
                else
                    Destroy(gameObject);

                yield break;
            }

            yield return null;
        }
    }
}