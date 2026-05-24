using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] public float healingCooldown = 5f;
    [SerializeField] public float timeToHealth = 1.5f;
    [SerializeField] public int screwsToHeal = 10;
    
    private float remainingTime;
    private float pressStartTime;
    private float totalDuration = 0f;
    private bool isKeyHeld = false;
    
    private int currentScrews;
    private int currentCores;
    
    public int CurrentScrews => currentScrews;
    public int CurrentCores => currentCores;
    
    private void Awake()
    {
        currentScrews = 120;
        currentCores = 20;
    }

    private void Start()
    {
        HUDController.SetScrewsToHeal(screwsToHeal);
        HUDController.Instance.OnScrewsHealing += OnHealingUpdate;
    }

    private void OnDisable()
    {
        HUDController.Instance.OnScrewsHealing -= OnHealingUpdate;
    }

    private void Update()
    {
        if (InputManager.healWasPressed)
        {
            if (remainingTime <= 0 && currentScrews >= screwsToHeal)
            {
                pressStartTime = Time.time - totalDuration;
                isKeyHeld = true;
            }
            
        }

        if (HUDController.IsFullHeal())
        {
            isKeyHeld = false;
        }
        
        if (InputManager.healIsHeld)
        {
            if (isKeyHeld)
            {
                totalDuration = Time.time - pressStartTime;
                if (totalDuration > timeToHealth)
                {
                    totalDuration = timeToHealth;
                }
            }
        }

        if (!InputManager.healWasReleased)
        {
            if (totalDuration >= 0 && totalDuration <= timeToHealth)
            {
                if (remainingTime <= 0)
                {
                    float percentageComplete = (totalDuration * 100) / timeToHealth;
                    totalDuration -= Time.deltaTime;

                    if ((int)percentageComplete >= 100)
                    {
                        remainingTime = healingCooldown;
                        totalDuration = 0;

                        HUDController.Cooldown(true);
                    }

                    HUDController.UpdateHealingCounter((int)percentageComplete);

                }
            }
        }
        if (remainingTime > 0)
        {
            isKeyHeld = false;
            remainingTime -= Time.deltaTime;

            if (remainingTime <= 0)
            {
                HUDController.Cooldown(false);
            }
        }
    }
    private void OnHealingUpdate()
    {
        currentScrews -= screwsToHeal;
    }
    
    #region public methods

    public void AddScrews(int screws = 1)
    {
        currentScrews += screws;
    }

    public void AddCore(int cores = 1)
    {
        currentCores += cores;
    }
    
    public void RemoveScrews(int screws = 1)
    {
        if (currentScrews >= screws)
        {
            currentScrews -= screws;
        }
    }

    public void RemoveCores(int cores = 1)
    {
        if (currentCores >= cores)
        {
            currentCores -= cores;
        }
    }
    
    #endregion
}
