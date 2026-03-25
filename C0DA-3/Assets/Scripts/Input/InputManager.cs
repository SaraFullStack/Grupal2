using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static Vector2 movement;

    public static bool jumpWasPressed;
    public static bool jumpIsHeld;
    public static bool jumpWasReleased;

    public static bool attackWasPressed;
    public static bool attackIsHeld;
    public static bool attackWasReleased;

    private PlayerInput _playerInput;
    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _attackAction;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();

        if (_playerInput == null)
        {
            Debug.LogError("Falta el componente PlayerInput en el objeto Player.");
            return;
        }

        _moveAction = _playerInput.actions["Move"];
        _jumpAction = _playerInput.actions["Jump"];
        _attackAction = _playerInput.actions["Attack"];
    }

    private void Update()
    {
        if (_playerInput == null) return;

        movement = _moveAction.ReadValue<Vector2>();

        jumpWasPressed = _jumpAction.WasPressedThisFrame();
        jumpIsHeld = _jumpAction.IsPressed();
        jumpWasReleased = _jumpAction.WasReleasedThisFrame();

        attackWasPressed = _attackAction.WasPressedThisFrame();
        attackIsHeld = _attackAction.IsPressed();
        attackWasReleased = _attackAction.WasReleasedThisFrame();
    }
}