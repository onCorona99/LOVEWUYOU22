using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

public class BT_LookAtTarget : Action
{
    public SharedGameObject target;

    public SharedMonoBehaviour mono;

    private ZombieController controller;

    public override void OnAwake()
    {
        controller = mono.Value as ZombieController;
    }

    public override void OnStart()
    {
        //if (controller.CanRotate)
            transform.LookAt(target.Value.transform);
    }

    public override TaskStatus OnUpdate()
    {
        return TaskStatus.Success;
    }

}