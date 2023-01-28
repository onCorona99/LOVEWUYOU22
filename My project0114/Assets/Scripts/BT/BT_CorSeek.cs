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
            SetDestination(Target());
            animator.SetBool("IsWalking", true);
        }

        public override void OnEnd()
        {
            animator.SetBool("IsWalking", false);
        }


        public override TaskStatus OnUpdate()
        {
            if (controller.CanRotate)
                transform.LookAt(target.Value.transform);

            if (Time.frameCount % 60 == 0)
            {
                SetDestination(Target());
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