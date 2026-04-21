using UnityEngine;
using UnityEngine.InputSystem;

public class HUDExample : MonoBehaviour
{
    public static int initialLife = 10;
    public static int actualLife;
    
    public static int screwCounter = 127;

    private float pressStartTime;
    private float totalDuration = 0f;
    private float timeToHealth = 1.5f;
    private bool isKeyHeld = false;
    
    float healingCooldown = 5f;
    float remainingTime;
    
    void Awake()
    {
        actualLife = initialLife;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            if (actualLife < initialLife)
            {
                actualLife += 1;
                HUDController.GainLife(actualLife);
            }
        }
        else if (Keyboard.current.wKey.wasPressedThisFrame)
        {
            if (actualLife > 0){
                actualLife -= 1;
                HUDController.LoseLife(actualLife);
            }
        }
        else if (Keyboard.current.hKey.wasPressedThisFrame)
        {
            if (remainingTime <= 0)
            {

                    pressStartTime = Time.time - totalDuration;

                // totalDuration = 0;

                isKeyHeld = true;
            }
        }
        
        // Esto se ejecuta CADA frame mientras la tecla esté bajada
        if (Keyboard.current.hKey.isPressed)
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

        // Detectar el momento en que se suelta
        if (Keyboard.current.hKey.wasReleasedThisFrame)
        {
            isKeyHeld = false;
        }
        
        
        // Mientras haya tiempo en el contador informamos
        if (totalDuration >= 0 && totalDuration <= timeToHealth)
        {
            if (remainingTime <= 0)
            {
                float percentageComplete = (totalDuration * 100) / timeToHealth;
                totalDuration -= Time.deltaTime;
                // si alcanza el maximo activamos el cooldown
                if ((int)percentageComplete >= 100 && actualLife < initialLife)
                {
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
}
