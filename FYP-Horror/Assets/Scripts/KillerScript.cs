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

    private Vector3 groundPosition;

    private GameObject currentRoom;

    private Vector3 previousGroundPosition;

    private int groundCounter;

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManagerScript>();
        groundCounter = 0;
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

                string moveDirection = checkMovementDirection();
                gameManagerScript.setKillerMoveLocationToEndOfCorridor(moveDirection, currentRoom);

                switch (moveDirection)
                {
                    case "East":
                        Debug.Log("Heading East");
                        break;

                    case "West":
                        Debug.Log("Heading West");
                        break;
                    case "North":
                        Debug.Log("Heading North");
                        break;
                    case "South":
                        Debug.Log("Heading South");
                        break;
                }
            }
        }

        //Debug.Log(other.name);
        if (other.tag == "Ground")
        {
            Debug.Log("Floor collision");
            other.GetComponentInParent<MeshRenderer>().material.color = Color.red;

            if (groundCounter == 0)
            {

            }
            else if (groundCounter > 0)
            {
                previousGroundPosition = groundPosition;
            }

            groundCounter++;
            groundPosition = other.transform.position;
            currentRoom = other.transform.parent.gameObject;

            checkMovementDirection();
            Debug.Log("");
        }
    }

    public string checkMovementDirection()
    {
        string direction = "NULL";
        if (previousGroundPosition.x < groundPosition.x)
        {
            direction = "East";
        }
        else if (previousGroundPosition.x > groundPosition.x)
        {
            direction = "West";
        }
        else if (previousGroundPosition.z < groundPosition.z)
        {
            direction = "North";
        }
        else if (previousGroundPosition.z > groundPosition.z)
        {
            direction = "South";
        }
        return direction;
    }
}
