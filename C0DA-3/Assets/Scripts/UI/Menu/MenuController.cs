using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using System.IO;
using System;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{

    [SerializeField] public GameDataSO gameData;
    [SerializeField] private string sceneToReset;

    private const string menu = "Menu";
    private const string tabViews = "TabContent";

    private const string loadBtn = "ButtonLoad";
    private const string settingsBtn = "ButtonSettings";
    private const string resetBtn = "ButtonReset";
    private const string backBtn = "ButtonBack";
    private const string backBtn2 = "ButtonBack2";
    private const string mainBtn = "ButtonMain";
    private const string labelLoad = "LabelLoad";
    private const string labelSettings = "LabelSettings";
    private const string englishBtn = "ButtonEnglish";
    private const string spanishBtn = "ButtonSpanish";
    private const string labelSelectLanguage = "LabelSelectLanguage";
    private const string checkEnglish = "checkEnglish";
    private const string checkSpanish = "checkSpanish";
    private const string screwText = "screwText";
    private const string coretText = "coreText";
    private const string slot1 = "slot1";
    private const string slot2 = "slot2";
    private const string slot3 = "slot3";
    private const string slot1Empty = "slot1Empty";
    private const string slot2Empty = "slot2Empty";
    private const string slot3Empty = "slot3Empty";
    private const string slot1Full = "slot1Full";
    private const string slot2Full = "slot2Full";
    private const string slot3Full = "slot3Full";
    private const string labelSlot1 = "slot1LabelEmpty";
    private const string labelSlot2 = "slot2LabelEmpty";
    private const string labelSlot3 = "slot3LabelEmpty";
    private const string slot1Screws = "slot1Screws";
    private const string slot1Cores = "slot1Cores";
    private const string slot1Date = "slot1Date";
    private const string slot2Screws = "slot2Screws";
    private const string slot2Cores = "slot2Cores";
    private const string slot2Date = "slot2Date";
    private const string slot3Screws = "slot3Screws";
    private const string slot3Cores = "slot3Cores";
    private const string slot3Date = "slot3Date";


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
    private Button _resetBtn;
    private Button _backBtn;
    private Button _backBtn2;
    private Button _mainBtn;
    private Label _labelLoad;
    private Label _labelSettings;
    private Button _englishBtn;
    private Button _spanishBtn;
    private Label _labelSelectLanguage;
    private VisualElement _checkEnglish;
    private VisualElement _checkSpanish;
    private Label _screwText;
    private Label _coreText;
    private VisualElement _slot1;
    private VisualElement _slot2;
    private VisualElement _slot3;
    private VisualElement _slot1_Empty;
    private VisualElement _slot2_Empty;
    private VisualElement _slot3_Empty;
    private VisualElement _slot1_Full;
    private VisualElement _slot2_Full;
    private VisualElement _slot3_Full;
    private VisualElement[] _slots;
    private VisualElement[] _slots_Empty;
    private VisualElement[] _slots_Full;
    private Label _slot1LabelEmpty;
    private Label _slot2LabelEmpty;
    private Label _slot3LabelEmpty;
    private Label _slot1LabelScrews;
    private Label _slot1LabelCores;
    private Label _slot1LabelDate;
    private Label _slot2LabelScrews;
    private Label _slot2LabelCores;
    private Label _slot2LabelDate;
    private Label _slot3LabelScrews;
    private Label _slot3LabelCores;
    private Label _slot3LabelDate;
    private Label[] _slotsScrews;
    private Label[] _slotsCores;
    private Label[] _slotsDate;


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
        if (_instance != null)
            _instance.ShowMenu();

    }

    public static void CloseMenu()
    {
        if (_instance != null)
            _instance.HideMenu();
    }

    public static bool IsShown()
    {
        return isMenuShown;
    }

    private void ShowMenu()
    {
        if (isMenuShown)
        {
            return;
        }

        var root = GetComponent<UIDocument>().rootVisualElement;
        root.style.display = DisplayStyle.Flex;

        InputManager.Instance.OpenUI();

        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;

        _tabViews.selectedTabIndex = 0;
        _loadBtn.Focus();

        isMenuShown = true;
    }

    private void HideMenu()
    {
        if (!isMenuShown)
        {
            return;
        }

        var root = GetComponent<UIDocument>().rootVisualElement;
        root.style.display = DisplayStyle.None;

        InputManager.Instance.CloseUI();

        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;

        isMenuShown = false;
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
        _menu = root.Q<VisualElement>(menu);
        _tabViews = root.Q<TabView>(tabViews);
        _loadBtn = root.Q<Button>(loadBtn);
        _settingsBtn = root.Q<Button>(settingsBtn);
        _resetBtn = root.Q<Button>(resetBtn);
        _backBtn = root.Q<Button>(backBtn);
        _backBtn2 = root.Q<Button>(backBtn2);
        _mainBtn = root.Q<Button>(mainBtn);
        _labelLoad = root.Q<Label>(labelLoad);
        _labelSettings = root.Q<Label>(labelSettings);
        _englishBtn = root.Q<Button>(englishBtn);
        _spanishBtn = root.Q<Button>(spanishBtn);
        _labelSelectLanguage = root.Q<Label>(labelSelectLanguage);
        _checkEnglish = root.Q<VisualElement>(checkEnglish);
        _checkSpanish = root.Q<VisualElement>(checkSpanish);
        _screwText = root.Q<Label>(screwText);
        _coreText = root.Q<Label>(coretText);
        _slot1 = root.Q<VisualElement>(slot1);
        _slot2 = root.Q<VisualElement>(slot2);
        _slot3 = root.Q<VisualElement>(slot3);
        _slot1_Empty = root.Q<VisualElement>(slot1Empty);
        _slot2_Empty = root.Q<VisualElement>(slot2Empty);
        _slot3_Empty = root.Q<VisualElement>(slot3Empty);
        _slot1_Full = root.Q<VisualElement>(slot1Full);
        _slot2_Full = root.Q<VisualElement>(slot2Full);
        _slot3_Full = root.Q<VisualElement>(slot3Full);

        VisualElement[] auxSlots = { _slot1, _slot2, _slot3 };
        _slots = auxSlots;

        VisualElement[] auxSlotsEmpty = { _slot1_Empty, _slot2_Empty, _slot3_Empty };
        _slots_Empty = auxSlotsEmpty;

        VisualElement[] auxSlotsFull = { _slot1_Full, _slot2_Full, _slot3_Full };
        _slots_Full = auxSlotsFull;

        _slot1LabelEmpty = root.Q<Label>(labelSlot1);
        _slot2LabelEmpty = root.Q<Label>(labelSlot2);
        _slot3LabelEmpty = root.Q<Label>(labelSlot3);
        _slot1LabelScrews = root.Q<Label>(slot1Screws);
        _slot1LabelCores = root.Q<Label>(slot1Cores);
        _slot1LabelDate = root.Q<Label>(slot1Date);
        _slot2LabelScrews = root.Q<Label>(slot2Screws);
        _slot2LabelCores = root.Q<Label>(slot2Cores);
        _slot2LabelDate = root.Q<Label>(slot2Date);
        _slot3LabelScrews = root.Q<Label>(slot3Screws);
        _slot3LabelCores = root.Q<Label>(slot3Cores);
        _slot3LabelDate = root.Q<Label>(slot3Date);

        Label[] auxSlotsScrews = { _slot1LabelScrews, _slot2LabelScrews, _slot3LabelScrews };
        _slotsScrews = auxSlotsScrews;

        Label[] auxSlotsCores = { _slot1LabelCores, _slot2LabelCores, _slot3LabelCores };
        _slotsCores = auxSlotsCores;

        Label[] auxSlotsDate = { _slot1LabelDate, _slot2LabelDate, _slot3LabelDate };
        _slotsDate = auxSlotsDate;

        _checkEnglish.style.display = DisplayStyle.None;
        _checkSpanish.style.display = DisplayStyle.None;

        _menu.style.display = DisplayStyle.Flex;
        root.style.display = DisplayStyle.None; // Hide menu

        // Localization
        var labelSlotEmpty = new LocalizedString("Main", "label_empty");
        _slot1LabelEmpty.SetBinding("text", labelSlotEmpty);
        _slot2LabelEmpty.SetBinding("text", labelSlotEmpty);
        _slot3LabelEmpty.SetBinding("text", labelSlotEmpty);

        var btnTextLoad = new LocalizedString("Main", "title_save");
        _loadBtn.SetBinding("text", btnTextLoad);

        var btnTextSettings = new LocalizedString("Main", "btn_settings");
        _settingsBtn.SetBinding("text", btnTextSettings);

        var btnTextReset = new LocalizedString("Main", "btn_reset");
        _resetBtn.SetBinding("text", btnTextReset);

        var btnTextBack = new LocalizedString("Main", "btn_back");
        _backBtn.SetBinding("text", btnTextBack);

        var btnTextClose = new LocalizedString("Main", "btn_close");
        _mainBtn.SetBinding("text", btnTextClose);

        var textLoad = new LocalizedString("Main", "title_save");
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


        _loadBtn.clicked += () =>
        {
            // Quitamos el foco actual
            //root.panel.focusController.focusedElement?.Blur();
            _tabViews.selectedTabIndex = 1;
        };

        _resetBtn.clicked += () =>
        {
            InputManager.Instance.CloseUI();
        
            SceneManager.LoadScene(sceneToReset);
        };

        _settingsBtn.clicked += () =>
        {
            _tabViews.selectedTabIndex = 2;
        };

        _backBtn.clicked += () =>
        {
            _tabViews.selectedTabIndex = 0;
        };

        _backBtn2.clicked += () =>
        {
            _tabViews.selectedTabIndex = 0;
        };

        _mainBtn.clicked += () =>
        {
            HideMenu();
        };

        _englishBtn.clicked += () =>
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
            _checkEnglish.style.display = DisplayStyle.Flex;
            _checkSpanish.style.display = DisplayStyle.None;
        };

        _spanishBtn.clicked += () =>
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[1];
            _checkEnglish.style.display = DisplayStyle.None;
            _checkSpanish.style.display = DisplayStyle.Flex;
        };


        _slot1.MakeInteractiveButton(() =>
        {
            SaveSlot(0);
        });

        _slot2.MakeInteractiveButton(() =>
        {
            SaveSlot(1);
        });

        _slot3.MakeInteractiveButton(() =>
        {
            SaveSlot(2);
        });


        RefreshSlots();

    }


    void RefreshSlots()
    {
        GetSlot(0);
        GetSlot(1);
        GetSlot(2);
    }

    void GetSlot(int index)
    {
        string savePath = Application.persistentDataPath + "/savegame" + index + ".json";
        VisualElement actualSlot = _slots[index];
        VisualElement emptySlot = _slots_Empty[index];
        VisualElement fullSlot = _slots_Full[index];

        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);

            GameDataSO auxGameData = ScriptableObject.CreateInstance<GameDataSO>();

            JsonUtility.FromJsonOverwrite(json, auxGameData);

            Label actualScrews = _slotsScrews[index];
            Label actualCores = _slotsCores[index];
            Label actualDate = _slotsDate[index];

            actualScrews.text = auxGameData.screws.ToString();
            actualCores.text = auxGameData.cores.ToString();
            actualDate.text = auxGameData.date;
        }
        else
        {
            emptySlot.style.display = DisplayStyle.Flex;
            fullSlot.style.display = DisplayStyle.None;
        }
    }


    void SaveSlot(int index)
    {

        string actualDate = DateTime.Now.ToString(@"dd\/MM\/yyyy h\:mm tt");
        gameData.date = actualDate;

        string savePath = Application.persistentDataPath + "/savegame" + index + ".json";
        string json = JsonUtility.ToJson(gameData);
        File.WriteAllText(savePath, json);
        Debug.Log("Partida Guardada en: " + savePath);

        RefreshSlots();
    }


    void OnEnable()
    {
        if (_mainBtn == null || _checkEnglish == null || _checkSpanish == null)
            return;

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
        if (_mainBtn == null)
            return;

        _mainBtn.UnregisterCallback<ClickEvent>(OnCloseButtonClicked, TrickleDown.TrickleDown);
    }

    private void OnCloseButtonClicked(ClickEvent e)
    {
        Debug.Log("Pulsa Atrás idioma");
        _tabViews.selectedTabIndex = 0;
        //  _loadBtn.Focus();
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


        _screwText.text = gameData.screws.ToString();
        _coreText.text = gameData.cores.ToString();


        if (InputManager.menuWasPressed)
        {
            Debug.Log("intenta cerrar");
            /*
            if (isMenuShown)
            {
                HideMenu();
            }
            */
        }

    }

    void Start()
    {
        //InputManager.Instance.OpenUI();
    }


}
