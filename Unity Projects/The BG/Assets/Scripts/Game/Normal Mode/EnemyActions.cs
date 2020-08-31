﻿using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyActions : MonoBehaviour
{

    private Animator animator;

    const int countOfDamageAnimations = 3;
    int lastDamageAnimation = -1;

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

    public void WalkBackward()
    {
        animator.SetFloat("Speed", -0.5f);
    }

    public void Walk(float speed)
    {
        animator.SetFloat("Speed", speed);
    }

    public void Run()
    {
        animator.SetFloat("Speed", 1.0f);
    }

    public void Attack()
    {
        animator.SetTrigger("Fire");
    }

    public void Death()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
            animator.Play("Idle", 0);
        else
            animator.SetTrigger("Death");
    }

    public void Damage()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Death")) return;
        int id = Random.Range(0, countOfDamageAnimations);
        if (countOfDamageAnimations > 1)
            while (id == lastDamageAnimation)
                id = Random.Range(0, countOfDamageAnimations);
        lastDamageAnimation = id;
        animator.SetInteger("DamageID", id);
        animator.SetTrigger("Damage");
    }

    public void Jump()
    {
        animator.SetBool("Squat", false);
        animator.SetFloat("Speed", 0f);
        animator.SetBool("Aiming", false);
        animator.SetTrigger("Jump");
    }

    public void Aiming()
    {
        animator.SetBool("Aiming", true);
    }

    public void Sitting()
    {
        animator.SetBool("Squat", !animator.GetBool("Squat"));
        animator.SetBool("Aiming", false);
    }
}
