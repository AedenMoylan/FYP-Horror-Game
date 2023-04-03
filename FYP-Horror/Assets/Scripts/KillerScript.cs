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

    public GameObject bearTrap;

    private GameManagerScript gameManagerScript;

    public bool hasPlayerBeenSeen = false;

    public bool willBearTrapBePlacedAtDestination = false;

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
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

        //if (checkIfInFrontOfWardrobe() == true && willBearTrapBePlacedAtDestination == true)
        //{
        //    placeTrap();
        //}
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

        if (Input.GetKeyDown("3"))
        {
            placeTrap();
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

    public void placeTrap()
    {
        Instantiate(bearTrap, this.transform.position, Quaternion.identity);
    }

    public bool checkIfInFrontOfWardrobe()
    {
        if (this.transform.position.x >= moveDestination.transform.position.x - 1 || this.transform.position.x <= moveDestination.transform.position.x + 1
            || this.transform.position.z >= moveDestination.transform.position.z - 1 || this.transform.position.z <= moveDestination.transform.position.z + 1)
        {
            return true;
        }

            return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "MovePosition" )
        {
            if (willBearTrapBePlacedAtDestination == true)
            {
                Debug.Log("Placing Trap");
                placeTrap();
            }
        }
    }
}
