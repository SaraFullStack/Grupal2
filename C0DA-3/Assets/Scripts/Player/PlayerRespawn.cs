using UnityEngine;
using System.Collections;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    
    [Header("Audio de Muerte")]
    [SerializeField] private AudioClip splashClip;
    [SerializeField] private AudioSource audioSource;

    private Health Health;

    public void Respawn()
    {
        CharacterController cc = GetComponent<CharacterController>();
    
        
        if (cc != null)
        {
            cc.enabled = false;
            transform.position = spawnPoint.position;
            transform.rotation = spawnPoint.rotation;
            cc.enabled = true;
        }
        else
        {
            transform.position = spawnPoint.position;
            transform.rotation = spawnPoint.rotation;
        }
        Health.TakeDamage(3);
        
    }

    

}
