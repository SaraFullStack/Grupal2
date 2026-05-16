using System;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.Linq; 

public class HUDController : MonoBehaviour
{

    public GameDataSO gameData;


    private const string gear = "Gear";
    private const string life = "LifeUnit";
    private const string healthBar = "ScrewHealingProgress";
    private const string screwContainer = "HUD_Screw_Content";
    private const string screwCounter = "ScrewText";
    private const string barBackground = "LifeBarBackground";
    private const string heart = "Heart";
    private const string coreCounter = "CoreText";

    private VisualElement _gear;
    private List<VisualElement> _lifeUnits;
    private Label _screwText;
    private ProgressBar _healthBar;
    private Label _screwCounter;
    private VisualElement _barBackground;
    private VisualElement _heart;
    private VisualElement _screwContainer;
    private Label _coreCounter;
    
    private const string backgroundDamage = "lifebar--damage";
    private const string heartDamage = "heart--damage";
    
    private float actualAngle = 0f;
    private const float degreesPerTime = 180f;

    private int lastScrewValue = 0;
    private bool isModifyingScrewValue = false;
    
    private AudioSource healSound;
    private AudioSource damageSound;
    private AudioSource chargingSound;
    private AudioSource energyCoreSound;
    private AudioSource screwSound;
    
    private int actualLife = 0;
    private int initialLife = 10;
    
    private int totalScrews = 0;
    private int screwsToHeal = 10;
    private int totalCores = 0;
    
    // Singleton
    public static HUDController Instance { get; private set; }
    
    // Event
    public event Action<int> OnHealing; 
    public event Action OnScrewsHealing; 
    
    void Awake()
    {
        // Validación del Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // No se destruye al cambiar de nivel
        }


        _lifeUnits = new List<VisualElement>();
        
        var root = GetComponent<UIDocument>().rootVisualElement;
        _gear = root.Q<VisualElement>(gear);
        _healthBar = root.Q<ProgressBar>(healthBar);
        _screwCounter = root.Q<Label>(screwCounter);
        _barBackground = root.Q<VisualElement>(barBackground);
        _heart = root.Q<VisualElement>(heart);
        _screwContainer = root.Q<VisualElement>(screwContainer);
        _coreCounter = root.Q<Label>(coreCounter);
        
        foreach (int i in Enumerable.Range(1, 10))
        {
            string elementName = life + i;
            _lifeUnits.Add(root.Q<VisualElement>(elementName));
        }
        
