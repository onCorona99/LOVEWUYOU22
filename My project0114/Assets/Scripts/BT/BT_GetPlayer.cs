using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

public class BT_GetPlayer : Action
{
    [Tooltip("»ñÈ¡Íæ¼Ò")]
    public SharedGameObject player;

    public override void OnStart()
    {
        player.Value = PlayerController.instance.gameObject;
    }

}