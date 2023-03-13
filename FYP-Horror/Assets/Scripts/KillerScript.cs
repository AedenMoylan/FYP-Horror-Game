using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Progress;

public class KillerScript : MonoBehaviour
{
    private Animator m_Animator;

    public NavMeshAgent navAgent;

    public GameObject moveDestination;

    public bool hasPlayerBeenSeen = false;

    bool testBool = false;
    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        handleInput();

        if (m_Animator)
        {
            Vector3 newPosition = transform.position;
            transform.position = newPosition;
        }

        navAgent.SetDestination(moveDestination.transform.position);
    }

    private void handleInput()
    {
        if (Input.GetKeyDown("1"))
        {
            print("1 key was pressed");
            m_Animator.SetBool("Alert", true);
            m_Animator.SetBool("Walking", false);
        }

        if (Input.GetKeyDown("2"))
        {
            print("2 key was pressed");
            m_Animator.SetBool("Walking", true);
            m_Animator.SetBool("Alert", false);
        }
    }

    public void setToDie()
    {
        
        //m_Animator.

        foreach (AnimatorControllerParameter parameter in m_Animator.parameters)
        {
            m_Animator.SetBool(parameter.name, false);
        }

        m_Animator.SetBool("Dead", true);
    }

    public void setToHunt()
    {
        //foreach (AnimatorControllerParameter parameter in m_Animator.parameters)
        //{
        //    m_Animator.SetBool(parameter.name, false);
        //}

        //m_Animator.SetBool("Alert", true);
        //if (testBool == false)
        //{
        //    testBool = true;
            m_Animator.SetBool("Alert", true);
            m_Animator.SetBool("Walking", false);
        //}
    }
}
