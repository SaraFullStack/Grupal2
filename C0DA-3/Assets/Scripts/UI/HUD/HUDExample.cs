using UnityEngine;
using UnityEngine.InputSystem;

public class HUDExample : MonoBehaviour
{
    private int initialLife = 10;

    private int actualLife;

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
    }
}
