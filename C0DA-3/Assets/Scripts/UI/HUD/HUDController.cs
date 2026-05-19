using System;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.Linq; 

public class HUDController : MonoBehaviour
{
    private const string gear = "Gear";
    private const string life = "LifeUnit";
    private const string healthBar = "ScrewHealingProgress";
    private const string screwCounter = "ScrewText";

    private VisualElement _gear;
    private List<VisualElement> _lifeUnits;
    private Label _screwText;
    private ProgressBar _healthBar;
    private Label _screwCounter;
    
    private float actualAngle = 0f;
    private const float degreesPerTime = 180f;

    private int lastScrewValue = 0;
    private bool isModifyingScrewValue = false;
    
    private AudioSource healSound;
    private AudioSource damageSound;
    private AudioSource chargingSound;
    
    // Singleton
    private static HUDController _instance;
    public static HUDController Instance { get { return _instance; } }
    
    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }

        _lifeUnits = new List<VisualElement>();
        
        var root = GetComponent<UIDocument>().rootVisualElement;
        _gear = root.Q<VisualElement>(gear);
        _healthBar = root.Q<ProgressBar>(healthBar);
        _screwCounter = root.Q<Label>(screwCounter);
        
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
    }
<<<<<<< Updated upstream
=======
    
    private void OnEnable()
    {
        if (_barBackground != null)
        _barBackground.RegisterCallback<TransitionEndEvent>(OnTintFinished);
    }
    
    private void OnDisable()
    {
        if (_barBackground != null){
            _barBackground.UnregisterCallback<TransitionEndEvent>(OnTintFinished);
        }
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
    
>>>>>>> Stashed changes
    #region Static Methods
    public static void GainLife(int newLife)
    {
        _instance.UpdateLife(newLife);
        _instance.AddLife();
    }
    
    public static void LoseLife(int newLife)
    {
        _instance.UpdateLife(newLife);
        _instance.SubstractLife();
    }
    
    public static void UpdateHealingCounter(int newValue)
    {
<<<<<<< Updated upstream
        _instance.UpdateHealing(newValue);
=======
        Instance.UpdateHealing(newValue);
    }

    public static void Cooldown(bool isCooldown)
    {
        Instance._screwContainer.style.opacity = isCooldown ? 0.5f : 1;
    }

    public static bool IsFullHeal()
    {
        if (Instance == null) return false;
        return Instance.actualLife >= Instance.initialLife;
>>>>>>> Stashed changes
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

        // TODO: Cambiar HUDExmple por el que contentga la info
        int actualCounter = HUDExample.screwCounter;
        int provisionalValue = actualCounter - (newValue/10);
        _screwCounter.text = provisionalValue.ToString();

        if (newValue == 100 && HUDExample.actualLife < HUDExample.initialLife)
        {
            HUDExample.screwCounter = provisionalValue;
            HUDExample.actualLife += 1;
            chargingSound.Stop();
            UpdateLife(HUDExample.actualLife);
            AddLife();
            _healthBar.value = 0;
        }
    }

    private void Update()
    {
<<<<<<< Updated upstream
        int actualCounter = HUDExample.screwCounter;
=======
        if (gameData == null) return;
        totalCores = gameData.cores;
        _coreCounter.text = gameData.cores.ToString();

        totalScrews = gameData.screws;
        _screwCounter.text = gameData.screws.ToString();

        int actualCounter = totalScrews;
>>>>>>> Stashed changes
        if (lastScrewValue != actualCounter && !isModifyingScrewValue)
        {
            _screwCounter.text = actualCounter.ToString();
            lastScrewValue =  actualCounter;
        }
        
    }
}
