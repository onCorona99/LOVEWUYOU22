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



    public override void OnAwake()
    {

    }

    public override void OnStart()
    {
        transform.LookAt(target.Value.transform);
    }

    public override TaskStatus OnUpdate()
    {
        return TaskStatus.Success;
    }

}