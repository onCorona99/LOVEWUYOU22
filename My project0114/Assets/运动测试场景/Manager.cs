using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Manager : MonoBehaviour
{
    public AutoController controller;


    private Animator animator;

    private bool turn2apply = false;

    private void Awake()
    {
        animator = controller.GetComponent<Animator>();
        animator.applyRootMotion = false;

        controller.gameObject.SetActive(false);
        //controller.GetComponent<NavMeshAgent>().enabled = false;
    }

    void Start()
    {
        
    }

    void Update()
    {
        //if (turn2apply)
        //{
        //    animator.applyRootMotion = true;
        //    turn2apply = false;

        //}

        if (Input.GetKeyDown(KeyCode.M))
        {

            //controller.GetComponent<NavMeshAgent>().enabled = true;
            controller.gameObject.SetActive(true);
            //canSetPos = true;
            controller.gameObject.transform.localPosition = new Vector3(4, 4, 4);
            //turn2apply = true;

            Invoke("Turn2", 0.05f);

            //animator.rootPosition = new Vector3(4, 4, 4);


        }

        

    }

    private void Turn2()
    {
        animator.applyRootMotion = true;
        turn2apply = false;
    }
}
