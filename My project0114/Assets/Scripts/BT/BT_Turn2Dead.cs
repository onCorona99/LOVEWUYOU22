using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_Turn2Dead : Action
{
    private Animator animator;

    public override void OnAwake()
    {
        animator = GetComponent<Animator>();
    }

    public override void OnStart()
    {
        animator.SetBool("IsDead", true);
    }

    public override TaskStatus OnUpdate()
    {
        return TaskStatus.Running;
    }
}
