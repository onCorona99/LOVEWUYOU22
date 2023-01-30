using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_ZombieAttack : Action
{
    private Animator animator;

    public override void OnAwake()
    {
        animator = GetComponent<Animator>();

    }

    public override void OnStart()
    {
        Debug.Log("<color=#ffccff>[BT]OnStart  ZombieAttack</color>");
        animator.SetBool("IsAttacking", true);
    }

    public override TaskStatus OnUpdate()
    {
        return TaskStatus.Running;
    }

    public override void OnEnd()
    {
        Debug.Log("<color=#ffccff>[BT]OnEnd  ZombieAttack</color>");
        animator.SetBool("IsAttacking", false);
    }

    public override void OnReset()
    {
        Debug.Log("ZombieAttack OnReset");
    }


}
