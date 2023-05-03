using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//using static UnityEditor.Progress;

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

    private bool hasPlayerBeenKilled = false;

    public bool isKillerMovePositionCollisionActive = false;

    public bool hasMovePositionBeenPlacedAtEndOfCorridor = false;

    public AudioSource trapPlaceAudio;

    private bool isChaseOccuring = false;

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

        if (m_Animator)
        {
            Vector3 newPosition = transform.position;
            transform.position = newPosition;
        }

        // sets movement destination
        navAgent.SetDestination(moveDestination.transform.position);

        // makes killer attack if player has been killed
        if (hasPlayerBeenKilled == true)
        {
            foreach (AnimatorControllerParameter parameter in m_Animator.parameters)
            {
                m_Animator.SetBool(parameter.name, false);
            }

        m_Animator.SetBool("Attack", true);
        }

    }

    /// <summary>
    /// sets killer animation to dead
    /// </summary>
    public void setToDie()
    {
        chaseCheck();

        foreach (AnimatorControllerParameter parameter in m_Animator.parameters)
        {
            m_Animator.SetBool(parameter.name, false);
        }

        m_Animator.SetBool("Dead", true);
    }

    /// <summary>
    /// sets killer animation to hunt
    /// </summary>
    public void setToHunt()
    {
        if (isChaseOccuring == false)
        {
            isChaseOccuring = true;
            gameManagerScript.playerHasBeenSeen();
        }
            foreach (AnimatorControllerParameter parameter in m_Animator.parameters)
            {
                m_Animator.SetBool(parameter.name, false);
            }

            m_Animator.SetBool("Alert", true);

            hasMovePositionBeenPlacedAtEndOfCorridor = false;
    }

    /// <summary>
    /// sets killer animation to walk
    /// </summary>
    public void setToWalk()
    {
        chaseCheck();
        foreach (AnimatorControllerParameter parameter in m_Animator.parameters)
        {
            m_Animator.SetBool(parameter.name, false);
        }

        m_Animator.SetBool("Walking", true);
    }

    /// <summary>
    /// spawns trap at the killer's location
    /// </summary>
    public void placeTrap()
    {
        Instantiate(bearTrap, this.transform.position, Quaternion.identity);
        trapPlaceAudio.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        // if killer touches the movePosition object, multiple checks are made.
        // look animation triggers and then move location changes if player has not been seen
        // if player has been seen, killer move position will get set to the end of the corridor.
        if (other.tag == "MovePosition" )
        {
            if (isKillerMovePositionCollisionActive == true)
            {
                if (willBearTrapBePlacedAtDestination == true)
                {
                    Debug.Log("Placing Trap");
                    willBearTrapBePlacedAtDestination = false;
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
                else
                {
                    isKillerMovePositionCollisionActive = false;
                    foreach (AnimatorControllerParameter parameter in m_Animator.parameters)
                    {
                        m_Animator.SetBool(parameter.name, false);
                    }
                    m_Animator.SetBool("Look", true);
                    StartCoroutine(lookAnimation());
                }
            }

            if (hasMovePositionBeenPlacedAtEndOfCorridor == true)
            {
                foreach (AnimatorControllerParameter parameter in m_Animator.parameters)
                {
                    m_Animator.SetBool(parameter.name, false);
                }
                m_Animator.SetBool("Look", true);
                StartCoroutine(lookAnimation());
            }
            else if (hasMovePositionBeenPlacedAtEndOfCorridor == false)
            {
                
                string moveDirection = checkMovementDirection();
                gameManagerScript.setKillerMoveLocationToEndOfCorridor(moveDirection, currentRoom);
                hasMovePositionBeenPlacedAtEndOfCorridor = true;
            }
        }

        //Debug.Log(other.name);
        if (other.tag == "Ground")
        {
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
        }

        if (other.tag == "Player")
        {
            if ( hasPlayerBeenKilled == false)
            {
                hasPlayerBeenKilled = true;

                gameManagerScript.playerHasBeenKilled();
            }
        }
    }

    /// <summary>
    /// checks direction the killer is moving
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// resets killer animation after looking arounf for 8 seconds
    /// </summary>
    /// <returns></returns>
    IEnumerator lookAnimation()
    {
        if (isChaseOccuring == true)
        {

        }
        else
        {
            chaseCheck();
        }
        yield return new WaitForSeconds(8);
        if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("LookAround"))
        {
            gameManagerScript.setMovePositionToRandomRoom();
            hasMovePositionBeenPlacedAtEndOfCorridor = false;
            setToWalk();
        }
    }

    /// <summary>
    /// checks if a chase is occuring
    /// </summary>
    public void chaseCheck()
    {
        if (isChaseOccuring == true)
        {
            isChaseOccuring = false;
            gameManagerScript.resetFromChase();
        }
    }

}
