using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MovementStats moveStats;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    private CharacterController controller;

    private Vector3 velocity;
    private float verticalVelocity;

    private bool isGrounded;
    private float coyoteTimer;
    private float jumpBufferTimer;

    private int jumpsUsed;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    private void Update()
    {
        HandleTimers();
        CheckGround();
        HandleMovement();
        HandleJump();
        ApplyGravity();
    }

    private void HandleTimers()
    {
        jumpBufferTimer -= Time.deltaTime;
        coyoteTimer -= Time.deltaTime;

        if (InputManager.jumpWasPressed)
            jumpBufferTimer = moveStats.jumpBufferTime;
    }

    private void CheckGround()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, groundLayer);

        if (isGrounded && verticalVelocity <= 0f)
        {
            coyoteTimer = moveStats.jumpCoyoteTime;
            jumpsUsed = 0;
            verticalVelocity = -2f;
        }
    }

    private void HandleMovement()
    {
        Vector2 input = InputManager.movement;
        Vector3 moveDir = new Vector3(input.x, 0f, input.y).normalized;

        if (moveDir.magnitude >= 0.1f)
        {
            float camY = cameraTransform.eulerAngles.y;

            float targetAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg + camY;

            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

            Vector3 move = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(move * moveStats.moveSpeed * Time.deltaTime);
        }
    }

    private void HandleJump()
    {
        bool canGroundJump = coyoteTimer > 0f;
        bool canAirJump = jumpsUsed < moveStats.numberOfJumpsAllowed;

        if (jumpBufferTimer > 0f && (canGroundJump || canAirJump))
        {
            verticalVelocity = moveStats.InitialJumpVelocity;
            jumpBufferTimer = 0f;
            coyoteTimer = 0f;
            jumpsUsed++;
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
}