using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] public float healingCooldown = 5f;
    [SerializeField] public float timeToHealth = 1.5f;
    
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
        currentScrews = 186;
        currentCores = 0;
    }

    private void Start()
    {
        HUDController.UpdateScrews(currentScrews);
        HUDController.Instance.OnScrewsHealing += OnHealingUpdate;
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        HUDController.Instance.OnScrewsHealing -= OnHealingUpdate;
    }

    private void Update()
    {
        if (InputManager.healWasPressed)
        {
            if (remainingTime <= 0)
            {
                pressStartTime = Time.time - totalDuration;
                isKeyHeld = true;
            }
            
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

        if (InputManager.healWasReleased)
        {
            isKeyHeld = false;
        }
        
        
        
        // Mientras haya tiempo en el contador informamos
        if (totalDuration >= 0 && totalDuration <= timeToHealth)
        {
            Debug.Log("---->CARGANDO");
            if (remainingTime <= 0)
            {
                float percentageComplete = (totalDuration * 100) / timeToHealth;
                totalDuration -= Time.deltaTime;
                // si alcanza el maximo activamos el cooldown
                
                //if ((int)percentageComplete >= 100 && actualLife < initialLife)
                if ((int)percentageComplete >= 100)
                {
                    Debug.Log("---->COOOLDOWN");
                    remainingTime = healingCooldown;
                    totalDuration = 0;
                }
                
                HUDController.UpdateHealingCounter((int)percentageComplete);

                
            }
        }
        
        // controlamos el cooldown
        if (remainingTime > 0)
        {
            isKeyHeld = false;
            remainingTime -= Time.deltaTime;
        }
    }
    private void OnHealingUpdate()
    {
        currentScrews -= 10;
        HUDController.UpdateScrews(currentScrews);
    }
}
