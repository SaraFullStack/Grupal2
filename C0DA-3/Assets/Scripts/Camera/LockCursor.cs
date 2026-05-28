using UnityEngine;

public class LockCursor : MonoBehaviour
{
    private void Start()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
#elif UNITY_EDITOR
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
#else
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
#endif
    }
}