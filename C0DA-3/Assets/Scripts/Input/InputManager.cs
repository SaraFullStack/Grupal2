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
    
    public static bool healWasPressed;
    public static bool healIsHeld;
    public static bool healWasReleased;
    
    public static bool cancelWasPressed;
    public static bool submitWasPressed;
    public static bool clickWasPressed;
    public static bool menuWasPressed;

    private PlayerInput _playerInput;
    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _attackAction;
    private InputAction _healingAction;
    
    private InputAction _cancelAction;
    private InputAction _submitAction;
    private InputAction _clickAction;
    private InputAction _menuAction;

    public static string currentControlScheme = "";
    
    public static InputManager Instance; // Estático para acceso global

    private void Awake()
    {
        Instance = this;
        _playerInput = GetComponent<PlayerInput>();

        if (_playerInput == null)
        {
            Debug.LogError("Falta el componente PlayerInput en el objeto Player.");
            return;
        }

        _playerInput.SwitchCurrentActionMap("Player");
        
        _moveAction = _playerInput.actions["Move"];
        _jumpAction = _playerInput.actions["Jump"];
        _attackAction = _playerInput.actions["Attack"];
        _healingAction = _playerInput.actions["Heal"];
        
        _cancelAction = _playerInput.actions["Cancel"];
        _submitAction = _playerInput.actions["Submit"];
        _clickAction = _playerInput.actions["Click"];
        _menuAction = _playerInput.actions["Menu"];
    }

    private void Update()
    {
        if (_playerInput == null) return;

        currentControlScheme = _playerInput.currentControlScheme;
        
        movement = _moveAction.ReadValue<Vector2>();

        jumpWasPressed = _jumpAction.WasPressedThisFrame();
        jumpIsHeld = _jumpAction.IsPressed();
        jumpWasReleased = _jumpAction.WasReleasedThisFrame();

        attackWasPressed = _attackAction.WasPressedThisFrame();
        attackIsHeld = _attackAction.IsPressed();
        attackWasReleased = _attackAction.WasReleasedThisFrame();
        
        healWasPressed = _healingAction.WasPressedThisFrame();
        healIsHeld = _healingAction.IsPressed();
        healWasReleased = _healingAction.WasReleasedThisFrame();
        
        cancelWasPressed = _cancelAction.WasPressedThisFrame();
        submitWasPressed = _submitAction.WasPressedThisFrame();
        clickWasPressed = _clickAction.WasPressedThisFrame();
        menuWasPressed = _menuAction.WasPressedThisFrame();
    }
    
    public void OpenUI() {
        // Cambia al mapa de UI y desactiva el del personaje automáticamente
        _playerInput.SwitchCurrentActionMap("UI");
        Time.timeScale = 0;
    }

    public void CloseUI() {
        // Regresa al mapa del personaje
        _playerInput.SwitchCurrentActionMap("Player");
        Time.timeScale = 1;
    }
}