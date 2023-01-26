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
            newPosition.z += m_Animator.GetFloat("Runspeed") * Time.deltaTime;
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
}
