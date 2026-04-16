using UnityEngine;
using Cinemachine;

public class CameraAutoAssign : MonoBehaviour
{
    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            var vcam = GetComponent<CinemachineVirtualCamera>();
            vcam.Follow = player.transform;
            vcam.LookAt = player.transform;
        }
    }
}