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

    [Header("Componentes")]
    public NavMeshAgent agent;
    public Animator animator;
    public EnemyHealth health;
    private PlayerDetected _PlayerDetected;
    private AudioSource[] audioSources;
    //private Score score;

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
        //score = FindFirstObjectByType<Score>();
    }

    void Start()
    {
        // Estado inicial
        ChangeState(new PatrolState(this));
    }

    void Update()
    {
       // Actualizamos la referencia del target cada frame
        target = _PlayerDetected.GetPlayerInSight(); 

        SelectNextState();
        
        // ¡Esto es vital! Ejecuta el movimiento del estado actual
        ActualSatate?.Update();
    }

    void SelectNextState()
    {
        // Si ya está muerto, no permitimos ninguna otra transición.
        if (ActualSatate.GetType() == typeof(DeathState))
            return;

        // 1. PRIORIDAD MÁXIMA: Si no tiene vida, morir.
        if (health.health <= 0)
        {
            ChangeState(new DeathState(this));
            return;
        }

        bool playerVisible = target != null;
        bool playerInRange = playerVisible &&
            Vector3.Distance(transform.position, target.position) <= attackRange;

        // 2. Jugador a rango de ataque -> atacar
        if (playerInRange && ActualSatate.GetType() != typeof(AttackState))
        {
            ChangeState(new AttackState(this));
        }
        // 3. Ve al jugador pero está lejos -> perseguir
        else if (playerVisible && !playerInRange && ActualSatate.GetType() != typeof(FollowState))
        {
            ChangeState(new FollowState(this));
        }
        // 4. No ve al jugador -> patrullar
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
        // Esto te dirá en la consola exactamente qué está haciendo
        // Debug.Log("Estado actual: " + nuevoEstado.GetType().Name);
      
        
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
