using UnityEngine;

public class WatterWorld_GO : MonoBehaviour
{
       void OnTriggerEnter(Collider other)
    {
        

        if (other.CompareTag("Player"))
        {
            PlayerRespawn respawn = other.GetComponent<PlayerRespawn>();
            if (respawn != null)
            {
               respawn.Respawn();
            }
            
        }
    }

}
