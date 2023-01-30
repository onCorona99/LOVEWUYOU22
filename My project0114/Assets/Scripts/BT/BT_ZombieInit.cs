using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

public class BT_ZombieInit : Action
{
    public SharedGameObject target;

    private NavMeshAgent agent;

    private Animator animator;

    public override void OnAwake()
    {
        agent = GetComponent<NavMeshAgent>();
        //agent.updateRotation = false;
        //agent.updatePosition = false;


    }

    public override void OnStart()
    {

    }

    

}