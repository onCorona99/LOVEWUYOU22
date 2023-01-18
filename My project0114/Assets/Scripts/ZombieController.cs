using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    private float m_maxHealth = 100f;
    private float m_curHealth = 100f;

    public List<AttackCommand> AtkList = new List<AttackCommand>();

    private Animator animator;

    public float MaxHealth { get => m_maxHealth; set => m_maxHealth = value; }
    public float CurHealth { get => m_curHealth; set => m_curHealth = value; }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            //animator.SetBool("IsDead", true);
            animator.SetTrigger("IsDead");
        }
    }

    public void OnReceiveAnAttack(AttackCommand curAtkCmd)
    {
        AtkList.Add(curAtkCmd);
        m_curHealth -= curAtkCmd.primaryDamage;
        Debug.Log($"当前生命值为:[{m_curHealth}]");
    }
}
