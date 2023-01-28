using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance { get; private set; }

    public float moveSpeed = 5f;

    public float Damage = 10f;

    public Camera mainCamera;

    public float Speed = 12f;
    public float Gravity = 20f;

    public Transform GroundCheck;// 需要检测和是否和地面接触的物体
    public float GroundDistance;
    public LayerMask GroundMask;

    public PlayerSwordAtk sword;

    public ParticleSystem particle;

    public float detect_radius = 5f;

    private Vector3 m_Velocity;

    private bool m_IsGrounded;

    private float m_moveSpeed;

    private CharacterController controller;
    private Animator animator;

    private bool canRotate = true;

    /// <summary>
    /// 缓存器容量
    /// </summary>
    public const int MAX_COUNT = 10;
    /// <summary>
    /// 缓存处在检测范围内的碰撞体
    /// </summary>
    private Collider[] colliders;

    private ParticleSystem ps;

    public void Destroy()
    {
        if (instance != null)
        {
            instance = null;
            Destroy(gameObject);
        }
    }

    private void Awake()
    {
        // 单例
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        // 组件
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        // 绑定动画帧事件
        foreach (var clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name.Equals("Atk1"))
            {
                AnimationEvent ae1 = new AnimationEvent() { functionName = "AttackStart", time = 0f };
                AnimationEvent ae2 = new AnimationEvent() { functionName = "PreInput", time = clip.length * 0.3f };
                AnimationEvent ae3 = new AnimationEvent() { functionName = "AttackEnd", time = clip.length };
                clip.AddEvent(ae1);
                clip.AddEvent(ae2);
                clip.AddEvent(ae3);

                var main = particle.main;
                main.startLifetime = clip.length;
            }
        }

        colliders = new Collider[MAX_COUNT];

    }

    // 帧事件调用
    public void AttackStart()
    {
        Debug.Log("ZZK ATK STA");
        canRotate = false;

        int tick = (int)DateTime.Now.Ticks;
        //sword.collider.enabled = true;
        sword.SetCurAtk(new AttackCommand() { attackid = tick, primaryDamage = Damage });

        var atkRadius = 3f;
        var atkHeight = 0.255f;
        sword.DoAttack(atkRadius,atkHeight);

        ps = Instantiate(particle, transform.position+new Vector3(0,0.8f,0), transform.localRotation,transform);
        ps.Play();
    }

    // 帧事件调用
    public void PreInput()
    {
        animator.SetBool("NeedATK1", false);
    }

    // 帧事件调用
    public void AttackEnd()
    {
        Debug.Log("ZZK ATK END");
        //sword.collider.enabled = false;
        canRotate = true;

        ps.Stop();
    }

    void Update()
    {
        Rotate();
        Move();
        SimGravity();

        DoAnimMove();

        Attack();

        DetectEnemyAround();
    }

    private void DetectEnemyAround()
    {
        // 每60帧执行一次
        if(Time.frameCount%60 == 0)
        {
            //Debug.Log("检测周围敌人中");
            #region 计算与球体接触或位于球体内部的碰撞体，并将它们存储到提供的缓冲区中。
            //在范围内的所有碰撞体数量
            int colliderCount = Physics.OverlapSphereNonAlloc(transform.position, detect_radius, colliders, 1<<11);

            foreach (var item in BattleManager.instance.zombieList)
            {
                bool isItemInSight = false;
                for (int i = 0; i < colliderCount; i++)
                {
                    //Debug.Log("发现敌人！");
                    if (item == colliders[i].gameObject.GetComponent<ZombieController>())
                    {
                        isItemInSight = true;
                        item.SetSliderStatus(isItemInSight);
                        break;
                    }
                }
                if (!isItemInSight)
                    item.SetSliderStatus(false);
            }
            #endregion
        }
    }


    /// <summary>
    /// 在编辑器里绘制Gizmos(一般用于scene窗口中便于观看，实际运行中可注释或删除)
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        //绘制球形线框范围
        Gizmos.DrawWireSphere(transform.position, detect_radius);
    }

    private void DoAnimMove()
    {
        float xaxis = Input.GetAxis("Horizontal");
        float yaxis = Input.GetAxis("Vertical");

        xaxis = Input.GetAxisRaw("Horizontal") == 0f ? 0 : xaxis;
        yaxis = Input.GetAxisRaw("Vertical") == 0f ? 0 : yaxis;

        m_moveSpeed = new Vector2(Mathf.Abs(xaxis), Mathf.Abs(yaxis)).normalized.magnitude;

        animator.SetFloat("XAxis", Mathf.Abs(xaxis));
        animator.SetFloat("YAxis", Mathf.Abs(yaxis));

        animator.SetFloat("MoveSpeed", m_moveSpeed);
    }

    private void Move()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float VerticalInput = Input.GetAxis("Vertical");
        // 平移
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            controller.Move(mainCamera.transform.TransformDirection(new Vector3(horizontalInput, 0, VerticalInput)) * moveSpeed * Time.deltaTime);
        }
    }

    private void Rotate()
    {
        if (!canRotate) return;
        // 旋转
        if (Input.GetKey(KeyCode.W))
        {
            // 根据主相机的朝向决定人物的移动方向，下同
            controller.transform.eulerAngles = new Vector3(0f, mainCamera.transform.eulerAngles.y, 0f);
        }
        if (Input.GetKey(KeyCode.S))
        {
            controller.transform.eulerAngles = new Vector3(0f, mainCamera.transform.eulerAngles.y + 180f, 0f);
        }

        if (Input.GetKey(KeyCode.A))
        {
            controller.transform.eulerAngles = new Vector3(0f, mainCamera.transform.eulerAngles.y + 270f, 0f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            controller.transform.eulerAngles = new Vector3(0f, mainCamera.transform.eulerAngles.y + 90f, 0f);
        }

        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
        {
            controller.transform.eulerAngles = new Vector3(0f, mainCamera.transform.eulerAngles.y + 315f, 0f);
        }
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
        {
            controller.transform.eulerAngles = new Vector3(0f, mainCamera.transform.eulerAngles.y + 45f, 0f);
        }
        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
        {
            controller.transform.eulerAngles = new Vector3(0f, mainCamera.transform.eulerAngles.y + 225f, 0f);
        }
        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
        {
            controller.transform.eulerAngles = new Vector3(0f, mainCamera.transform.eulerAngles.y + 135f, 0f);
        }
    }

    private void SimGravity()
    {
        // 模拟重力
        m_IsGrounded = Physics.CheckSphere(GroundCheck.position, GroundDistance, GroundMask);
        if (m_IsGrounded && m_Velocity.y < 0)
        {
            m_Velocity.y = -2;
        }
        m_Velocity.y -= Time.deltaTime * Gravity;

        controller.Move(m_Velocity * Time.deltaTime);
    }

    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PressKeyCode(KeyCode.Alpha1);
        }
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            UpKeyCode(KeyCode.Alpha1);
            DoAttackAnim();
        }
    }

    public void PressKeyCode(KeyCode keyCode)
    {
        MainPanel.PressBtnSk(keyCode);
    }

    public void UpKeyCode(KeyCode keyCode)
    {
        MainPanel.UpBtnSk(keyCode);
    }

    public void DoAttackAnim()
    {
        //UIManager.in
        //Debug.Log("攻击！");
        animator.SetBool("NeedATK1", true);
    }

    private void OnGUI()
    {
        GUIStyle style_40_bold_rich = new GUIStyle() { fontSize = 40, fontStyle = FontStyle.Bold, richText = true };

        GUI.Label(new Rect(1620, 60, 100, 100), 
            $"<color=#ffff00>X:{Input.GetAxis("Horizontal")}\n" +
            $"Y:{Input.GetAxis("Vertical")}\n" +
            $"MoveSpeed:{m_moveSpeed}</color>", 
            style_40_bold_rich);
       
        //if (GUI.Button(new Rect(0, 40, 40, 40), "按钮"))//点击的时候会返回true
        //{
        //    Debug.Log("被点击了");
        //}
    }
}
