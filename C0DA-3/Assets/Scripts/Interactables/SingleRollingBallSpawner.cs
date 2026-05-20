using System.Collections;
using UnityEngine;

public class SingleRollingBallSpawner : MonoBehaviour
{
    [Header("Datos")]
    [SerializeField] private RollingBallDataSO data;

    [Header("Ancho del punto de salida")]
    [SerializeField] private float spawnWidth = 1.2f;
    [SerializeField] private Vector3 widthAxis = Vector3.right;

    [Header("Ajuste al suelo")]
    [SerializeField] private float rayStartHeight = 10f;
    [SerializeField] private float groundOffset = 0.6f;
    [SerializeField] private LayerMask groundLayers = Physics.DefaultRaycastLayers;

    [Header("Orden de salida")]
    [SerializeField] private float initialDelay = 0f;

    private Coroutine spawnCoroutine;
    private int laneIndex;

    private void OnEnable()
    {
        spawnCoroutine = StartCoroutine(SpawnRoutine());
    }

    private void OnDisable()
    {
        if (spawnCoroutine != null)
            StopCoroutine(spawnCoroutine);
    }

    private IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(initialDelay);

        while (true)
        {
            SpawnBall();

            float interval = data != null ? Mathf.Max(0.1f, data.spawnInterval) : 2.5f;
            yield return new WaitForSeconds(interval);
        }
    }

    private void SpawnBall()
    {
        if (data == null || data.ballPrefab == null)
            return;

        Vector3 axis = widthAxis.normalized;
        if (axis == Vector3.zero)
            axis = Vector3.right;

        float offset = GetLaneOffset();

        Vector3 basePosition = transform.position + axis * offset;
        Vector3 spawnPosition = GetGroundedSpawnPosition(basePosition);

        GameObject ball = Instantiate(data.ballPrefab, spawnPosition, Quaternion.identity);

        Rigidbody rb = ball.GetComponent<Rigidbody>();

        if (rb == null)
            return;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    private float GetLaneOffset()
    {
        float[] lanes =
        {
            0f,
            -spawnWidth * 0.5f,
            spawnWidth * 0.5f
        };

        float offset = lanes[laneIndex];
        laneIndex = (laneIndex + 1) % lanes.Length;

        return offset;
    }

    private Vector3 GetGroundedSpawnPosition(Vector3 basePosition)
    {
        Vector3 rayOrigin = basePosition + Vector3.up * rayStartHeight;

        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, rayStartHeight * 2f, groundLayers))
            return hit.point + Vector3.up * groundOffset;

        return basePosition;
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 axis = widthAxis.normalized;
        if (axis == Vector3.zero)
            axis = Vector3.right;

        Vector3 center = transform.position;
        Vector3 left = transform.position - axis * spawnWidth * 0.5f;
        Vector3 right = transform.position + axis * spawnWidth * 0.5f;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(center, 0.35f);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(left, 0.35f);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(right, 0.35f);

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(left, right);
    }
}