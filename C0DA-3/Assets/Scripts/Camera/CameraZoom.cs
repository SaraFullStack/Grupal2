using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] private float zoomAmount = 2.5f;
    [SerializeField] private float smoothSpeed = 8f;
    [SerializeField] private float minDistance = -6f;   
    [SerializeField] private float maxDistance = -18f;  

    private CinemachineVirtualCamera vcam;
    private CinemachineTransposer transposer;
    private float targetZ;

    private void Awake()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
        transposer = vcam.GetCinemachineComponent<CinemachineTransposer>();
    }

    private void Start()
    {
        if (transposer == null)
            return;

        targetZ = minDistance;

        Vector3 offset = transposer.m_FollowOffset;
        offset.z = targetZ;
        transposer.m_FollowOffset = offset;
    }

    private void Update()
    {
        if (Mouse.current == null || transposer == null)
            return;

        float wheel = Mouse.current.scroll.ReadValue().y;

        if (wheel > 0f)
        {
            targetZ += zoomAmount;
        }
        else if (wheel < 0f)
        {
            targetZ -= zoomAmount;
        }

        targetZ = Mathf.Clamp(targetZ, maxDistance, minDistance);

        Vector3 offset = transposer.m_FollowOffset;
        offset.z = Mathf.Lerp(offset.z, targetZ, smoothSpeed * Time.deltaTime);
        transposer.m_FollowOffset = offset;
    }
}