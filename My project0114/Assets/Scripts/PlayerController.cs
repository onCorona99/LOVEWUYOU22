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

    public Transform GroundCheck;// ��Ҫ�����Ƿ�͵���Ӵ�������
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
    /// ����������
    /// </summary>
    public const int MAX_COUNT = 10;
    /// <summary>
    /// ���洦�ڼ�ⷶΧ�ڵ���ײ��
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
        // ����
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        // ���
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        // �󶨶���֡�¼�
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

    // ֡�¼�����
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

    // ֡�¼�����
    public void PreInput()
    {
        animator.SetBool("NeedATK1", false);
    }

    // ֡�¼�����
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
        // ÿ60ִ֡��һ��
        if(Time.frameCount%60 == 0)
        {
            //Debug.Log("�����Χ������");
            #region ����������Ӵ���λ�������ڲ�����ײ�壬�������Ǵ洢���ṩ�Ļ������С�
            //�ڷ�Χ�ڵ�������ײ������
            int colliderCount = Physics.OverlapSphereNonAlloc(transform.position, detect_radius, colliders, 1<<11);

            foreach (var item in BattleManager.instance.zombieList)
            {
                bool isItemInSight = false;
                for (int i = 0; i < colliderCount; i++)
                {
                    //Debug.Log("���ֵ��ˣ�");
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
    /// �ڱ༭�������Gizmos(һ������scene�����б��ڹۿ���ʵ�������п�ע�ͻ�ɾ��)
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        //���������߿�Χ
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
        // ƽ��
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            controller.Move(mainCamera.transform.TransformDirection(new Vector3(horizontalInput, 0, VerticalInput)) * moveSpeed * Time.deltaTime);
        }
    }

    private void Rotate()
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

    private void SimGravity()
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
        //Debug.Log("������");
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
       
        //if (GUI.Button(new Rect(0, 40, 40, 40), "��ť"))//�����ʱ��᷵��true
        //{
        //    Debug.Log("�������");
        //}
    }
}
