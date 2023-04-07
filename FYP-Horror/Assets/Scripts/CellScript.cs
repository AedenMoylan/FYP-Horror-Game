using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellScript : MonoBehaviour
{
    public int id = 0;
    public string roomTypeName = "EMPTY";
    public bool canCurvedCorridorBePlaced = false;
    public Vector3 position;
    public int specialRoomEntranceDirection = -1;
    public bool doesRoomHaveWardrobe = false;
    public bool hasKillerWalkedInRoom = false;
    public GameObject roomObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setRoomObject(GameObject _roomObject)
    {
        roomObject = _roomObject;
    }
}
