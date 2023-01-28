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

    // ��һֱ����·���� ���Ƿ���Running 
    public override TaskStatus OnUpdate()
    {
        // ÿ60֡�ж�һ���Ƿ�����Ұ��Χ��
        // �������Ұ��Χ�� ÿ60֡����һ��Ŀ�ĵ�
        bool canSeeTarget = false;
        if(Time.frameCount % 60 == 0)
        {
            if ((transform.position - target.Value.transform.position).magnitude <= controller.ViewRadius)
            {
                canSeeTarget = true;
            }
            else
            {
                // ���������Ұ��Χ ����һ��ʼ�Ͳ�����Ұ��Χ�� Ŀ�ĵ���Ϊ��ǰ��Pos
                canSeeTarget = false;
            }
            
            if (canSeeTarget)
            {
                Debug.Log("����Ŀ�ĵ�");
                SetDestination(Target());
            }
            else
            {
                SetDestination(transform.position);
            }
        }

        // ÿ1֡���һ���Ƿ���빥����Χ
        if((transform.position-target.Value.transform.position).magnitude<= controller.AttackRadius)
        {
            // 
            Debug.Log("���빥����Χ������");
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
    /// ���ص�ǰ·���±� ��·��λ��
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
