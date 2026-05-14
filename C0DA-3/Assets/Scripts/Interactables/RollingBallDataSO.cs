using UnityEngine;

[CreateAssetMenu(
    fileName = "RollingBallData",
    menuName = "Scriptable Objects/Hazards/Rolling Ball Data")]
public class RollingBallDataSO : ScriptableObject
{
    [Header("Prefab")]
    public GameObject ballPrefab;

    [Header("Tiempo")]
    public float spawnInterval = 3f;
    public float route2Delay = 1.5f;

    [Header("Impulso inicial")]
    public bool useInitialPush = true;
    public float initialPushForce = 2f;

    [Header("Limpieza")]
    public float maxLifeTime = 10f;
}