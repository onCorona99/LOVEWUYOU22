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

    public List<AttackCommand> AtkList = new List<AttackCommand>(); // 收到攻击的列表

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
    /// 用于 在禁用物体后 为了修改transform.position的 需要禁用的两个组件属性
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
        // 因为动画机中是从 any state 进入的death 放置抽搐 置否
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


    // 帧事件调用
    private void AttackStart()
    {
        agent.updateRotation = false;
        animator.rootRotation = Quaternion.LookRotation(PlayerController.instance.gameObject.transform.position - this.transform.position);

        //Quaternion dir = Quaternion.LookRotation(PlayerController.instance.gameObject.transform.position - this.transform.position);
        ////this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, dir, 1f);
        //this.transform.rotation = dir;

        Debug.Log($"<color=#ffff00>执行了AttackStart帧事件</color>");
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

        Debug.Log($"<color=#ffff00>执行了AttackEnd帧事件</color> BOOL:");
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
            // 画布看向的位置 应该是 摄像机指向画布的向量 在 摄像机forward平面上的投影向量 这个投影向量+摄像机自身的位置
            Transform playerCamTrans = PlayerController.instance.mainCamera.transform;

            Vector3 lookPoint = Vector3.ProjectOnPlane(HUD_GO.transform.position - playerCamTrans.position, playerCamTrans.forward);
            HUD_GO.transform.LookAt(playerCamTrans.transform.position + lookPoint);

            float x = pcamera.distance;
            // 修改血条大小 30 - 1   2 - 0.5
            float y = 1f / 56 * x + 13f / 28;
            SliderGO.GetComponent<RectTransform>().localScale = new Vector3(y, y, 1f);
        }
    }

    /// <summary>
    /// 受到一次伤害
    /// </summary>
    public void OnReceiveAnAttack(AttackCommand curAtkCmd)
    {
        AtkList.Add(curAtkCmd);
        m_curHealth -= curAtkCmd.primaryDamage;
        //Debug.Log($"当前生命值为:[{m_curHealth}]");
        SliderGO.GetComponent<Slider>().value = m_curHealth / m_maxHealth;

        // 判断是否死亡
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
        Debug.Log("Zombie被销毁乐");
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        //绘制球形线框范围
        Gizmos.DrawWireSphere(transform.position, m_attackRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_viewRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 5f);

        //if (Time.frameCount % 60 == 0)
        //{
        //    Task task = btree.FindTaskWithName("BT   Cor Seek"); // 直接复制行为树编辑器中的名字即可

        //    GameObject go = btree.GetVariable("Player").GetValue() as GameObject;

        //}
        //if (!CanRotate)
        //{
        //    Debug.Log($"当前不能旋转！");
        //}
    }
}
