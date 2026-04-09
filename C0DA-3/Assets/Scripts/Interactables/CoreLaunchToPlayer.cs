using System.Collections;
using UnityEngine;

public class CoreLaunchToPlayer : MonoBehaviour
{
    private Transform player;

    private float launchHeight;
    private float launchDuration;
    private float magnetSpeed;
    private float targetYOffset;

    public void Init(float launchHeight, float launchDuration, float magnetSpeed, float targetYOffset)
    {
        this.launchHeight = launchHeight;
        this.launchDuration = launchDuration;
        this.magnetSpeed = magnetSpeed;
        this.targetYOffset = targetYOffset;

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
            player = p.transform;

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
            float k = t / launchDuration;
            transform.position = Vector3.Lerp(startPos, upPos, k);
            yield return null;
        }

        while (player != null)
        {
            Vector3 target = player.position + Vector3.up * targetYOffset;

            transform.position = Vector3.MoveTowards(
                transform.position,
                target,
                magnetSpeed * Time.deltaTime
            );

            yield return null;
        }
    }
}