using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ZombieController : MonoBehaviour
{
    [SerializeField]
    private float m_maxHealth = 100f;
    private float m_curHealth = 100f;

    private float m_cylinderHeight = 1.6f;

    public GameObject SliderGO;

    [SerializeField]
    private float m_viewRadius = 20f;

    [SerializeField]
    private float m_attackRadius = 2f;

    public List<AttackCommand> AtkList = new List<AttackCommand>(); // �յ��������б�



    private bool m_IsGrounded;

    public Transform GroundCheck;// ��Ҫ�����Ƿ�͵���Ӵ�������
    public float GroundDistance;
    public LayerMask GroundMask;

    public float Gravity = 20f;

    private Vector3 m_Velocity;

    private CharacterController controller;
    private Animator animator;
    private Slider slider;
    private NavMeshAgent agent;

    private float m_atkAnimLenght;

    private bool m_canRotate = true;

    private bool m_isDead;

    private bool m_canReceiveDamage = true;

    public float MaxHealth { get => m_maxHealth; set => m_maxHealth = value; }

    public float CurHealth { get => m_curHealth; set => m_curHealth = value; }
    public float CylinderHeight { get => m_cylinderHeight; set => m_cylinderHeight = value; }
    public float AttackRadius { get => m_attackRadius; set => m_attackRadius = value; }
    public float ViewRadius { get => m_viewRadius; set => m_viewRadius = value; }
    public float AtkAnimLenght { get => m_atkAnimLenght; set => m_atkAnimLenght = value; }
    public bool CanRotate { get => m_canRotate; set => m_canRotate = value; }
    public bool IsDead { get => m_isDead; set => m_isDead = value; }
    public bool CanReceiveDamage { get => m_canReceiveDamage; set => m_canReceiveDamage = value; }

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        slider = SliderGO.GetComponent<Slider>();
        m_cylinderHeight = GetComponent<CharacterController>().height;
        agent = GetComponent<NavMeshAgent>();

        // �󶨶���֡�¼�
        foreach (var clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name.Equals("Zombie_Attack_3"))
            {
                Debug.Log("�ҵ��˸ö���Ƭ��");
                AnimationEvent ae1 = new AnimationEvent() { functionName = "AttackStart", time = 0f };
                AnimationEvent ae2 = new AnimationEvent() { functionName = "PreInput", time = clip.length * 0.3f };
                AnimationEvent ae3 = new AnimationEvent() { functionName = "AttackEnd", time = clip.length };
                clip.AddEvent(ae1);
                clip.AddEvent(ae2);
                clip.AddEvent(ae3);

                m_atkAnimLenght = clip.length / 2f;// / animator.GetCurrentAnimatorStateInfo(1).speed;
                Debug.Log($"{clip.length} ++ {clip.length / 2f}");
                //var main = particle.main;
                //main.startLifetime = clip.length;
            }
            if (clip.name.Equals("Death_2"))
            {
                Debug.Log("�ҵ��˶���Ƭ��Death_2");
                AnimationEvent ae1 = new AnimationEvent() { functionName = "DeathStart", time = 0f };
                clip.AddEvent(ae1);
            }

        }

    }
    private void DeathStart()
    {
        // ��Ϊ���������Ǵ� any state �����death ���ó鴤 �÷�
        animator.SetBool("IsDead", false);
    }


    // ֡�¼�����
    private void AttackStart()
    {
        //transform.LookAt(PlayerController.instance.gameObject.transform);
        m_canRotate = false;

        //agent.updateRotation = true;

        //Quaternion dir = Quaternion.LookRotation(PlayerController.instance.gameObject.transform.position - this.transform.position);
        ////this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, dir, 1f);
        //this.transform.rotation = dir;


        Debug.Log($"ִ����AttackStart {animator.GetCurrentAnimatorStateInfo(0).speed} {animator.GetLayerIndex("Animations")} ");
    }

    private void PreInput()
    {

    }

    private void AttackEnd()
    {
        m_canRotate = true;

        //agent.updateRotation = false;
        Debug.Log("ִ����AttackEnd");
    }

    private void Start()
    {
        slider.value = m_curHealth / m_maxHealth;
    }



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            animator.SetBool("IsDead",true);
        }

        if (SliderGO.activeInHierarchy)
        {
            SliderGO.transform.LookAt(PlayerController.instance.mainCamera.transform);
        }
    }

    /// <summary>
    /// �ܵ�һ���˺�
    /// </summary>
    public void OnReceiveAnAttack(AttackCommand curAtkCmd)
    {
        AtkList.Add(curAtkCmd);
        m_curHealth -= curAtkCmd.primaryDamage;
        Debug.Log($"��ǰ����ֵΪ:[{m_curHealth}]");
        SliderGO.GetComponent<Slider>().value = m_curHealth / m_maxHealth;

        // �ж��Ƿ�����
        if(m_curHealth == 0)
        {
            CanReceiveDamage = false;
            m_isDead = true;
            animator.SetBool("IsDead",true);
        }
    }

    public void SetSliderStatus(bool status)
    {
        SliderGO.SetActive(status);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        //���������߿�Χ
        Gizmos.DrawWireSphere(transform.position, m_attackRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_viewRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 5f);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        Debug.Log("Zombie��������");
    }
}
