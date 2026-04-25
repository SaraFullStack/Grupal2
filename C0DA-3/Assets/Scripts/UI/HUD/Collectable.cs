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
        // TODO: get saved values
        currentScrews = 120;
        currentCores = 20;
    }

    private void Start()
    {
        HUDController.UpdateScrews(currentScrews);
        HUDController.SetScrewsToHeal(screwsToHeal);
        HUDController.UpdateCores(currentCores);
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
            // Mientras haya tiempo en el contador informamos
            if (totalDuration >= 0 && totalDuration <= timeToHealth)
            {
                if (remainingTime <= 0)
                {
                    float percentageComplete = (totalDuration * 100) / timeToHealth;
                    totalDuration -= Time.deltaTime;
                    // si alcanza el maximo activamos el cooldown

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

        // controlamos el cooldown
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
        HUDController.UpdateScrews(currentScrews);
    }
    
    #region public methods

    public void AddScrews(int screws = 1)
    {
        currentScrews += screws;
        HUDController.UpdateScrews(currentScrews);
    }

    public void AddCore(int cores = 1)
    {
        currentCores += cores;
        HUDController.UpdateCores(currentCores);
    }
    
    public void RemoveScrews(int screws = 1)
    {
        if (currentScrews >= screws)
        {
            currentScrews -= screws;
            HUDController.UpdateScrews(currentScrews);
        }
    }

    public void RemoveCores(int cores = 1)
    {
        if (currentCores >= cores)
        {
            currentCores -= cores;
            HUDController.UpdateCores(currentCores);
        }
    }
    
    #endregion
}
