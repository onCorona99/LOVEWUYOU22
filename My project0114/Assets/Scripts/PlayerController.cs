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

    public Transform GroundCheck;// ��Ҫ�����Ƿ�͵���Ӵ�������
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

        // ��֡�¼�
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

    // ֡�¼�
    // ��ʼ����ʱ��ɫ���ɽ�����ת ����ʱ�������
    private void Begin_ATK1()
    {
        int tick = (int)DateTime.Now.Ticks;

        swordAtk.SetCurAtk(new AttackCommand() { attackid = tick ,primaryDamage = attackDamage});
        swordCollider.enabled = true;
        canRotate = false;
        Debug.Log($"Begin_ATK1 TICK:[{tick}]");

    }

    // ֡�¼�
    private void End_ATK1()
    {
        swordCollider.enabled = false;
        canRotate = true;
    }

    // ֡�¼�
    private void PreInput_ATK1()
    {
        Debug.Log("ִ����ATK01");
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
            // ģ������
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
            // ��ת
            if (Input.GetKey(KeyCode.W))
            {
                // ����������ĳ������������ƶ�������ͬ
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
            // ƽ��
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
            style_40_bold_rich);//��Ļ�����Ͻ�Ϊ��0��0���㣬Rect��x,y,w,h��
        
        
        
        
        GUI.Label(new Rect(200, 200, 200, 200), "��ƽ����02");//��Ļ�����Ͻ�Ϊ��0��0���㣬Rect��x,y,w,h��
                                                        // Debug.Log(Time.time);//���������Console��ͣ���ӡ�˸ı��˵�ʱ�䣬֤����OnGUI����ÿһ֡��������Ⱦ��
        //if (GUI.Button(new Rect(0, 40, 40, 40), "��ť"))//�����ʱ��᷵��true
        //{
        //    Debug.Log("�������");
        //}
    }
}
