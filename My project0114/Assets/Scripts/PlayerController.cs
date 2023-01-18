using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;

    public float Damage = 10f;

    public Camera mainCamera;

    public float Speed = 12f;
    public float Gravity = 20f;

    public Transform GroundCheck;// 需要检测和是否和地面接触的物体
    public float GroundDistance;
    public LayerMask GroundMask;

    public PlayerSwordAtk sword;

    private Vector3 m_Velocity;

    private bool m_IsGrounded;

    private float m_moveSpeed;

    private CharacterController controller;
    private Animator animator;

    private bool canRotate = true;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();


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
            }
        } 
    }

    // 帧事件调用
    public void AttackStart()
    {
        canRotate = false;

        int tick = (int)DateTime.Now.Ticks;
        sword.gameObject.GetComponent<BoxCollider>().enabled = true;
        sword.SetCurAtk(new AttackCommand() { attackid = tick, primaryDamage = Damage });
    }

    // 帧事件调用
    public void PreInput()
    {

        animator.SetBool("NeedATK1", false);
    }

    // 帧事件调用
    public void AttackEnd()
    {
        sword.gameObject.GetComponent<BoxCollider>().enabled = false;
        canRotate = true;
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
            Debug.Log("攻击！");
            animator.SetBool("NeedATK1", true);
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
            style_40_bold_rich);
       
        //if (GUI.Button(new Rect(0, 40, 40, 40), "按钮"))//点击的时候会返回true
        //{
        //    Debug.Log("被点击了");
        //}
    }
}
