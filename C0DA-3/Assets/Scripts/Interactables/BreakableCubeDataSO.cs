using UnityEngine;

[CreateAssetMenu(fileName = "BreakableCubeData", menuName = "Scriptable Objects/Interactables/Breakable Cube Data")]
public class BreakableCubeDataSO : ScriptableObject
{
    [Header("Resistencia")]
    [Min(1)] public int hitsToBreak = 5;

    [Header("Prefab")]
    public GameObject collectiblePrefab;

    [Header("Movimiento del collectible")]
    public float launchHeight = 1.4f;
    public float launchDuration = 0.12f;
    public float magnetSpeed = 9f;
    public float targetYOffset = 1f;

    [Header("Rotura")]
    public GameObject breakEffectPrefab;
    public float destroyDelay = 0.08f;
}