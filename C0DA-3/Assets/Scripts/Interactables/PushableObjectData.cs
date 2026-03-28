using UnityEngine;

[CreateAssetMenu(fileName = "PushableObjectData", menuName = "Scriptable Objects/Interactables/Pushable Object Data")]
public class PushableObjectData : ScriptableObject
{
    [Header("Movement")]
    public float moveSpeed = 2f;
    public float smoothness = 10f;

    [Header("Axis")]
    public bool allowPushX = true;
    public bool allowPushZ = true;
}