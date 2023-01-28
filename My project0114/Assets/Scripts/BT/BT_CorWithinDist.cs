using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_CorWithinDist : Conditional
{
    public SharedMonoBehaviour mono;

    public SharedGameObject target;

    public SharedFloat distanse = 5f;

    private float sqrMagnitude;

    private ZombieController controller;

    public override void OnAwake()
    {
        controller = mono.Value as ZombieController;
    }

    public override void OnStart()
    {
        sqrMagnitude = distanse.Value * distanse.Value;
    }

    public override TaskStatus OnUpdate()
    {
        if(controller.IsDead)
            return TaskStatus.Failure;

        if (transform == null || target == null)
            return TaskStatus.Failure;

        Vector3 direction = target.Value.transform.position - transform.position;
        // check to see if the square magnitude is less than what is specified
        if (Vector3.SqrMagnitude(direction) < sqrMagnitude)
        {
            return TaskStatus.Success;
        }

        return TaskStatus.Failure;
    }
}
