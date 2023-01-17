using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float attackDamage = 10f;

    public float moveSpeed = 5f;

    public Camera mainCamera;

    public float Speed = 12f;
    public float Gravity = 20f;

    public Transform GroundCheck;// 需要检测和是否和地面接触的物体
    public float GroundDistance;
    public LayerMask GroundMask;

    public PlayerSwordAtk swordAtk;
    public BoxCollider swordCollider;

    private Vector3 m_Velocity;

    private bool m_IsGrounded;

    private float m_moveSpeed;

    private bool canRotate = true;

    private CharacterController controller;
    private Animator animator;



    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        // 绑定帧事件
        foreach (var clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name.Equals("ATK1", System.StringComparison.OrdinalIgnoreCase))
            {
                AnimationEvent ae1 = new AnimationEvent() { functionName = "Begin_ATK1",time= 0f}; 
                AnimationEvent ae2 = new AnimationEvent() { functionName = "PreInput_ATK1",time= clip.length*0.2f}; 
                AnimationEvent ae3 = new AnimationEvent() { functionName = "End_ATK1",time= clip.length}; 
                clip.AddEvent(ae1);
                clip.AddEvent(ae2);
                clip.AddEvent(ae3);
            }
        } 
    }

    // 帧事件
    // 开始攻击时角色不可进行旋转 结束时解除限制
    private void Begin_ATK1()
    {
        int tick = (int)DateTime.Now.Ticks;

        swordAtk.SetCurAtk(new AttackCommand() { attackid = tick ,primaryDamage = attackDamage});
        swordCollider.enabled = true;
        canRotate = false;
        Debug.Log($"Begin_ATK1 TICK:[{tick}]");

    }

    // 帧事件
    private void End_ATK1()
    {
        swordCollider.enabled = false;
        canRotate = true;
    }

    // 帧事件
    private void PreInput_ATK1()
    {
        Debug.Log("执行了ATK01");
        animator.SetBool("NeedATK1", false);
    }

   
    void Update()
    {
        Rotate();
        Move();
        SimGravity();

        float xaxis = Input.GetAxis("Horizontal");
        float yaxis = Input.GetAxis("Vertical");

        xaxis = Input.GetAxisRaw("Horizontal") == 0f ? 0 : xaxis;
        yaxis = Input.GetAxisRaw("Vertical") == 0f ? 0 : yaxis;

        m_moveSpeed = new Vector2(Mathf.Abs(xaxis), Mathf.Abs(yaxis)).normalized.magnitude;

        animator.SetFloat("XAxis", Mathf.Abs(xaxis));
        animator.SetFloat("YAxis", Mathf.Abs(yaxis));

        animator.SetFloat("MoveSpeed", m_moveSpeed);

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            animator.SetBool("NeedATK1", true);
        }
    
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("ATK1"))
        {
        }

        void SimGravity()
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

        void Rotate()
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

        void Move()
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float VerticalInput = Input.GetAxis("Vertical");
            // 平移
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                controller.Move(mainCamera.transform.TransformDirection(new Vector3(horizontalInput, 0, VerticalInput)) * moveSpeed * Time.deltaTime);
            }
        }

    }

    private void OnGUI()
    {
        GUIStyle style_40_bold_rich = new GUIStyle() { fontSize = 40, fontStyle = FontStyle.Bold, richText = true };

        GUI.Label(new Rect(100, 100, 100, 100), 
            $"<color=#ffff00>X:{Input.GetAxis("Horizontal")}\n" +
            $"Y:{Input.GetAxis("Vertical")}\n" +
            $"MoveSpeed:{m_moveSpeed}</color>", 
            style_40_bold_rich);//屏幕的左上角为（0，0）点，Rect（x,y,w,h）
        
        
        
        
        GUI.Label(new Rect(200, 200, 200, 200), "心平气和02");//屏幕的左上角为（0，0）点，Rect（x,y,w,h）
                                                        // Debug.Log(Time.time);//这个测试在Console不停大打印了改变了的时间，证明了OnGUI会在每一帧被调用渲染。
        //if (GUI.Button(new Rect(0, 40, 40, 40), "按钮"))//点击的时候会返回true
        //{
        //    Debug.Log("被点击了");
        //}
    }
}
