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
            // ˵����һ�ι���ָ���� �������뽩ʬ�����˶�δ��� ֱ�ӷ���
            if (ZBController.AtkList.Contains(curAtkCmd))
                return;
            Debug.Log($"{other.gameObject.name}��������");
            ZBController.OnReceiveAnAttack(curAtkCmd);

        }
    }

    /// <summary>
    /// Բ���ι��� atkRadius:Բ�̰뾶 
    /// ��Ϊ��Բ�� �ͳ�ϸԲ������ײ
    /// </summary>
    public void DoAttack(float atkRadius, float atkHeight)
    {
        foreach (var item in BattleManager.instance.zombieList)
        {
            Vector3 playerPosV3 = PlayerController.instance.gameObject.transform.position;
            Vector3 enemyPosV3 = item.transform.position;

            Vector2 playerPosV2 = new Vector2(playerPosV3.x, playerPosV3.z);
            Vector2 enemyPosV2 = new Vector2(enemyPosV3.x, enemyPosV3.z);

            // �ȴ�Y�ḩ���ж� �·���Pos���ǽŵ�root��λ��
            float distSqr = (playerPosV2 - enemyPosV2).sqrMagnitude;
            if (distSqr < atkRadius * atkRadius)
            {
                // ���ж�Y�᷽���ֵ С��Բ���뾶/2+��ʬԲ���뾶/2 ˵����ײ��
                var deltaYLimitValue = atkHeight / 2 + item.CylinderHeight / 2;
                float deltaY = enemyPosV3.y - playerPosV3.y;
                deltaY = deltaY > 0 ? deltaY : -deltaY;
                if (deltaY < deltaYLimitValue)
                {
                    // ˵����һ�ι���ָ���� �������뽩ʬ�����˶�δ��� ֱ�ӷ���
                    if (item.AtkList.Contains(curAtkCmd))
                        return;
                    Debug.Log($"{item.gameObject.name}��������");
                    item.OnReceiveAnAttack(curAtkCmd);
                }
            }
        }
    }
}