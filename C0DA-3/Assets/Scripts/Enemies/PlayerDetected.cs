using UnityEngine;

public class PlayerDetected : MonoBehaviour
{
    /**[Header("Configuración")]
    [SerializeField] float radius = 10f;
    [SerializeField] float angle = 90f; // Para que no vea por la espalda
    [SerializeField] float checksPerSecond = 5f;
    [SerializeField] LayerMask layerMask = Physics.DefaultRaycastLayers;
    [SerializeField] Vector3 eyeOffset = new Vector3(0, 1.5f, 0); // Altura de los ojos

    private Transform player;
    private float lastCheckTime = 5f;

    void Update()
    {
        if ((Time.time - lastCheckTime) > (1f / checksPerSecond))
        {
            lastCheckTime = Time.time;
            PerformDetection();
        }
    }

    void PerformDetection()
    {
        player = null;
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, layerMask);
        
        foreach (Collider c in colliders)
        {
            if (c.CompareTag("Player"))
            {
                Vector3 directionToPlayer = (c.transform.position - transform.position).normalized;
                if (Vector3.Angle(transform.forward, directionToPlayer) < angle / 2)
                {
                    Vector3 startPos = transform.position + eyeOffset;
                    if (Physics.Raycast(startPos, directionToPlayer, out RaycastHit hit, radius, layerMask))
                    {
                        if (hit.collider == c)
                        {
                            player = c.transform;
                        }
                    }
                }
            }
        }
    }

    public Transform GetPlayerInSight()
    {
        return player;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radius);
    }**/

    
    
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


