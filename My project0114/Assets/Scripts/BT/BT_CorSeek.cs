using UnityEngine;
using UnityEngine.AI;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
   
    public class BT_CorSeek : Action
    {
        [Tooltip("The GameObject that the agent is seeking")]
        public SharedGameObject target;
        [Tooltip("If target is null then use the target position")]
        public SharedVector3 targetPosition;



        private NavMeshAgent agent;

        private Animator animator;

        private ZombieController controller;

        public override void OnAwake()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            controller = GetComponent<ZombieController>();
        }

        public override void OnStart()
        {
            //SetDestination(Target());
            animator.SetBool("IsWalking", true);
        }

        public override void OnEnd()
        {
            animator.SetBool("IsWalking", false);
        }


        public override TaskStatus OnUpdate()
        {
            // 动画还在 攻击的时候就不设置目标点 在帧事件中设置
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                return TaskStatus.Running;
            }

            if (Time.frameCount % 20 == 0)
            {
                if (!SetDestination(Target()))
                {
                    Debug.Log($"<color=#ff0000>没有正确寻路 pending:{agent.pathPending}</color>");
                }
            }

            if (Time.frameCount % 22 == 0)
            {
                // 这样就可以放置错误寻路 错误的目的地了...
                agent.enabled = false;
                agent.enabled = true;
            }

            return TaskStatus.Running;
        }
        private bool SetDestination(Vector3 destination)
        {
            agent.isStopped = false;
            return agent.SetDestination(destination);
        }

        // Return targetPosition if target is null
        private Vector3 Target()
        {
            if (target.Value != null)
            {
                return target.Value.transform.position;
            }
            return targetPosition.Value;
        }

        public override void OnReset()
        {
            base.OnReset();
            target = null;
            targetPosition = Vector3.zero;
        }
    }
}