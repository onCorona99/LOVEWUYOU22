using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;

    public Camera mainCamera;

    public float Speed = 12f;
    public float Gravity = 20f;

    public Transform GroundCheck;// ��Ҫ�����Ƿ�͵���Ӵ�������
    public float GroundDistance;
    public LayerMask GroundMask;


    private Vector3 m_Velocity;

    private bool m_IsGrounded;

    private float m_moveSpeed;

    private CharacterController controller;
    private Animator animator;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        
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
            Debug.Log("������");
            animator.SetBool("NeedATK1", true);
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

    public void CastATK01()
    {
        Debug.Log("ִ����ATK01");
        animator.SetBool("NeedATK1", false);
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
