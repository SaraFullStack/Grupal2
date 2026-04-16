using UnityEngine;

[CreateAssetMenu(
    fileName = "PoisonMushroomData",
    menuName = "Scriptable Objects/Enemies/Poison Mushroom Data")]
public class PoisonMushroomDataSO : ScriptableObject
{
    public int touchDamage = 1;
}