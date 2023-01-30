using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AutoController : MonoBehaviour
{
    public Transform target;

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
    }
}
