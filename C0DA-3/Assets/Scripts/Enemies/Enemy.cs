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
        //1. PRIORIDAD MÁXIMA: Si no tiene vida, morir.
        if (health.health <= 0 && ActualSatate.GetType() != typeof(DeathState))
        {
            ChangeState(new DeathState(this));
        }
        // 2. Si acaba de atacar, esperar un poco (WaitingState)
        else if (hasMadeDamage && ActualSatate.GetType() != typeof(WaitingState))
        {
            ChangeState(new WaitingState(this));
        }
         // 3. Si VE al jugador, perseguir (FollowState)
        else if (target != null && ActualSatate.GetType() != typeof(FollowState))
        {
            ChangeState(new FollowState(this));
        }
        // 4. SI NO PASA NADA DE LO ANTERIOR, volver a patrullar
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
