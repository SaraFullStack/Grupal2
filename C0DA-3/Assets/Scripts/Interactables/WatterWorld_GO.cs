using UnityEngine;

public class WatterWorld_GO : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameOverController.LaunchGameOver();
            
        }
    }
}