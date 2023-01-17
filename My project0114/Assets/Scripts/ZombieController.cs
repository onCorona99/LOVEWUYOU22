using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    private float m_maxHealth = 100f;
    private float m_curHealth = 100f;


    private Animator animator;

    private List<AttackCommand> atkList = new List<AttackCommand>(); // 受击列表

    public float MaxHealth { get => m_maxHealth; set => m_maxHealth = value; }
    public float CurHealth { get => m_curHealth; set => m_curHealth = value; }
    public List<AttackCommand> AtkList { get => atkList; set => atkList = value; }

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

    public void OnReceiveAnAttack(AttackCommand atk)
    {
        AtkList.Add(atk);
        m_curHealth -= atk.primaryDamage;
        Debug.Log($"当前生命值为{m_curHealth}");
    }
}
