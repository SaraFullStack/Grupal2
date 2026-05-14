using UnityEngine;

public class DestroyRollingBallZone : MonoBehaviour
{
    [SerializeField] private string ballTag = "RollingBall";

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(ballTag))
            return;

        Destroy(other.gameObject);
    }
}