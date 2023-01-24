using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TempTest : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform targrt;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        agent.SetDestination(targrt.position);
        agent.stoppingDistance = 2f;
        agent.isStopped = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("∞¥œ¬¡ÀQ");
            agent.Move(new Vector3(2f, 0, 2f));
        }
    }
}
