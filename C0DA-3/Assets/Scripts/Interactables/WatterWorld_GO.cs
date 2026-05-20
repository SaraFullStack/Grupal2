using UnityEngine;

public class WatterWorld_GO : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("WatterWorld tocado por: " + other.gameObject.name + " tag: " + other.gameObject.tag);
        if (other.CompareTag("Player"))
        {
            PlayerRespawn respawn = other.GetComponent<PlayerRespawn>();
            Debug.Log("PlayerRespawn encontrado: " + respawn);
            if (respawn != null)
            {
                respawn.TriggerDeath();
            }
        }
    }
}