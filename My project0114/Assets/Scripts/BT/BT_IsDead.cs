using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

public class BT_IsDead : Conditional
{
    public SharedMonoBehaviour zombieController;

    private ZombieController controller;

    public override void OnAwake()
    {
        zombieController.Value = GetComponent<ZombieController>();
        controller = (ZombieController)(zombieController.Value);
    }

    public override TaskStatus OnUpdate()
    {
        if (controller.IsDead)
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }

}