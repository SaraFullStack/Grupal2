using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using System.Collections.Generic;


public class MenuController : MonoBehaviour
{
    private const string menu = "Menu";
    private const string tabViews = "TabContent";

    private const string loadBtn = "ButtonLoad";
    private const string settingsBtn = "ButtonSettings";
    private const string backBtn = "ButtonBack";
    private const string mainBtn = "ButtonMain";
    private const string labelLoad = "LabelLoad";
    private const string labelSettings = "LabelSettings";
    private const string englishBtn = "ButtonEnglish";
    private const string spanishBtn = "ButtonSpanish";
    private const string labelSelectLanguage = "LabelSelectLanguage";
    private const string checkEnglish = "checkEnglish";
    private const string checkSpanish = "checkSpanish";

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

    
    private VisualElement _menu;
    private TabView _tabViews;
    private Button _loadBtn;
    private Button _settingsBtn;
    private Button _backBtn;
    private Button _mainBtn;
    private Label _labelLoad;
    private Label _labelSettings;
    private Button _englishBtn;
    private Button _spanishBtn;
    private Label _labelSelectLanguage;
    private VisualElement _checkEnglish;
    private VisualElement _checkSpanish;

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


    private static bool isMenuShown;

    // Singleton
    private static MenuController _instance;
    public static MenuController Instance { get { return _instance; } }

    public static void LaunchMenu()
    {
        _instance.ShowMenu();

    }

    public static void CloseMenu()
    {
        _instance.HideMenu();
    }

    private void ShowMenu()
    {
        if (isMenuShown)
        {
            return;
        }


        var root = GetComponent<UIDocument>().rootVisualElement;
        root.style.display = DisplayStyle.Flex;
        isMenuShown = true;
        
        InputManager.Instance.OpenUI();
    }

    private void HideMenu()
    {
        if (!isMenuShown)
        {
            return;
        }

        var root = GetComponent<UIDocument>().rootVisualElement;
        root.style.display = DisplayStyle.None;
        isMenuShown = false;

        InputManager.Instance.CloseUI();
    }

    void Awake()
    {
         if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }

        var root = GetComponent<UIDocument>().rootVisualElement;
        _menu = root.Q<VisualElement>(menu);
        _tabViews = root.Q<TabView>(tabViews);
        _loadBtn = root.Q<Button>(loadBtn);
        _settingsBtn = root.Q<Button>(settingsBtn);
        _backBtn = root.Q<Button>(backBtn);
        _mainBtn = root.Q<Button>(mainBtn);
        _labelLoad = root.Q<Label>(labelLoad);
        _labelSettings = root.Q<Label>(labelSettings);
        _englishBtn = root.Q<Button>(englishBtn);
        _spanishBtn = root.Q<Button>(spanishBtn);
        _labelSelectLanguage = root.Q<Label>(labelSelectLanguage);
        _checkEnglish = root.Q<VisualElement>(checkEnglish);
        _checkSpanish = root.Q<VisualElement>(checkSpanish);

        _checkEnglish.style.display = DisplayStyle.None;
        _checkSpanish.style.display = DisplayStyle.None;

        _menu.style.display = DisplayStyle.Flex;
        root.style.display = DisplayStyle.None; // Hide menu

        // Localization

        var btnTextLoad = new LocalizedString("Main", "btn_load");
        _loadBtn.SetBinding("text", btnTextLoad);

        var btnTextSettings = new LocalizedString("Main", "btn_settings");
        _settingsBtn.SetBinding("text", btnTextSettings);

        var btnTextBack = new LocalizedString("Main", "btn_back");
        _backBtn.SetBinding("text", btnTextBack);
        _mainBtn.SetBinding("text", btnTextBack);

        var textLoad = new LocalizedString("Main", "title_load");
        _labelLoad.SetBinding("text", textLoad);

        var textSettings = new LocalizedString("Main", "title_settings");
        _labelSettings.SetBinding("text", textSettings);

        var textSelectLanguage = new LocalizedString("Main", "select_language");
        _labelSelectLanguage.SetBinding("text", textSelectLanguage);

        var btnEnglish = new LocalizedString("Main", "btn_english");
        _englishBtn.SetBinding("text", btnEnglish);

        var btnSpanish = new LocalizedString("Main", "btn_spanish");
        _spanishBtn.SetBinding("text", btnSpanish);

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

        _englishBtn.clicked += () => {
            Debug.Log("Pulsa Inglés");
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
            _checkEnglish.style.display = DisplayStyle.Flex;
            _checkSpanish.style.display = DisplayStyle.None;
        };

        _spanishBtn.clicked += () => {
            Debug.Log("Pulsa Español");
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[1];
            _checkEnglish.style.display = DisplayStyle.None;
            _checkSpanish.style.display = DisplayStyle.Flex;
        };

        
    }

    void OnEnable()
    {
        var locales = LocalizationSettings.AvailableLocales.Locales;
        int currentLocaleIndex = locales.IndexOf(LocalizationSettings.SelectedLocale);
        
        if (currentLocaleIndex == 0)
        {
            _checkEnglish.style.display = DisplayStyle.Flex;
            _checkSpanish.style.display = DisplayStyle.None;
        }
        else
        {
            _checkEnglish.style.display = DisplayStyle.None;
            _checkSpanish.style.display = DisplayStyle.Flex;
        }

        _mainBtn.RegisterCallback<ClickEvent>(OnCloseButtonClicked, TrickleDown.TrickleDown);
    }

    void OnDisable()
    {
        _mainBtn.UnregisterCallback<ClickEvent>(OnCloseButtonClicked, TrickleDown.TrickleDown);
    }

    private void OnCloseButtonClicked(ClickEvent e)
    {
        HideMenu();
    }


    void ChangeLanguage(int index)
    {
        // Cambia el idioma globalmente
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
    }

    void Update()
    {
         float realDeltaTime = Time.unscaledDeltaTime;

        currentAngleLeft += rotationSpeed * realDeltaTime;
        currentAngleRight -= rotationSpeed * realDeltaTime;

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

        _checkEnglish.style.rotate = new Rotate(new Angle(currentAngleLeft));
        _checkSpanish.style.rotate = new Rotate(new Angle(currentAngleLeft));
    }

    void Start()
    {
       //InputManager.Instance.OpenUI();
    }


}
