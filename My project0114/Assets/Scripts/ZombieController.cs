using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

    public GameObject HUD_GO;

    private PlayerCamera pcamera;

    [SerializeField]
    private float m_viewRadius = 20f;

    [SerializeField]
    private float m_attackRadius = 2f;

    public List<AttackCommand> AtkList = new List<AttackCommand>(); // �յ��������б�

    public float Gravity = 20f;

    private Vector3 m_Velocity;

    private CharacterController controller;
    private Animator animator;
    private Slider slider;
    private NavMeshAgent agent;
    private BehaviorTree btree;

    private float m_atkAnimLenght;

    private bool m_isDead;

    private bool m_canReceiveDamage = true;

    public float MaxHealth { get => m_maxHealth; set => m_maxHealth = value; }

    public float CurHealth { get => m_curHealth; set => m_curHealth = value; }
    public float CylinderHeight { get => m_cylinderHeight; set => m_cylinderHeight = value; }
    public float AttackRadius { get => m_attackRadius; set => m_attackRadius = value; }
    public float ViewRadius { get => m_viewRadius; set => m_viewRadius = value; }
    public float AtkAnimLenght { get => m_atkAnimLenght; set => m_atkAnimLenght = value; }
    public bool IsDead { get => m_isDead; set => m_isDead = value; }
    public bool CanReceiveDamage { get => m_canReceiveDamage; set => m_canReceiveDamage = value; }

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        slider = SliderGO.GetComponent<Slider>();
        m_cylinderHeight = GetComponent<CharacterController>().height;
        agent = GetComponent<NavMeshAgent>();
        btree = GetComponent<BehaviorTree>();
        pcamera = PlayerController.instance.mainCamera.GetComponent<PlayerCamera>();


        foreach (var clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name.Equals("Zombie_Attack_3"))
            {
                AnimationEvent ae1 = new() { functionName = "AttackStart", time = 0f };
                AnimationEvent ae2 = new() { functionName = "PreInput", time = clip.length * 0.3f };
                AnimationEvent ae3 = new() { functionName = "AttackEnd", time = clip.length };

                AddEvent(ae1);
                AddEvent(ae2);
                AddEvent(ae3);
            }
            if (clip.name.Equals("Death_2"))
            {
                AnimationEvent ae1 = new() { functionName = "DeathStart", time = 0f };
                AnimationEvent ae2 = new() { functionName = "DeathEnd", time = clip.length };
                AddEvent(ae1);
                AddEvent(ae2);
            }

            void AddEvent(AnimationEvent targetEvent)
            {
                bool isExist = false;
                foreach (var ae in clip.events)
                {
                    if (ae.functionName.Equals(targetEvent.functionName))
                    {
                        isExist = true;
                        break;
                    }
                }
                if (!isExist)
                    clip.AddEvent(targetEvent);
            }
        }
    }

    /// <summary>
    /// ���� �ڽ�������� Ϊ���޸�transform.position�� ��Ҫ���õ������������
    /// ARM:applyRootMotion  AUP:agent.updatePosition
    /// </summary>
    public void SetStatus_ARMandAUP(bool status)
    {
        animator.applyRootMotion = status;
        agent.updatePosition = status;
    }

    public IEnumerator SS(bool status)
    {
        yield return new WaitForSeconds(0.5f);
        SetStatus_ARMandAUP(status);
    }

    //private void OnAnimatorMove()
    //{
    //    //Debug.Log("OnAnimatorMove");
    //    //agent.velocity = animator.velocity;
    //    //agent.updatePosition = false;
    //    animator.ApplyBuiltinRootMotion();
    //}

    private void DeathStart()
    {
        // ��Ϊ���������Ǵ� any state �����death ���ó鴤 �÷�
        animator.SetBool("IsDead", false);
    }

    private void DeathEnd()
    {
        SetAllCompStatus(false);
    }

    public void SetAllCompStatus(bool status)
    {
        controller.detectCollisions = status;
        controller.enabled = status;
        animator.enabled = status;
        agent.isStopped = status;
        agent.enabled = !status;
        agent.enabled = status;
        btree.enabled = status;
        this.enabled = status;
    }


    // ֡�¼�����
    private void AttackStart()
    {
        agent.updateRotation = false;
        animator.rootRotation = Quaternion.LookRotation(PlayerController.instance.gameObject.transform.position - this.transform.position);

        //Quaternion dir = Quaternion.LookRotation(PlayerController.instance.gameObject.transform.position - this.transform.position);
        ////this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, dir, 1f);
        //this.transform.rotation = dir;

        Debug.Log($"<color=#ffff00>ִ����AttackStart֡�¼�</color>");
    }

    private void PreInput()
    {

    }

    private void AttackEnd()
    {
        agent.updateRotation = true;

        transform.LookAt(PlayerController.instance.transform);

        agent.ResetPath();
        agent.SetDestination(PlayerController.instance.transform.position);

        NavMeshPath path = null;

        if (path != null)
        {
            agent.SetPath(path);
            StringBuilder sb = new StringBuilder();
            foreach (var item in path.corners)
            {
                sb.Append(item).Append(" + ");
            }
            Debug.Log($"{sb}");
        }

        Debug.Log($"<color=#ffff00>ִ����AttackEnd֡�¼�</color> BOOL:");
    }

    private void Start()
    {
        m_curHealth = m_maxHealth;
        slider.value = m_curHealth / m_maxHealth;
    }

    private bool hasInvoke;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            transform.LookAt(PlayerController.instance.transform);
        }
    }


    private void LateUpdate()
    {
        if (HUD_GO.activeInHierarchy)
        {
            // ���������λ�� Ӧ���� �����ָ�򻭲������� �� �����forwardƽ���ϵ�ͶӰ���� ���ͶӰ����+����������λ��
            Transform playerCamTrans = PlayerController.instance.mainCamera.transform;

            Vector3 lookPoint = Vector3.ProjectOnPlane(HUD_GO.transform.position - playerCamTrans.position, playerCamTrans.forward);
            HUD_GO.transform.LookAt(playerCamTrans.transform.position + lookPoint);

            float x = pcamera.distance;
            // �޸�Ѫ����С 30 - 1   2 - 0.5
            float y = 1f / 56 * x + 13f / 28;
            SliderGO.GetComponent<RectTransform>().localScale = new Vector3(y, y, 1f);
        }
    }

    /// <summary>
    /// �ܵ�һ���˺�
    /// </summary>
    public void OnReceiveAnAttack(AttackCommand curAtkCmd)
    {
        AtkList.Add(curAtkCmd);
        m_curHealth -= curAtkCmd.primaryDamage;
        //Debug.Log($"��ǰ����ֵΪ:[{m_curHealth}]");
        SliderGO.GetComponent<Slider>().value = m_curHealth / m_maxHealth;

        // �ж��Ƿ�����
        if (m_curHealth == 0)
        {
            CanReceiveDamage = false;
            m_isDead = true;
            animator.SetBool("IsDead", true);
            BattleManager.instance.zombieList.Remove(this);
            SliderGO.SetActive(false);

        }
    }

    public void SetSliderStatus(bool status)
    {
        SliderGO.SetActive(status);
    }


    public void Destroy()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        Debug.Log("Zombie��������");
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        //���������߿�Χ
        Gizmos.DrawWireSphere(transform.position, m_attackRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_viewRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 5f);

        //if (Time.frameCount % 60 == 0)
        //{
        //    Task task = btree.FindTaskWithName("BT   Cor Seek"); // ֱ�Ӹ�����Ϊ���༭���е����ּ���

        //    GameObject go = btree.GetVariable("Player").GetValue() as GameObject;

        //}
        //if (!CanRotate)
        //{
        //    Debug.Log($"��ǰ������ת��");
        //}
    }
}
