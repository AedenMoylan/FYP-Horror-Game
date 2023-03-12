using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerScript : MonoBehaviour
{
    public TextMeshProUGUI wardrobeText;
    private bool hasWardrobeCollisionHappened;
    private bool hasDoorCollisionHappened = false;
    private GameObject wardrobe;
    private Vector3 wardrobePosition;
    private bool isPlayerInWardrobe = false;
    private Vector3 preWardrobePosition;
    private GameObject doorObject;

    public Camera playerCamera;
    public Camera WardrobeCamera;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hasWardrobeCollisionHappened == true)
        {
            if (Input.GetKeyDown("e"))
            {
                if (isPlayerInWardrobe == false)
                {
                    isPlayerInWardrobe = true;
                    playerCamera.enabled = false;
                    WardrobeCamera.enabled = true;
                    preWardrobePosition = this.transform.position;
                    this.transform.position = new Vector3(5000, 5000, 5000);
                }
                else
                {
                    isPlayerInWardrobe = false;
                    playerCamera.enabled = true;
                    WardrobeCamera.enabled = false;
                }
            }
        }

        if (hasDoorCollisionHappened == true)
        {
            if (Input.GetKeyDown("e"))
            {
                doorObject.GetComponentInParent<DoorScript>().isDoorOpen = true;

                Debug.Log("Terset");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Wardrobe")
        {
            if (hasWardrobeCollisionHappened == false)
            {
                wardrobe = other.gameObject;
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

            //if (Input.GetKeyDown("e"))
            //{
            //    other.GetComponentInParent<DoorScript>().isDoorOpen = true;
            //}

        }

    }


    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Wardrobe")
        {
            wardrobeText.enabled = false;
            hasWardrobeCollisionHappened = false;
        }
    }
}
