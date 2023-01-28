using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_ZombieIdle : Action
{
    private Animator animator;

    public override void OnAwake()
    {
        animator = GetComponent<Animator>();
    }

    public override void OnStart()
    {
        Debug.Log("ZombieIdle OnStart");
        animator.SetBool("IsIdling", true);
    }

    public override TaskStatus OnUpdate()
    {
        return TaskStatus.Running;
    }

    public override void OnEnd()
    {
        Debug.Log("ZombieIdle OnEnd");
        animator.SetBool("IsIdling", false);
    }

    public override void OnReset()
    {
        Debug.Log("ZombieIdle OnReset");
    }


}
