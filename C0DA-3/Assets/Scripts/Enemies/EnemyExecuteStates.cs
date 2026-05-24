using UnityEngine;

public class EnemyExecuteStates : MonoBehaviour
{

    [SerializeField] Transform target;
    [SerializeField] Transform patrolPointsParent;

    enum State
    {
        Patrol,
        Following,
        Death,
        Waiting,
        Atack,
    }

    State currentState;
    void Awake()
    {
    }
    void Start()
    {
        
    }
    void Update()
    {
        
    }
}
