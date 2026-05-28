using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-100)]
[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    public static Vector2 movement;
    public static Vector2 lookDelta;

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

    public static string currentControlScheme = "";
    public static InputManager Instance;

    public static bool IsUIOpen { get; private set; }

    private const string PlayerMap = "Player";
    private const string UIMap = "UI";

    private PlayerInput playerInput;

    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction;
    private InputAction attackAction;
    private InputAction healAction;

    private InputAction playerCancelAction;
    private InputAction playerSubmitAction;
    private InputAction playerClickAction;
    private InputAction playerMenuAction;

    private InputAction uiCancelAction;
    private InputAction uiSubmitAction;
    private InputAction uiClickAction;
    private InputAction uiMenuAction;

    private static Vector2 mobileMovement;
    private static Vector2 mobileLookDelta;

    private static bool mobileJumpPressed;
    private static bool mobileJumpHeld;
    private static bool mobileJumpReleased;

    private static bool mobileAttackPressed;
    private static bool mobileAttackHeld;
    private static bool mobileAttackReleased;

    private static bool mobileHealPressed;
    private static bool mobileHealHeld;
    private static bool mobileHealReleased;

    private static bool mobileSubmitPressed;
    private static bool mobileCancelPressed;
    private static bool mobileMenuPressed;
    private static bool mobileClickPressed;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        playerInput = GetComponent<PlayerInput>();

        if (playerInput == null || playerInput.actions == null)
        {
            Debug.LogError("InputManager necesita un PlayerInput con InputSystem_Actions asignado.");
            enabled = false;
            return;
        }

        CacheActions();

        IsUIOpen = false;
        ResetAllMobileInput();

        if (playerInput.actions.FindActionMap(PlayerMap, false) != null)
            playerInput.SwitchCurrentActionMap(PlayerMap);
    }

    private void CacheActions()
    {
        moveAction = FindAction(PlayerMap, "Move");
        lookAction = FindAction(PlayerMap, "Look");

        jumpAction = FindAction(PlayerMap, "Jump");
        attackAction = FindAction(PlayerMap, "Attack");
        healAction = FindAction(PlayerMap, "Heal");

        playerCancelAction = FindAction(PlayerMap, "Cancel");
        playerSubmitAction = FindAction(PlayerMap, "Submit");
        playerClickAction = FindAction(PlayerMap, "Click");
        playerMenuAction = FindAction(PlayerMap, "Menu");

        uiCancelAction = FindAction(UIMap, "Cancel");
        uiSubmitAction = FindAction(UIMap, "Submit");
        uiClickAction = FindAction(UIMap, "Click");
        uiMenuAction = FindAction(UIMap, "Menu");
    }

    private InputAction FindAction(string mapName, string actionName)
    {
        InputActionMap map = playerInput.actions.FindActionMap(mapName, false);

        if (map != null)
        {
            InputAction action = map.FindAction(actionName, false);
            if (action != null)
                return action;
        }

        return playerInput.actions.FindAction(actionName, false);
    }

    private void Update()
    {
        ResetFrameValues();

        currentControlScheme = playerInput != null ? playerInput.currentControlScheme : "";

        if (!IsUIOpen)
        {
            movement = ReadVector2(moveAction);
            lookDelta = ReadVector2(lookAction);

            jumpWasPressed = WasPressed(jumpAction);
            jumpIsHeld = IsPressed(jumpAction);
            jumpWasReleased = WasReleased(jumpAction);

            attackWasPressed = WasPressed(attackAction);
            attackIsHeld = IsPressed(attackAction);
            attackWasReleased = WasReleased(attackAction);

            healWasPressed = WasPressed(healAction);
            healIsHeld = IsPressed(healAction);
            healWasReleased = WasReleased(healAction);
        }

        submitWasPressed = WasPressed(playerSubmitAction) || WasPressed(uiSubmitAction);
        cancelWasPressed = WasPressed(playerCancelAction) || WasPressed(uiCancelAction);
        clickWasPressed = WasPressed(playerClickAction) || WasPressed(uiClickAction);
        menuWasPressed = WasPressed(playerMenuAction) || WasPressed(uiMenuAction);

        ApplyMobileInput();

        if (IsUIOpen || Time.timeScale == 0f)
        {
            if (Touchscreen.current != null &&
                Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
            {
                clickWasPressed = true;
                submitWasPressed = true;
            }

#if UNITY_EDITOR
            if (Mouse.current != null &&
                Mouse.current.leftButton.wasPressedThisFrame)
            {
                clickWasPressed = true;
                submitWasPressed = true;
            }
#endif
        }

        ClearMobileFrameInput();
    }

    private static void ResetFrameValues()
    {
        movement = Vector2.zero;
        lookDelta = Vector2.zero;

        jumpWasPressed = false;
        jumpIsHeld = false;
        jumpWasReleased = false;

        attackWasPressed = false;
        attackIsHeld = false;
        attackWasReleased = false;

        healWasPressed = false;
        healIsHeld = false;
        healWasReleased = false;

        cancelWasPressed = false;
        submitWasPressed = false;
        clickWasPressed = false;
        menuWasPressed = false;
    }

    private static void ApplyMobileInput()
    {
        if (IsUIOpen)
        {
            submitWasPressed |= mobileSubmitPressed;
            cancelWasPressed |= mobileCancelPressed;
            clickWasPressed |= mobileClickPressed;
            menuWasPressed |= mobileMenuPressed;
            return;
        }

        if (mobileMovement != Vector2.zero)
            movement = mobileMovement;

        lookDelta += mobileLookDelta;

        jumpWasPressed |= mobileJumpPressed;
        jumpIsHeld |= mobileJumpHeld;
        jumpWasReleased |= mobileJumpReleased;

        attackWasPressed |= mobileAttackPressed;
        attackIsHeld |= mobileAttackHeld;
        attackWasReleased |= mobileAttackReleased;

        healWasPressed |= mobileHealPressed;
        healIsHeld |= mobileHealHeld;
        healWasReleased |= mobileHealReleased;

        submitWasPressed |= mobileSubmitPressed;
        cancelWasPressed |= mobileCancelPressed;
        clickWasPressed |= mobileClickPressed;
        menuWasPressed |= mobileMenuPressed;
    }

    private static void ClearMobileFrameInput()
    {
        mobileLookDelta = Vector2.zero;

        mobileJumpPressed = false;
        mobileJumpReleased = false;

        mobileAttackPressed = false;
        mobileAttackReleased = false;

        mobileHealPressed = false;
        mobileHealReleased = false;

        mobileSubmitPressed = false;
        mobileCancelPressed = false;
        mobileMenuPressed = false;
        mobileClickPressed = false;
    }

    public static void ResetMobileGameplayInputOnly()
    {
        mobileMovement = Vector2.zero;
        mobileLookDelta = Vector2.zero;

        mobileJumpPressed = false;
        mobileJumpHeld = false;
        mobileJumpReleased = false;

        mobileAttackPressed = false;
        mobileAttackHeld = false;
        mobileAttackReleased = false;

        mobileHealPressed = false;
        mobileHealHeld = false;
        mobileHealReleased = false;

        movement = Vector2.zero;
        lookDelta = Vector2.zero;

        jumpWasPressed = false;
        jumpIsHeld = false;
        jumpWasReleased = false;

        attackWasPressed = false;
        attackIsHeld = false;
        attackWasReleased = false;

        healWasPressed = false;
        healIsHeld = false;
        healWasReleased = false;
    }

    public static void ResetAllMobileInput()
    {
        ResetMobileGameplayInputOnly();

        mobileSubmitPressed = false;
        mobileCancelPressed = false;
        mobileMenuPressed = false;
        mobileClickPressed = false;

        submitWasPressed = false;
        cancelWasPressed = false;
        clickWasPressed = false;
        menuWasPressed = false;
    }

    public static void SetMobileMovement(Vector2 value)
    {
        if (IsUIOpen)
        {
            mobileMovement = Vector2.zero;
            return;
        }

        mobileMovement = Vector2.ClampMagnitude(value, 1f);
    }

    public static void AddMobileLook(Vector2 value)
    {
        if (IsUIOpen)
            return;

        mobileLookDelta += value;
    }

    public static void MobileJumpDown()
    {
        mobileJumpPressed = true;
        mobileJumpHeld = true;

        mobileSubmitPressed = true;
        mobileClickPressed = true;
    }

    public static void MobileJumpUp()
    {
        mobileJumpReleased = true;
        mobileJumpHeld = false;
    }

    public static void MobileAttackDown()
    {
        mobileAttackPressed = true;
        mobileAttackHeld = true;

        mobileSubmitPressed = true;
        mobileClickPressed = true;
    }

    public static void MobileAttackUp()
    {
        mobileAttackReleased = true;
        mobileAttackHeld = false;
    }

    public static void MobileHealDown()
    {
        mobileHealPressed = true;
        mobileHealHeld = true;
    }

    public static void MobileHealUp()
    {
        mobileHealReleased = true;
        mobileHealHeld = false;
    }

    public static void MobileMenu()
    {
        mobileMenuPressed = true;
    }

    public static void MobileSubmit()
    {
        mobileSubmitPressed = true;
        mobileClickPressed = true;
    }

    public static void MobileCancel()
    {
        mobileCancelPressed = true;
    }

    public static void MobileClick()
    {
        mobileClickPressed = true;
        mobileSubmitPressed = true;
    }

    private static Vector2 ReadVector2(InputAction action)
    {
        if (action == null || !action.enabled)
            return Vector2.zero;

        return action.ReadValue<Vector2>();
    }

    private static bool WasPressed(InputAction action)
    {
        return action != null && action.enabled && action.WasPressedThisFrame();
    }

    private static bool IsPressed(InputAction action)
    {
        return action != null && action.enabled && action.IsPressed();
    }

    private static bool WasReleased(InputAction action)
    {
        return action != null && action.enabled && action.WasReleasedThisFrame();
    }

    public void OpenUI()
    {
        IsUIOpen = true;
        ResetAllMobileInput();

        // NO cambiar a UIMap. Mantener Player activo evita que móvil se quede sin gameplay.
        InputActionMap playerMap = playerInput.actions.FindActionMap(PlayerMap, false);
        if (playerMap != null && !playerMap.enabled)
            playerMap.Enable();

        InputActionMap uiMap = playerInput.actions.FindActionMap(UIMap, false);
        if (uiMap != null && !uiMap.enabled)
            uiMap.Enable();

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseUI()
    {
        Time.timeScale = 1f;

        IsUIOpen = false;
        ResetAllMobileInput();

        // NO depender de SwitchCurrentActionMap.
        InputActionMap playerMap = playerInput.actions.FindActionMap(PlayerMap, false);
        if (playerMap != null && !playerMap.enabled)
            playerMap.Enable();

        InputActionMap uiMap = playerInput.actions.FindActionMap(UIMap, false);
        if (uiMap != null && !uiMap.enabled)
            uiMap.Enable();

        if (EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(null);

#if UNITY_ANDROID && !UNITY_EDITOR
    Cursor.lockState = CursorLockMode.None;
    Cursor.visible = false;
#elif UNITY_EDITOR
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
#else
    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;
#endif
    }
}