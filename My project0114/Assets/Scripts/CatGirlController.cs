using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatGirlController : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (animator.GetBool("IsTrace"))
                animator.SetBool("IsTrace", false);
            else
                animator.SetBool("IsTrace", true);

        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            if (animator.GetBool("IsAttack"))
                animator.SetBool("IsAttack", false);
            else
                animator.SetBool("IsAttack", true);

        }
    }
}
