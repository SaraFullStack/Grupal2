using UnityEngine;

public class Bridge : MonoBehaviour
{
    [SerializeField] PushableObject[] boxes;

    private void FixedUpdate()
    {
        foreach (var box in boxes)
        {
            if (!box.isBlocked)
            {
                break;
            }
            GetComponent<PlayPauseAnimation>().PlayAnimation();
            GetComponent<Bridge>().enabled = false;
        }
    }
}
