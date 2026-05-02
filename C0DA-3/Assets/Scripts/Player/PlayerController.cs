using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MovementStats moveStats;
    [SerializeField] private Transform cameraTransform;

    private CharacterController controller;
    private Animator animator;

    private Vector3 velocity;
    private float verticalVelocity;

    private bool isGrounded;
    private float coyoteTimer;
    private float jumpBufferTimer;

    private int jumpsUsed;

    float turnSmoothVelocity;

    public float VerticalVelocity => verticalVelocity;
    public int JumpsUsed => jumpsUsed;
    public bool IsGroundedNow => isGrounded;
    public bool hasClaw = false;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;
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

        animator.SetBool("Walking", false);

        if (moveDir.magnitude >= 0.1f)
        {
            float camY = cameraTransform.eulerAngles.y;
            float targetAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg + camY;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, moveStats.turnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 move = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(move * moveStats.moveSpeed * Time.deltaTime);

            animator.SetBool("Walking", true);
        }
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

            animator.SetBool("Jumping", true);
        }

        if (InputManager.jumpWasReleased && verticalVelocity > 0f)
        {
            verticalVelocity /= moveStats.gravityOnReleaseMultiplier;
        }
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

        velocity.y = verticalVelocity;
        controller.Move(velocity * Time.deltaTime);
    }

    public void ForceJump(float force)
    {
        verticalVelocity = force;
    }
}