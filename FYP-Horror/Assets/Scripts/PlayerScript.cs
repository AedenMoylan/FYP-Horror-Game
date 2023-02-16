using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerScript : MonoBehaviour
{
    public TextMeshProUGUI wardrobeText;
    private bool hasWardrobeCollisionHappened;
    private GameObject wardrobe;
    private Vector3 wardrobePosition;
    private bool isPlayerInWardrobe = false;
    private Vector3 preWardrobePosition;

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
                    this.transform.position = new Vector3(5000,5000,5000);
                }
                //this.transform.position = wardrobePosition;
            }
        }
        if (isPlayerInWardrobe == true || Input.GetKeyDown("e"))
        {
            isPlayerInWardrobe = false;
            playerCamera.enabled = true;
            WardrobeCamera.enabled = false;
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
