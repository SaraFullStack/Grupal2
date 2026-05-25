using UnityEngine;

public class WatterWorld_GO : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
if (other.CompareTag("Player"))
        {
            Debug.Log("bug");
            Debug.Log(other.transform.position);
            GameOverController.LaunchGameOver();
        }
    }
}