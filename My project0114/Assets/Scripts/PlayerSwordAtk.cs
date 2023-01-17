using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordAtk : MonoBehaviour
{
    public PlayerController playerController;

    public AttackCommand curAtkCmd;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
