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


    // Singleton
    private static GameOverController _instance;
    public static GameOverController Instance { get { return _instance; } }

    public static void LaunchGameOver()
    {
        if (_instance != null)
            _instance.ShowGameOver();

    }

     private void ShowGameOver()
    {

        var root = GetComponent<UIDocument>().rootVisualElement;
        root.style.display = DisplayStyle.Flex;

        InputManager.Instance.OpenUI();

        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;

        _resetBtn.Focus();
    }

    private void HideGameOver()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        root.style.display = DisplayStyle.None;

        InputManager.Instance.CloseUI();
        
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


        var root = GetComponent<UIDocument>().rootVisualElement;
        _content = root.Q<VisualElement>(content);
        _gear = root.Q<VisualElement>(gear);
        _resetBtn = root.Q<Button>(btnReset);
        _closeBtn = root.Q<Button>(btnClose);

        _content.style.display = DisplayStyle.Flex;
        root.style.display = DisplayStyle.None; // Hide menu

        var btnTextReset = new LocalizedString("Main", "btn_reset");
        _resetBtn.SetBinding("text", btnTextReset);

        var btnTextClose = new LocalizedString("Main", "btn_restart");
        _closeBtn.SetBinding("text", btnTextClose);


        _resetBtn.clicked += () => {
            HideGameOver();
            SceneManager.LoadScene(sceneToReset);
        };

        _closeBtn.clicked += () => {
            HideGameOver();
            SceneManager.LoadScene(sceneToOut);
        };
    }

    // Update is called once per frame
    void Update()
    {
        float realDeltaTime = Time.unscaledDeltaTime;

        currentAngleLeft += rotationSpeed * realDeltaTime;
        currentAngleLeft %= 360f; 
        _gear.style.rotate = new Rotate(new Angle(currentAngleLeft));
    }
}
