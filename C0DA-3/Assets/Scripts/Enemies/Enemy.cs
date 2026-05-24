using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
   [Header("Configuración")]
    public Transform target;
    public Transform patrolPointsParent;
    public float reachDistance = 2f;
    public Canvas canvasEnemyDetection;
    public float waitingTime = 3f;

    [Header("Ataque")]
    public float attackRange = 2f;
    public int attackDamage = 1;
    public float attackCooldown = 1.5f;
    public float lastAttackTime = 0;
    [Range(0f, 360f)] public float attackAngle = 180f;   

    [Header("Componentes")]
    public NavMeshAgent agent;
    public Animator animator;
    public EnemyHealth health;
    private PlayerDetected _PlayerDetected;
    private AudioSource[] audioSources;

    private InterfaceEnemyStates ActualSatate;
    public bool hasMadeDamage = false;
    public float lastTimeDamage = 0;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        _PlayerDetected = GetComponent<PlayerDetected>();
        health = GetComponent<EnemyHealth>();
        animator = GetComponentInChildren<Animator>();
        audioSources = GetComponents<AudioSource>();
    }

    void Start()
    {
        ChangeState(new PatrolState(this));
    }

    void Update()
    {
        target = _PlayerDetected.GetPlayerInSight(); 

        SelectNextState();
        ActualSatate?.Update();
    }

    void SelectNextState()
    {
        if (ActualSatate.GetType() == typeof(DeathState))
            return;
        if (health.health <= 0)
        {
            ChangeState(new DeathState(this));
            return;
        }

        bool playerVisible = target != null;
        bool playerInRange = playerVisible &&
            Vector3.Distance(transform.position, target.position) <= attackRange;
        if (playerInRange && ActualSatate.GetType() != typeof(AttackState))
        {
            ChangeState(new AttackState(this));
        }
        else if (playerVisible && !playerInRange && ActualSatate.GetType() != typeof(FollowState))
        {
            ChangeState(new FollowState(this));
        }
        else if (!playerVisible && ActualSatate.GetType() != typeof(PatrolState))
        {
            ChangeState(new PatrolState(this));
        }
    }

    public void ChangeState(InterfaceEnemyStates nuevoEstado)
    {
        ActualSatate?.Exit();
        ActualSatate = nuevoEstado;
        ActualSatate.Enter();
      
        
    }

    public void PlayAudio(int index)
    {
        if (index < audioSources.Length && audioSources[index] != null)
            audioSources[index].Play();
    }

    public void Hurt()
    {
        PlayAudio(2);
        animator.SetTrigger("Hurt");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            lastTimeDamage = Time.time;
            hasMadeDamage = true;
        }
    }
}
