using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordAtk : MonoBehaviour
{
    //public PlayerController playerController;

    public BoxCollider collider;

    public AttackCommand curAtkCmd;

    private void Awake()
    {
        collider = GetComponent<BoxCollider>();
        collider.enabled = false;
    }

    public void SetCurAtk(AttackCommand atk)
    {
        curAtkCmd = atk;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Zombie"))
        {
            ZombieController ZBController = other.gameObject.GetComponent<ZombieController>();
            // 说明在一次攻击指令中 触发器与僵尸进行了多次触发 直接返回
            if (ZBController.AtkList.Contains(curAtkCmd))
                return;
            Debug.Log($"{other.gameObject.name}被击打了");
            ZBController.OnReceiveAnAttack(curAtkCmd);

        }
    }

    /// <summary>
    /// 圆盘形攻击 atkRadius:圆盘半径 
    /// 简化为扁圆柱 和长细圆柱的碰撞
    /// </summary>
    public void DoAttack(float atkRadius, float atkHeight)
    {
        foreach (var item in BattleManager.instance.zombieList)
        {
            Vector3 playerPosV3 = PlayerController.instance.gameObject.transform.position;
            Vector3 enemyPosV3 = item.transform.position;

            Vector2 playerPosV2 = new Vector2(playerPosV3.x, playerPosV3.z);
            Vector2 enemyPosV2 = new Vector2(enemyPosV3.x, enemyPosV3.z);

            // 先从Y轴俯视判断 下方的Pos均是脚底root的位置
            float distSqr = (playerPosV2 - enemyPosV2).sqrMagnitude;
            if (distSqr < atkRadius * atkRadius)
            {
                // 在判断Y轴方向差值 小于圆柱半径/2+僵尸圆柱半径/2 说明碰撞到
                var deltaYLimitValue = atkHeight / 2 + item.CylinderHeight / 2;
                float deltaY = enemyPosV3.y - playerPosV3.y;
                deltaY = deltaY > 0 ? deltaY : -deltaY;
                if (deltaY < deltaYLimitValue)
                {
                    // 说明在一次攻击指令中 触发器与僵尸进行了多次触发 直接返回
                    if (item.AtkList.Contains(curAtkCmd))
                        return;
                    Debug.Log($"{item.gameObject.name}被击打了");
                    item.OnReceiveAnAttack(curAtkCmd);
                }
            }
        }
    }
}