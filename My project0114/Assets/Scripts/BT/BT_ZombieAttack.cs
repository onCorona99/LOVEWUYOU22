using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_ZombieAttack : Action
{
    private Animator animator;

    private float AtkAnimClipLength;

    private float startTime;

    public override void OnAwake()
    {
        animator = GetComponent<Animator>();

        AtkAnimClipLength = GetComponent<ZombieController>().AtkAnimLenght;
    }

    public override void OnStart()
    {
        Debug.Log("ZombieAttack OnStart");
        //transform.LookAt(PlayerController.instance.transform);
        animator.SetBool("IsAttacking", true);

        startTime = Time.time;
    }

    public override TaskStatus OnUpdate()
    {

        //if (startTime + AtkAnimClipLength < Time.time)
        //{
        //    return TaskStatus.Success;
        //}
        return TaskStatus.Running;
    }

    public override void OnEnd()
    {
        Debug.Log("ZombieAttack OnEnd");
        animator.SetBool("IsAttacking", false);

        startTime = 0f;
    }

    public override void OnReset()
    {
        Debug.Log("ZombieAttack OnReset");
    }


}
