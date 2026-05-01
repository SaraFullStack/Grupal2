using UnityEngine;

public enum BreakableCubeMaterial
{
    Wood,
    Metal
}

[CreateAssetMenu(
    fileName = "BreakableCubeData",
    menuName = "Scriptable Objects/Interactables/Breakable Cube Data")]
public class BreakableCubeDataSO : ScriptableObject
{
    [Header("Tipo")]
    public BreakableCubeMaterial material = BreakableCubeMaterial.Wood;
    public bool isCage;

    [Header("Resistencia")]
    [Min(1)] public int hitsToBreak = 5;

    [Header("Collectibles")]
    public GameObject[] collectiblePrefabs;

    [Header("Lanzamiento collectible")]
    public float launchUpForce = 4f;
    public float launchSideForce = 0.8f;

    [Header("Rotura")]
    public GameObject breakEffectPrefab;
    public float destroyDelay = 0.08f;
}