using UnityEngine;

public class PlayerDetected : MonoBehaviour
{
    [SerializeField] float radius =10f;
    [SerializeField] float checksPerSecond;
    [SerializeField] LayerMask layerMask = Physics.DefaultRaycastLayers;

    Transform player;
    float lastCheckTime =0f;
    void Start()
    {
        
    }
    void Update()
    {
        if ((Time.time - lastCheckTime) > (1f / checksPerSecond))
        {
            lastCheckTime = Time.time;
            player = null;
            Collider[] colliders = Physics.OverlapSphere(transform.position, radius,layerMask);
            foreach ( Collider c in colliders)
            {

                if (c.CompareTag("Player"))
                {
                    Vector3 direction = c.transform.position - transform.position;
                    if (Physics.Raycast(transform.position, direction, out RaycastHit hit))
                    {
                        if (hit.collider == c)
                        {
                             player=c.transform;
                        }
                    }
                   

                }
            }
        }
         
        
    }

    public Transform GetPlayerInSight ()
    {
        return player;
    }

}


