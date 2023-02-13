using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AutoController : MonoBehaviour
{
    public Transform target;
    public Transform T02;
    public Transform T03;

    private NavMeshAgent agent;
    private Animator animator;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        animator.SetBool("IsWalk", true);
        animator.SetBool("IsIdle", false);
        agent.SetDestination(target.position);

        //aabbcc = GameObject.Find("aabcc");
        //agent.updatePosition = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(agent.remainingDistance < agent.stoppingDistance)
        {
            animator.SetBool("IsWalk", false);
            animator.SetBool("IsIdle", true);
        }

        //if(Time.frameCount % 60 == 0)
        //{
        //    transform.LookAt(target);
        //    agent.SetDestination(target.position);
        //}
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (T02 == null)
            {
                Debug.Log("aabbcc is null");
            }
            else
            {
                agent.SetDestination(T02.position);
            }
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            agent.SetDestination(T03.position);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            transform.LookAt(target);
        }
        
    }
}
