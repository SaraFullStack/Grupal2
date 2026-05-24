using System.Collections;
using System.Linq;
using UnityEngine;

public class PlayerCollectibles : MonoBehaviour
{

    [Header("Dialogos")]
    [SerializeField] private DialogType coreObtainedDialog = DialogType.EnergyCoreObtained;
    [SerializeField] private bool showCoreDialogOnlyOnce = true;
    [SerializeField] private float coreDialogDelay = 0.05f;

    private bool coreDialogShown;

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
    }

    void Start()
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
        currentCores = gameData.cores;
        currentScrews = gameData.screws;

        if (InputManager.healWasPressed)
        {
            if (remainingTime <= 0 && currentScrews >= screwsToHeal && !HUDController.IsFullHeal())
            {
                pressStartTime = Time.time - totalDuration;
                isKeyHeld = true;
            }

        }

        if (HUDController.IsFullHeal() || currentScrews < screwsToHeal)
        {
            isKeyHeld = false;
            totalDuration = 0;
            HUDController.UpdateHealingCounter(0);
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
                        currentScrews = gameData.screws;
                        totalDuration = 0;

                        if (currentScrews >= screwsToHeal && !HUDController.IsFullHeal())
                        {
                            pressStartTime = Time.time;
                            isKeyHeld = true;
                            HUDController.UpdateHealingCounter(0);
                        }
                        else
                        {
                            isKeyHeld = false;
                        }
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

    public void AddCollectible(int amount, CollectibleType type, int identifier)
    {
        switch (type)
        {
            case CollectibleType.EnergyCore:
                currentCores += amount;
                gameData.cores = currentCores;
                gameData.obteinedCores.Add(identifier);
if (DialogController.Instance != null)
                {
                    if (!showCoreDialogOnlyOnce || !coreDialogShown)
                    {
                        coreDialogShown = true;
                        StartCoroutine(LaunchCoreDialogDelayed());
                    }
                }

                break;
            case CollectibleType.Screw:
                currentScrews += amount;
                gameData.screws = currentScrews;
break;
        }
    }

    private IEnumerator LaunchCoreDialogDelayed()
    {
        yield return new WaitForSeconds(coreDialogDelay);

        if (DialogController.Instance != null)
        {
            DialogController.LaunchDialog(coreObtainedDialog);
        }
    }

    private void OnHealingUpdate()
    {
        currentScrews = gameData.screws;

        if (HUDController.IsFullHeal())
            return;

        if (currentScrews < screwsToHeal)
            return;

        currentScrews -= screwsToHeal;
        currentScrews = Mathf.Max(currentScrews, 0);
        gameData.screws = currentScrews;

    }
}
