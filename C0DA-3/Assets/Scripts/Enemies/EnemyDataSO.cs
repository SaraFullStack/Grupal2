using UnityEngine;

[CreateAssetMenu(
    fileName = "EnemyData",
    menuName = "Scriptable Objects/Enemies/Enemy Data")]
public class EnemyDataSO : ScriptableObject
{
    public float maxHealth = 3f;
    public float reachDistance = 2f;
    public float detectionRadius = 10f;
}