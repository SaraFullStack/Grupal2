using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MovementStats moveStats;
    [SerializeField] private Transform cameraTransform;

    [Header("Particles Prefabs")]
    [SerializeField] private ParticleSystem walkParticles;
    [SerializeField] private ParticleSystem jumpParticles;
    [SerializeField] private float walkParticleInterval = 0.25f;

    private CharacterController controller;
    private Animator animator;

    private Vector3 velocity;
    private float verticalVelocity;

    private bool isGrounded;
    private float coyoteTimer;
    private float jumpBufferTimer;

    private int jumpsUsed;
    private float turnSmoothVelocity;
    private float nextWalkParticleTime;

    public float VerticalVelocity => verticalVelocity;
    public int JumpsUsed => jumpsUsed;
    public bool IsGroundedNow => isGrounded;
    public bool hasClaw = false;

    public GameObject claw1;
    public GameObject claw2;
    public GameDataSO gameData;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;

        if (gameData != null)
        {
            hasClaw = gameData.hasClaw;
        }

        claw1.SetActive(hasClaw);
        claw2.SetActive(hasClaw);
    }

    private void Update()
    {
        CheckGround();
        HandleTimers();
        HandleMovement();
        HandleJump();
        ApplyGravity();
    }

    private void CheckGround()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && verticalVelocity <= 0f)
        {
            coyoteTimer = moveStats.jumpCoyoteTime;
            jumpsUsed = 0;
            verticalVelocity = -2f;

            if (animator != null)
                animator.SetBool("Jumping", false);
        }
        else
        {
            coyoteTimer -= Time.deltaTime;
        }
    }

    private void HandleTimers()
    {
        jumpBufferTimer -= Time.deltaTime;

        if (InputManager.jumpWasPressed)
            jumpBufferTimer = moveStats.jumpBufferTime;
    }

    private void HandleMovement()
    {
        Vector2 input = InputManager.movement;
        Vector3 moveDir = new Vector3(input.x, 0f, input.y).normalized;

        bool isMoving = moveDir.magnitude >= 0.1f;

        if (animator != null)
            animator.SetBool("Walking", false);

        if (isMoving)
        {
            float camY = cameraTransform.eulerAngles.y;
            float targetAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg + camY;

            float angle = Mathf.SmoothDampAngle(
                transform.eulerAngles.y,
                targetAngle,
                ref turnSmoothVelocity,
                moveStats.turnSmoothTime
            );

            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 move = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(move * moveStats.moveSpeed * Time.deltaTime);

            if (animator != null)
                animator.SetBool("Walking", true);
        }

        if (isGrounded && isMoving)
            PlayWalkParticles();
    }

    private void PlayWalkParticles()
    {
        if (walkParticles == null)
        {
return;
        }

        if (Time.time < nextWalkParticleTime)
            return;

        nextWalkParticleTime = Time.time + walkParticleInterval;

        Vector3 spawnPosition = GetFeetPosition();

        ParticleSystem particles = Instantiate(
            walkParticles,
            spawnPosition,
            Quaternion.identity
        );

        particles.Play();

        float destroyTime = particles.main.duration + particles.main.startLifetime.constantMax;
        Destroy(particles.gameObject, destroyTime);

    }

    private void HandleJump()
    {
        bool canGroundJump = coyoteTimer > 0f && jumpsUsed == 0;
        bool canAirJump = jumpsUsed > 0 && jumpsUsed < moveStats.numberOfJumpsAllowed;

        if (jumpBufferTimer > 0f && (canGroundJump || canAirJump))
        {
            verticalVelocity = moveStats.InitialJumpVelocity;
            jumpBufferTimer = 0f;
            coyoteTimer = 0f;
            jumpsUsed++;

            if (animator != null)
                animator.SetBool("Jumping", true);

            PlayJumpParticles();
        }

        if (InputManager.jumpWasReleased && verticalVelocity > 0f)
        {
            verticalVelocity /= moveStats.gravityOnReleaseMultiplier;
        }
    }

    private void PlayJumpParticles()
    {
        if (jumpParticles == null)
        {
return;
        }

        Vector3 spawnPosition = GetFeetPosition();

        ParticleSystem particles = Instantiate(
            jumpParticles,
            spawnPosition,
            Quaternion.identity
        );

        particles.Play();

        float destroyTime = particles.main.duration + particles.main.startLifetime.constantMax;
        Destroy(particles.gameObject, destroyTime);
    }

    private Vector3 GetFeetPosition()
    {
        Vector3 position = transform.position;
        position.y -= controller.height * 0.5f;
        return position;
    }

    private void ApplyGravity()
    {
        if (!isGrounded || verticalVelocity > 0f)
        {
            if (verticalVelocity < 0f)
                verticalVelocity += moveStats.Gravity * moveStats.gravityOnReleaseMultiplier * Time.deltaTime;
            else
                verticalVelocity += moveStats.Gravity * Time.deltaTime;
        }

        verticalVelocity = Mathf.Max(verticalVelocity, -moveStats.maxFallSpeed);

        if (isGrounded && verticalVelocity < 0)
            verticalVelocity = -2f;

        velocity.y = verticalVelocity;
        controller.Move(velocity * Time.deltaTime);
    }

    public void ForceJump(float force)
    {
        verticalVelocity = force;
    }
}