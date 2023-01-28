using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;
using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;


[TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}SeekIcon.png")]
public class MyFollowAI : Action
{

    [Tooltip("The agent has arrived when the destination is less than the specified amount. This distance should be greater than or equal to the NavMeshAgent StoppingDistance.")]
    public SharedFloat arriveDistance = 0.2f;

    private Animator animator;

    private NavMeshAgent agent;

    [Tooltip("The GameObject that the agent is seeking")]
    public SharedGameObject target;

    [Tooltip("If target is null then use the target position")]
    public SharedVector3 targetPosition;

    private ZombieController controller;

    public override void OnAwake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        controller = GetComponent<ZombieController>();

        agent.stoppingDistance = arriveDistance.Value;
    }

    public override void OnStart()
    {
        target = PlayerController.instance.gameObject;
        
    }

    // 会一直绕着路点走 总是返回Running 
    public override TaskStatus OnUpdate()
    {
        // 每60帧判断一下是否在视野范围内
        // 如果在视野范围内 每60帧设置一下目的地
        bool canSeeTarget = false;
        if(Time.frameCount % 60 == 0)
        {
            if ((transform.position - target.Value.transform.position).magnitude <= controller.ViewRadius)
            {
                canSeeTarget = true;
            }
            else
            {
                // 如果脱离视野范围 或者一开始就不在视野范围内 目的地设为当前的Pos
                canSeeTarget = false;
            }
            
            if (canSeeTarget)
            {
                Debug.Log("设置目的地");
                SetDestination(Target());
            }
            else
            {
                SetDestination(transform.position);
            }
        }

        // 每1帧检测一下是否进入攻击范围
        if((transform.position-target.Value.transform.position).magnitude<= controller.AttackRadius)
        {
            // 
            Debug.Log("进入攻击范围！！！");
        }

        return TaskStatus.Running;
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
        if (target.Value != null)
        {
            return target.Value.transform.position;
        }
        return targetPosition.Value;
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
