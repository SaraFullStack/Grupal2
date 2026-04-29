using UnityEngine;

[CreateAssetMenu(fileName = "BreakableCubeData", menuName = "Scriptable Objects/Interactables/Breakable Cube Data")]
public class BreakableCubeDataSO : ScriptableObject
{
    [Header("Resistencia")]
    [Min(1)] public int hitsToBreak = 5;

    [Header("Prefab")]
    public GameObject collectiblePrefab;

    [Header("Lanzamiento del collectible")]
    public float launchUpForce = 4f;
    public float launchSideForce = 0.8f;

    [Header("Rotura")]
    public GameObject breakEffectPrefab;
    public float destroyDelay = 0.08f;
}