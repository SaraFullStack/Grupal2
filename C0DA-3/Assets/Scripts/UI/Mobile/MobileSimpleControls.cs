using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

[DefaultExecutionOrder(-50)]
public class MobileSimpleControls : MonoBehaviour
{
    [SerializeField] private bool showInEditor = true;

    private Canvas canvas;
    private CanvasGroup gameplayGroup;
    private CanvasGroup selectGroup;

    private bool lastGameplayVisible;
    private bool lastSelectVisible;

    private void Awake()
    {
#if UNITY_ANDROID || UNITY_EDITOR
        if (!showInEditor && Application.isEditor)
        {
            gameObject.SetActive(false);
            return;
        }

        EnsureEventSystem();
        CreateMobileUI();
        DontDestroyOnLoad(gameObject);
#else
        gameObject.SetActive(false);
#endif
    }

    private void Update()
    {
        if (gameplayGroup == null || selectGroup == null)
            return;

        bool menuShown = MenuController.IsShown();

        bool gameplayVisible = Time.timeScale > 0f && !InputManager.IsUIOpen;
        bool selectVisible = gameplayVisible || menuShown;

        SetGroupVisible(gameplayGroup, gameplayVisible);
        SetGroupVisible(selectGroup, selectVisible);

        if (lastGameplayVisible && !gameplayVisible)
            InputManager.ResetMobileGameplayInputOnly();

        if (!lastGameplayVisible && gameplayVisible)
            InputManager.ResetAllMobileInput();

        lastGameplayVisible = gameplayVisible;
        lastSelectVisible = selectVisible;
    }

    private void SetGroupVisible(CanvasGroup group, bool visible)
    {
        group.alpha = visible ? 1f : 0f;
        group.interactable = visible;
        group.blocksRaycasts = visible;
    }

    private void EnsureEventSystem()
    {
        if (FindFirstObjectByType<EventSystem>() != null)
            return;

        GameObject obj = new GameObject("EventSystem");
        obj.AddComponent<EventSystem>();
        obj.AddComponent<InputSystemUIInputModule>();
        DontDestroyOnLoad(obj);
    }

    private void CreateMobileUI()
    {
        canvas = new GameObject("Mobile Controls").AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 999;
        canvas.transform.SetParent(transform, false);

        CanvasScaler scaler = canvas.gameObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.matchWidthOrHeight = 0.5f;

        canvas.gameObject.AddComponent<GraphicRaycaster>();

        gameplayGroup = CreateGroup("Gameplay Buttons");
        selectGroup = CreateGroup("Select Button");

        CreateLookArea(gameplayGroup.transform);

        CreateDirectionButton("↑", new Vector2(210, 260), Vector2.up);
        CreateDirectionButton("↓", new Vector2(210, 80), Vector2.down);
        CreateDirectionButton("←", new Vector2(90, 170), Vector2.left);
        CreateDirectionButton("→", new Vector2(330, 170), Vector2.right);

        CreateHoldButton(
            gameplayGroup.transform,
            "A",
            new Vector2(1710, 150),
            InputManager.MobileJumpDown,
            InputManager.MobileJumpUp,
            new Vector2(125, 125)
        );

        CreateHoldButton(
            gameplayGroup.transform,
            "B",
            new Vector2(1545, 260),
            InputManager.MobileAttackDown,
            InputManager.MobileAttackUp,
            new Vector2(125, 125)
        );

        CreateHoldButton(
            gameplayGroup.transform,
            "START",
            new Vector2(820, 70),
            InputManager.MobileHealDown,
            InputManager.MobileHealUp,
            new Vector2(150, 58)
        );

        CreateTapButton(
            selectGroup.transform,
            "SELECT",
            new Vector2(1040, 70),
            InputManager.MobileMenu,
            new Vector2(160, 58)
        );
    }

    private CanvasGroup CreateGroup(string groupName)
    {
        GameObject obj = new GameObject(groupName);
        obj.transform.SetParent(canvas.transform, false);

        RectTransform rect = obj.AddComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        CanvasGroup group = obj.AddComponent<CanvasGroup>();
        group.alpha = 1f;
        group.interactable = true;
        group.blocksRaycasts = true;

        return group;
    }

    private void CreateDirectionButton(string label, Vector2 pos, Vector2 dir)
    {
        CreateHoldButton(
            gameplayGroup.transform,
            label,
            pos,
            () => InputManager.SetMobileMovement(dir),
            () => InputManager.SetMobileMovement(Vector2.zero),
            new Vector2(120, 120)
        );
    }

    private void CreateLookArea(Transform parent)
    {
        GameObject obj = new GameObject("Touch_Look_Area");
        obj.transform.SetParent(parent, false);

        RectTransform rect = obj.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.38f, 0.18f);
        rect.anchorMax = new Vector2(1f, 1f);
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        Image img = obj.AddComponent<Image>();
        img.color = new Color(0f, 0f, 0f, 0f);
        img.raycastTarget = true;

        obj.AddComponent<LookAreaEvents>();
    }

    private void CreateHoldButton(
        Transform parent,
        string text,
        Vector2 pos,
        System.Action down,
        System.Action up,
        Vector2 size)
    {
        GameObject obj = CreateVisualButton(parent, text, pos, size);

        MobileButtonEvents events = obj.AddComponent<MobileButtonEvents>();
        events.onDown = down;
        events.onUp = up;
    }

    private void CreateTapButton(
        Transform parent,
        string text,
        Vector2 pos,
        System.Action tap,
        Vector2 size)
    {
        GameObject obj = CreateVisualButton(parent, text, pos, size);

        MobileButtonEvents events = obj.AddComponent<MobileButtonEvents>();
        events.onDown = tap;
        events.onUp = null;
    }

    private GameObject CreateVisualButton(Transform parent, string text, Vector2 pos, Vector2 size)
    {
        GameObject obj = new GameObject("Btn_" + text);
        obj.transform.SetParent(parent, false);

        RectTransform rect = obj.AddComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.zero;
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = pos;
        rect.sizeDelta = size;

        Image img = obj.AddComponent<Image>();
        img.color = new Color(1f, 0.45f, 0.05f, 0.55f);
        img.raycastTarget = true;

        GameObject label = new GameObject("Text");
        label.transform.SetParent(obj.transform, false);

        RectTransform labelRect = label.AddComponent<RectTransform>();
        labelRect.anchorMin = Vector2.zero;
        labelRect.anchorMax = Vector2.one;
        labelRect.offsetMin = Vector2.zero;
        labelRect.offsetMax = Vector2.zero;

        Text t = label.AddComponent<Text>();
        t.text = text;
        t.alignment = TextAnchor.MiddleCenter;
        t.fontSize = text.Length > 1 ? 22 : 46;
        t.color = new Color(1f, 1f, 1f, 0.95f);
        t.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        t.raycastTarget = false;

        return obj;
    }
}

public class MobileButtonEvents : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    public System.Action onDown;
    public System.Action onUp;

    private bool pressed;

    public void OnPointerDown(PointerEventData eventData)
    {
        pressed = true;
        onDown?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Release();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Release();
    }

    private void OnDisable()
    {
        Release();
    }

    private void OnDestroy()
    {
        Release();
    }

    private void Release()
    {
        if (!pressed)
            return;

        pressed = false;
        onUp?.Invoke();
    }
}

public class LookAreaEvents : MonoBehaviour, IDragHandler
{
    [SerializeField] private float sensitivity = 0.18f;

    public void OnDrag(PointerEventData eventData)
    {
        if (InputManager.IsUIOpen)
            return;

        InputManager.AddMobileLook(eventData.delta * sensitivity);
    }
}