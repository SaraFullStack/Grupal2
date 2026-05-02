using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class StartController : MonoBehaviour
{

    private const string tabViews = "TabContent";

    private const string startBtn = "ButtonStart";
    private const string loadBtn = "ButtonLoad";
    private const string settingsBtn = "ButtonSettings";
    private const string backBtn = "ButtonBack";
    private const string mainBtn = "ButtonMain";

    private const string gear1 = "Gear1";
    private const string gear2 = "Gear2";
    private const string gear3 = "Gear3";
    private const string gear4 = "Gear4";
    private const string gear5 = "Gear5";
    private const string gear6 = "Gear6";
    private const string gear7 = "Gear7";
    private const string gear8 = "Gear8";
    private const string gear9 = "Gear9";
    private const string gear10 = "Gear10";
    private const string gear11 = "Gear11";

    

    private TabView _tabViews;
    private Button _startBtn;
    private Button _loadBtn;
    private Button _settingsBtn;
    private Button _backBtn;
    private Button _mainBtn;

    private VisualElement _gear1;
    private VisualElement _gear2;
    private VisualElement _gear3;
    private VisualElement _gear4;
    private VisualElement _gear5;
    private VisualElement _gear6;
    private VisualElement _gear7;
    private VisualElement _gear8;
    private VisualElement _gear9;
    private VisualElement _gear10;
    private VisualElement _gear11;

    private float currentAngleLeft = 0f; 
    private float currentAngleRight = 0f;    
    public float rotationSpeed = 90f;

    void Awake()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        _tabViews = root.Q<TabView>(tabViews);
        _startBtn = root.Q<Button>(startBtn);
        _loadBtn = root.Q<Button>(loadBtn);
        _settingsBtn = root.Q<Button>(settingsBtn);
        _backBtn = root.Q<Button>(backBtn);
        _mainBtn = root.Q<Button>(mainBtn);

        _gear1 = root.Q<VisualElement>(gear1);
        _gear2 = root.Q<VisualElement>(gear2);
        _gear3 = root.Q<VisualElement>(gear3);
        _gear4 = root.Q<VisualElement>(gear4);
        _gear5 = root.Q<VisualElement>(gear5);
        _gear6 = root.Q<VisualElement>(gear6);
        _gear7 = root.Q<VisualElement>(gear7);
        _gear8 = root.Q<VisualElement>(gear8);
        _gear9 = root.Q<VisualElement>(gear9);
        _gear10 = root.Q<VisualElement>(gear10);
        _gear11 = root.Q<VisualElement>(gear11);

        // Optimización crucial para elementos que se mueven o rotan
        _gear1.usageHints = UsageHints.DynamicTransform;
        _gear2.usageHints = UsageHints.DynamicTransform;
        _gear3.usageHints = UsageHints.DynamicTransform;
        _gear4.usageHints = UsageHints.DynamicTransform;
        _gear5.usageHints = UsageHints.DynamicTransform;
        _gear6.usageHints = UsageHints.DynamicTransform;
        _gear7.usageHints = UsageHints.DynamicTransform;
        _gear8.usageHints = UsageHints.DynamicTransform;
        _gear9.usageHints = UsageHints.DynamicTransform;
        _gear10.usageHints = UsageHints.DynamicTransform;
        _gear11.usageHints = UsageHints.DynamicTransform;
        

       // _startBtn.Focus();

        _startBtn.clicked += () => {
            // Esto se ejecutará si haces clic O si pulsas 'A' teniendo el foco
            Debug.Log("Pulsa INICIAR");
        };

        _loadBtn.clicked += () => {
            Debug.Log("Pulsa CARGAR");
            // Quitamos el foco actual
            //root.panel.focusController.focusedElement?.Blur();
            _tabViews.selectedTabIndex = 1;
        };

        _settingsBtn.clicked += () => {
            Debug.Log("Pulsa SETTINGS");
            _tabViews.selectedTabIndex = 2;
        };

        _backBtn.clicked += () => {
            Debug.Log("Pulsa Atrás");
            _tabViews.selectedTabIndex = 0;
        };

        _mainBtn.clicked += () => {
            Debug.Log("Pulsa Atrás");
            _tabViews.selectedTabIndex = 0;
        };

        
    }

    void Update()
    {
        currentAngleLeft += rotationSpeed * Time.deltaTime;
        currentAngleRight -= rotationSpeed * Time.deltaTime;

        // Mantener el ángulo entre 0 y 360 para evitar imprecisiones de coma flotante
        currentAngleLeft %= 360f; 
        currentAngleRight %= 360f;

        _gear1.style.rotate = new Rotate(new Angle(currentAngleLeft));
        _gear3.style.rotate = new Rotate(new Angle(currentAngleLeft));
        _gear5.style.rotate = new Rotate(new Angle(currentAngleLeft));
        _gear6.style.rotate = new Rotate(new Angle(currentAngleLeft));
        _gear8.style.rotate = new Rotate(new Angle(currentAngleLeft));
        _gear10.style.rotate = new Rotate(new Angle(currentAngleLeft));

        _gear2.style.rotate = new Rotate(new Angle(currentAngleRight));
        _gear4.style.rotate = new Rotate(new Angle(currentAngleRight));
        _gear7.style.rotate = new Rotate(new Angle(currentAngleRight));
        _gear9.style.rotate = new Rotate(new Angle(currentAngleRight));
        _gear11.style.rotate = new Rotate(new Angle(currentAngleRight));
    }

    void Start()
    {
       //InputManager.Instance.OpenUI();
    }


}
