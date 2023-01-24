using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZombieController : MonoBehaviour
{
    private float m_maxHealth = 100f;
    private float m_curHealth = 100f;

    private float m_cylinderHeight = 1.6f;

    public GameObject SliderGO;

    public List<AttackCommand> AtkList = new List<AttackCommand>();

    private Animator animator;

    private Slider slider;

    private bool m_IsGrounded;

    public Transform GroundCheck;// ��Ҫ�����Ƿ�͵���Ӵ�������
    public float GroundDistance;
    public LayerMask GroundMask;

    public float Gravity = 20f;

    private Vector3 m_Velocity;

    private CharacterController controller;

    public float MaxHealth { get => m_maxHealth; set => m_maxHealth = value; }



    public float CurHealth { get => m_curHealth; set => m_curHealth = value; }
    public float CylinderHeight { get => m_cylinderHeight; set => m_cylinderHeight = value; }

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        slider = SliderGO.GetComponent<Slider>();
        m_cylinderHeight = GetComponent<CharacterController>().height;
    }

    private void Start()
    {
        slider.value = m_curHealth / m_maxHealth;
    }

    public void SetSliderStatus(bool status)
    {
        SliderGO.SetActive(status);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            animator.SetTrigger("IsDead");
        }

        if (SliderGO.activeInHierarchy)
        {
            SliderGO.transform.LookAt(PlayerController.instance.mainCamera.transform);
        }

        // ģ������
        SimGravity();
    }
    private void SimGravity()
    {
        // ģ������
        //m_IsGrounded = Physics.CheckSphere(GroundCheck.position, GroundDistance, GroundMask);
        //if (m_IsGrounded && m_Velocity.y < 0)
        //{
        //    m_Velocity.y = -2;
        //}
        //m_Velocity.y -= Time.deltaTime * Gravity;

        //controller.Move(m_Velocity * Time.deltaTime);
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
