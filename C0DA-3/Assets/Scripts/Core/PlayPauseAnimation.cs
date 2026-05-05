using UnityEngine;

public class PlayPauseAnimation : MonoBehaviour
{
    Animator animator;
    [SerializeField] bool pauseOnStart;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (pauseOnStart)
            PauseAnimation();
    }

    public void PlayAnimation()
    {
        animator.speed = 1f;
    }

    public void PauseAnimation()
    {
        animator.speed = 0f;
    }
}
