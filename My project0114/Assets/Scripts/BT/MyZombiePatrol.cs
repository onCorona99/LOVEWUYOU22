using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}PatrolIcon.png")]
public class MyZombiePatrol : Action
{
    public SharedTransformList waypointList;

    [BehaviorDesigner.Runtime.Tasks.Tooltip("The length of time that the agent should pause when arriving at a waypoint")]
    public SharedFloat waypointPauseDuration = 0;

    [BehaviorDesigner.Runtime.Tasks.Tooltip("The agent has arrived when the destination is less than the specified amount. This distance should be greater than or equal to the NavMeshAgent StoppingDistance.")]
    public SharedFloat arriveDistance = 0.2f;

    private int waypointIndex;
    private float waypointReachedTime;

    private Animator animator;


    private NavMeshAgent agent;

    public override void OnAwake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        agent.stoppingDistance = arriveDistance.Value;
    }

    public override void OnStart()
    {
        // 最开始的目标是最近的路点
        float distance = Mathf.Infinity;
        float localDistance;
        for (int i = 0; i < waypointList.Value.Count; ++i)
        {
            if ((localDistance = Vector3.Magnitude(transform.position - waypointList.Value[i].transform.position)) < distance)
            {
                distance = localDistance;
                waypointIndex = i;
            }
        }
        waypointReachedTime = -1;
        SetDestination(Target());
    }

    /// <summary>
    /// Set a new pathfinding destination.
    /// </summary>
    protected bool SetDestination(Vector3 destination)
    {
        agent.isStopped = false;
        return agent.SetDestination(destination);
    }

    /// <summary>
    /// 返回当前路点下标 的路点位置
    /// </summary>
    private Vector3 Target()
    {
        if (waypointIndex >= waypointList.Value.Count)
        {
            return transform.position;
        }
        return waypointList.Value[waypointIndex].transform.position;
    }

    // 会一直绕着路点走 总是返回Running 
    public override TaskStatus OnUpdate()
    {
        if (waypointList.Value.Count == 0)
        {
            return TaskStatus.Failure;
        }
        if (HasArrived())
        {
            if (waypointReachedTime == -1)
            {
                waypointReachedTime = Time.time;
            }
            // wait the required duration before switching waypoints.
            if (waypointReachedTime + waypointPauseDuration.Value <= Time.time)
            {
                waypointIndex = (waypointIndex + 1) % waypointList.Value.Count;
                SetDestination(Target());
                waypointReachedTime = -1;
            }
        }

        if(waypointReachedTime!= -1)
        {
            // 不等于 -1 说明在等待
            animator.SetBool("IsWalking",false);
            animator.SetBool("IsIdling",true);
        }
        else
        {
            // 等于 -1 说明在朝目标前进
            animator.SetBool("IsWalking", true);
            animator.SetBool("IsIdling", false);
        }

        return TaskStatus.Running;
    }

    /// <summary>
    /// Has the agent arrived at the destination?
    /// </summary>
    /// <returns>True if the agent has arrived at the destination.</returns>
    protected bool HasArrived()
    {
        // The path hasn't been computed yet if the path is pending.
        float remainingDistance;
        if (agent.pathPending)
        {
            remainingDistance = float.PositiveInfinity;
        }
        else
        {
            remainingDistance = agent.remainingDistance;
        }

        return remainingDistance <= arriveDistance.Value;
    }

}
