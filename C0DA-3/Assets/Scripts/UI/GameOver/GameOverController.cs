using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{

    [SerializeField] private string sceneToReset;
    [SerializeField] private string sceneToOut;

    private const string gear = "Gear";
    private const string btnReset = "Reset";
    private const string btnClose = "Close";
    private const string content = "GameOver";

    private VisualElement _gear;
    private Button _resetBtn;
    private Button _closeBtn;
    private VisualElement _content;

    private float currentAngleLeft = 0f; 
    public float rotationSpeed = 90f;

    private bool isGameOver = false;
    private static GameOverController _instance;
    public static GameOverController Instance { get { return _instance; } }

    public static void LaunchGameOver()
    {
        if (_instance == null)
        {
            return;
        }

        _instance.ShowGameOver();

    }

     private void ShowGameOver()
    {

        if (isGameOver)
        {
            return;
        }

        isGameOver = true;

        UIDocument uiDocument = GetComponent<UIDocument>();

        if (uiDocument == null)
        {
            return;
        }

        var root = uiDocument.rootVisualElement;

        if (root == null)
        {
            return;
        }

        root.style.display = DisplayStyle.Flex;

        Time.timeScale = 0f;

        if (InputManager.Instance != null)
        {
            InputManager.Instance.OpenUI();
        }

        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;

        if (_resetBtn != null)
        {
            _resetBtn.Focus();
        }
    }

    private void HideGameOver()
    {
        isGameOver = false;

        UIDocument uiDocument = GetComponent<UIDocument>();

        if (uiDocument == null)
        {
            return;
        }

        var root = uiDocument.rootVisualElement;

        if (root == null)
        {
            return;
        }

        root.style.display = DisplayStyle.None;

        Time.timeScale = 1f;

        if (InputManager.Instance != null)
        {
            InputManager.Instance.CloseUI();
        }
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;

    }


    void Awake()
    {
         if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            _instance = this;
        }


        UIDocument uiDocument = GetComponent<UIDocument>();

        if (uiDocument == null)
        {
            return;
        }

        var root = uiDocument.rootVisualElement;

        if (root == null)
        {
            return;
        }

        _content = root.Q<VisualElement>(content);
        _gear = root.Q<VisualElement>(gear);
        _resetBtn = root.Q<Button>(btnReset);
        _closeBtn = root.Q<Button>(btnClose);

        if (_content != null)
            _content.style.display = DisplayStyle.Flex;

        root.style.display = DisplayStyle.None; 

        if (_resetBtn != null)
        {
            var btnTextReset = new LocalizedString("Main", "btn_reset");
            _resetBtn.SetBinding("text", btnTextReset);

            _resetBtn.clicked += () => {

                HideGameOver();

                SceneManager.LoadScene(sceneToReset);
            };
        }

        if (_closeBtn != null)
        {
            var btnTextClose = new LocalizedString("Main", "btn_restart");
            _closeBtn.SetBinding("text", btnTextClose);

            _closeBtn.clicked += () => {

                HideGameOver();

                SceneManager.LoadScene(sceneToOut);
            };
        }

    }
    void Update()
    {
        if (_gear == null)
        {
            return;
        }

        float realDeltaTime = Time.unscaledDeltaTime;

        currentAngleLeft += rotationSpeed * realDeltaTime;
        currentAngleLeft %= 360f; 
        _gear.style.rotate = new Rotate(new Angle(currentAngleLeft));
    }
}
