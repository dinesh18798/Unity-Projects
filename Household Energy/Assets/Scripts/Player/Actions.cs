using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Actions : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Stay()
    {
        animator.SetFloat("Speed", 0f);
    }

    public void RunBackward()
    {
        animator.SetFloat("Speed", -1.0f);
    }

    public void WalkBackward(float value)
    {
        animator.SetFloat("Speed", value);
    }

    public void Walk(float value)
    {
        animator.SetFloat("Speed", value);
    }

    public void Run()
    {
        animator.SetFloat("Speed", 1.0f);
    }
}
