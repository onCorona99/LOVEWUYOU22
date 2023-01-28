using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_CanRotate : Action
{
    public SharedGameObject target;

    public SharedBool canRotate;

    public override TaskStatus OnUpdate()
    {
        transform.GetComponent<ZombieController>().CanRotate = canRotate.Value;

        return TaskStatus.Success;
    }

}
