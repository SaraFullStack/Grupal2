using UnityEngine;

public class EnemyExecuteStates : MonoBehaviour
{

    [SerializeField] Transform target;
    [SerializeField] Transform patrolPointsParent;
    [SerializeField] float reachDistance = 2f;
    [SerializeField] float initialLife = 3f;

    //NavMeshAgent agent;
    //Vision vision;

    int currenPatrolPoint = 0;
    float currentLife =10f;
    [SerializeField] float waitingTime = 3f;
    private bool hasMadeDamage = false;

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
        //agent= GetComponent<NavMeshAgent>();
        //vision = GetComponent<Vision>();
        //healthBar = GetComponentInChildren<FloatingHealthBar>();
        //health = GetComponent<Health>();
        //animator = GetComponentInChildren<Animator>();
        //currentLife = initialLife;
        //audioSources = GetComponents<AudioSource>();
        //score = FindAnyObjectByType(typeof(Score)).GetComponent<Score>();
        //healthBar.UpdateHealthBar(currentLife, initialLife);
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
