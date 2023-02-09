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
                Debug.Log("Wassup blud");
                this.transform.position = wardrobePosition;
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
