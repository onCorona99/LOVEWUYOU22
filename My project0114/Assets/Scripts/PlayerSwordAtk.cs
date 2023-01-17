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
            // 说明在一次攻击指令中 触发器与僵尸进行了多次触发 直接返回
            if (ZBController.AtkList.Contains(curAtkCmd))
                return; 
            Debug.Log($"{other.gameObject.name}被击打了");
            ZBController.OnReceiveAnAttack(curAtkCmd);

        }
    }
}
