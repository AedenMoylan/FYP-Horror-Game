using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerScript : MonoBehaviour
{
    public TextMeshProUGUI wardrobeText;
    private bool hasWardrobeCollisionHappened;
    private bool hasDoorCollisionHappened = false;
    public GameObject wardrobe;
    private GameManagerScript gameManagerScript;
    private Vector3 wardrobePosition;
    private bool isPlayerInWardrobe = false;
    //private Vector3 preWardrobePosition;
    private GameObject doorObject;
    private PlayerMovementScript playerMovementScript;
    private CharacterController characterController;
    public GameObject playerGun;

    public Camera playerCamera;
    public Camera WardrobeCamera;
    private bool isPlayerTrapped = false;
    private bool isPlayerInRoom = false;


    // Start is called before the first frame update
    void Start()
    {
        playerMovementScript = GetComponentInParent<PlayerMovementScript>();
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManagerScript>();
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // used to enter and leave player from wardrobe
        if (Input.GetKeyDown("e"))
        {
            if (isPlayerInWardrobe == false && hasWardrobeCollisionHappened == true)
            {
                gameManagerScript.playerEnteredWardrobe();
                isPlayerInWardrobe = true;
                playerCamera.enabled = false;
                WardrobeCamera.enabled = true;
                //preWardrobePosition = this.transform.position;
                //this.transform.position = new Vector3(5000, 5000, 5000);
                characterController.enabled = false;
                transform.position = new Vector3(5000, 5000, 5000);
                characterController.enabled = true;
            }
            else if (isPlayerInWardrobe == true)
            {
                isPlayerInWardrobe = false;
                playerCamera.enabled = true;
                WardrobeCamera.enabled = false;
                characterController.enabled = false;
                Transform leavePosition = wardrobe.transform.Find("PlayerLeavePosition").transform;
                this.transform.position = new Vector3(leavePosition.position.x, 0.5f, leavePosition.position.z);
                characterController.enabled = true;
                wardrobe.GetComponentInParent<WardrobeScript>().setIsPlayerInside(isPlayerInWardrobe);
            }
        }
        // used to open door
        if (hasDoorCollisionHappened == true)
        {
            if (Input.GetKeyDown("e"))
            {
                doorObject.GetComponentInParent<DoorScript>().isDoorOpen = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // lets player enter wardrobe if touching
        if (other.tag == "Wardrobe")
        {
            if (hasWardrobeCollisionHappened == false)
            {
                wardrobe = other.transform.parent.gameObject;
                wardrobe.transform.Find("WardrobeCamera").gameObject.SetActive(true);
                WardrobeCamera = wardrobe.transform.Find("WardrobeCamera").gameObject.GetComponent<Camera>();
                wardrobePosition = wardrobe.transform.position;
                wardrobeText.enabled = true;
                hasWardrobeCollisionHappened = true;
            }
        }
        
        else if (other.tag == "DoorCollider")
        {
            wardrobeText.enabled = false;
            Debug.Log("Door Collision");

            doorObject = other.gameObject;

            hasDoorCollisionHappened = true;
        }
        // traps player if touching a bear trap
        else if (other.tag == "BearTrap")
        {
            other.GetComponent<BearTrapScript>().setTrapBoolToTrue();
            other.GetComponent<AudioSource>().Play();
            isPlayerTrapped = true;
            playerMovementScript.changeCanPlayerMove(false);
        }

        else if (other.tag == "RoomFloor")
        {
            isPlayerInRoom = true;
        }

        else if (other.tag == "pistol")
        {
            playerGun.SetActive(true);
            other.gameObject.SetActive(false);
        }

    }


    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Wardrobe")
        {
            wardrobeText.enabled = false;
            wardrobe.transform.Find("WardrobeCamera").gameObject.SetActive(false);
            hasWardrobeCollisionHappened = false;
        }

        if (other.tag == "RoomFloor")
        {
            isPlayerInRoom = false;
        }
    }

    public bool getIsPlayerInWardrobe()
    {
        return isPlayerInWardrobe;
    }
}
