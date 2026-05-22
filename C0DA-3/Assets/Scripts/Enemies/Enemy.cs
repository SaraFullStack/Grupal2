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
        if (health.health <= 0 && ActualSatate.GetType() != typeof(DeathState))
        {
            ChangeState(new DeathState(this));
        }
        else if (hasMadeDamage && ActualSatate.GetType() != typeof(WaitingState))
        {
            ChangeState(new WaitingState(this));
        }
        else if (target != null && ActualSatate.GetType() != typeof(FollowState))
        {
            ChangeState(new FollowState(this));
        }
        else if (target == null && !hasMadeDamage && ActualSatate.GetType() != typeof(PatrolState))
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
