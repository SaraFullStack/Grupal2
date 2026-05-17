using System.Linq;
using UnityEngine;

public class PlayerCollectibles : MonoBehaviour
{
    public GameDataSO gameData;

    [Header("Collectibles")]
    [SerializeField] private int currentCores = 0;
    [SerializeField] private int currentScrews = 100;
    
    [SerializeField] public float healingCooldown = 5f;
    [SerializeField] public float timeToHealth = 1.5f;
    [SerializeField] public int screwsToHeal = 10;
    
    private float remainingTime;
    private float pressStartTime;
    private float totalDuration = 0f;
    private bool isKeyHeld = false;

    public int CurrentCores => currentCores;
    public int CurrentScrews => currentScrews;

    
    private void Awake()
    {
        currentCores = gameData.cores;
        currentScrews = gameData.screws;
        // TODO: get saved values
        // currentScrews = 120;
        // currentCores = 20;
    }
    
    void Start()
    {
        //HUDController.SetCores(currentCores);
        //HUDController.SetScrews(currentScrews);

        HUDController.SetScrewsToHeal(screwsToHeal);
        HUDController.Instance.OnScrewsHealing += OnHealingUpdate;
    }
    
    private void OnDisable()
    {
        HUDController.Instance.OnScrewsHealing -= OnHealingUpdate;
    }
    
    private void Update()
    {
        currentCores = gameData.cores;
        currentScrews = gameData.screws;

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
    
    public void AddCollectible(int amount, CollectibleType type, int identifier)
    {
        switch (type)
        {
            case CollectibleType.EnergyCore:
                currentCores += amount;
                gameData.cores = currentCores;
                gameData.obteinedCores.Add(identifier);

                Debug.Log("Núcleos actuales: " + currentCores);
                break;
            case CollectibleType.Screw:

            Debug.Log("CURRENT "+ currentScrews);
            Debug.Log("GUARDADO "+gameData.screws);
                currentScrews += amount;
                gameData.screws = currentScrews;
                Debug.Log("Tornillos actuales: " + currentScrews);
                break;
        }
        
    }
    
    private void OnHealingUpdate()
    {
        currentScrews -= screwsToHeal;
        gameData.screws = currentScrews;

        HUDController.UpdateScrews(currentScrews);
    }
}