        root.style.display = DisplayStyle.Flex; // Show HUD

    }

    void Start()
    {
        AudioSource[] sources = GetComponents<AudioSource>();
        healSound = sources[0];
        damageSound = sources[1];
        chargingSound = sources[2];
        energyCoreSound = sources[3];
        screwSound = sources[4];
    }
    
    private void OnEnable()
    {
        _barBackground.RegisterCallback<TransitionEndEvent>(OnTintFinished);
    }
    
    private void OnDisable()
    {
        _barBackground.UnregisterCallback<TransitionEndEvent>(OnTintFinished);
    }

    private void OnTintFinished(TransitionEndEvent evt)
    {
        VisualElement element = evt.target as VisualElement;
        if (element.name == barBackground && _barBackground.ClassListContains(backgroundDamage))
        {
            _barBackground.RemoveFromClassList(backgroundDamage);
            _heart.RemoveFromClassList(heartDamage);
        }
    }
    
    #region Static Methods

    public static void SetLife(int totalLife)
    {
        if (Instance != null){
            Instance.initialLife = totalLife;
            Instance.actualLife = totalLife;
            Instance.UpdateLife(totalLife);
        }
    }

    public static void SetScrews(int totalScrews)
    {
       // _instance.totalScrews = totalScrews;
       // _instance._screwCounter.text = totalScrews.ToString();
    }
    
    public static void UpdateScrews(int newScrews)
    {
        /*
        if (_instance.totalScrews < newScrews)
        {
            _instance.screwSound.Play();
        }
        
        _instance.totalScrews = newScrews;
        _instance._screwCounter.text = newScrews.ToString();
        */
    }
    
    public static void SetScrewsToHeal(int screwsToHeal)
    {
        Instance.screwsToHeal = screwsToHeal;
    }

    public static void SetCores(int totalCores)
    {
        /*
        _instance.totalCores = totalCores;
        _instance._coreCounter.text = totalCores.ToString();
        */
    }
    
    public static void UpdateCores(int newCores)
    {
        /*
        if (_instance.totalCores < newCores)
        {
            _instance.energyCoreSound.Play();
        }
        _instance.totalCores = newCores;
        _instance._coreCounter.text = newCores.ToString();
        */
    }
    
    public static void GainLife(int newLife)
    {
        Instance.UpdateLife(newLife);
        Instance.actualAngle = newLife;
        Instance.AddLife();
        
    }
    
    public static void LoseLife(int newLife)
    {
        Instance.UpdateLife(newLife);
        Instance.actualLife = newLife;
        Instance.SubstractLife();
    }
    
    public static void UpdateHealingCounter(int newValue)
    {
        Instance.UpdateHealing(newValue);
    }

    public static void Cooldown(bool isCooldown)
    {
        Instance._screwContainer.style.opacity = isCooldown ? 0.5f : 1;
    }

    public static bool IsFullHeal()
    {
        return Instance.actualLife >= Instance.initialLife;
    }
    #endregion

    private void AddLife()
    {
        healSound.Play();
        actualAngle += degreesPerTime;
        _gear.style.rotate = new StyleRotate(new Rotate(Angle.Degrees(actualAngle)));
    }
    
    private void SubstractLife()
    {
        damageSound.Play();
        actualAngle -= degreesPerTime;
        _gear.style.rotate = new StyleRotate(new Rotate(Angle.Degrees(actualAngle)));
        
        _barBackground.AddToClassList(backgroundDamage);
        _heart.AddToClassList(heartDamage);
    }

    private void UpdateLife(int newLife)
    {
        foreach (int i in Enumerable.Range(1, 10))
        {
            if (i <= newLife)
            {
                _lifeUnits[i-1].style.visibility = Visibility.Visible;
            }
            else
            {
                _lifeUnits[i-1].style.visibility = Visibility.Hidden;
            }
            
        }
        actualLife = newLife;
    }
    
    private void UpdateHealing(int newValue)
    {
        if (!chargingSound.isPlaying && newValue > 0)
        {
            chargingSound.Play();
        }
        else if (chargingSound.isPlaying && newValue == 0)
        {
            chargingSound.Stop();
        }
        
        _healthBar.value = newValue;

        isModifyingScrewValue = (newValue != 0);

        int actualCounter = totalScrews;
        float screwsToRemove = ((float)newValue / 100.0f) * (float)screwsToHeal;
        
        int provisionalValue = actualCounter - (int)screwsToRemove;
        _screwCounter.text = provisionalValue.ToString();

        if (newValue == 100 && actualLife < initialLife)
        {
            actualLife += 1;
            chargingSound.Stop();
            UpdateLife(actualLife);
            AddLife();
            _healthBar.value = 0;
            OnHealing?.Invoke(actualLife);
            OnScrewsHealing?.Invoke();
        }
    }

    private void Update()
    {
        totalCores = gameData.cores;
        _coreCounter.text = gameData.cores.ToString();

        totalScrews = gameData.screws;
        _screwCounter.text = gameData.screws.ToString();

        int actualCounter = totalScrews;
        if (lastScrewValue != actualCounter && !isModifyingScrewValue)
        {
            _screwCounter.text = actualCounter.ToString();
            lastScrewValue =  actualCounter;
        }

        if (InputManager.menuWasPressed)
        {
            if (MenuController.IsShown())
                MenuController.CloseMenu();
            else
                MenuController.LaunchMenu();
        }
    }
}
