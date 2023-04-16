using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCollisionScript : MonoBehaviour
{

    private RoomDecorationScript roomDecorationScript;
    // Start is called before the first frame update
    void Start()
    {
    }

    //// Update is called once per frame
    //void Update()
    //{
    //    //Debug.Log("collision");
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "DoorWall")
        {
            other.gameObject.SetActive(false);
        }

        if (other.gameObject.tag == "Right Wall")
        {
            roomDecorationScript = other.gameObject.GetComponentInParent<RoomDecorationScript>();
            roomDecorationScript.setWallAsNonPlaceable("Right");
        }
        else if (other.gameObject.tag == "Left Wall")
        {
            roomDecorationScript = other.gameObject.GetComponentInParent<RoomDecorationScript>();
            roomDecorationScript.setWallAsNonPlaceable("Left");
        }
        else if (other.gameObject.tag == "Top Wall")
        {
            roomDecorationScript = other.gameObject.GetComponentInParent<RoomDecorationScript>();
            roomDecorationScript.setWallAsNonPlaceable("Top");
        }
        else if (other.gameObject.tag == "Bottom Wall")
        {
            roomDecorationScript = other.gameObject.GetComponentInParent<RoomDecorationScript>();
            roomDecorationScript.setWallAsNonPlaceable("Bottom");
        }

    }
}
