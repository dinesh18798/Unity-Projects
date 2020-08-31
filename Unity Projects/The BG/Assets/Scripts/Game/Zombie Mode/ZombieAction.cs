using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAction : MonoBehaviour
{
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Walk(float speed)
    {
        animator.SetFloat("MoveSpeed", speed);
    }

    public void Stay()
    {
        animator.SetFloat("MoveSpeed", 0);
    }

    public void Death()
    {
        animator.SetTrigger("Dead");
    }

    public void Attack()
    {
        animator.SetTrigger("Attack");
    }
}

