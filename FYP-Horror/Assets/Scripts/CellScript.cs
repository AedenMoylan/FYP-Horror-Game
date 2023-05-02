using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellScript : MonoBehaviour
{
    // cells hold relevant information about the room in that location
    public int id = 0;
    public string roomTypeName = "EMPTY";
    public bool canCurvedCorridorBePlaced = false;
    public Vector3 position;
    public int specialRoomEntranceDirection = -1;
    public bool doesRoomHaveWardrobe = false;
    public bool hasKillerWalkedInRoom = false;
    public GameObject roomObject;

    public void setRoomObject(GameObject _roomObject)
    {
        roomObject = _roomObject;

        roomObject.transform.parent = this.transform;
    }

    public void activateGunSpawnInRoom()
    {
        // this.GetComponentInChildren<GameObject>().transform.Find("pistol1").gameObject.SetActive(true);

        GameObject tempObject = this.gameObject.transform.Find("Meat Room(Clone)").gameObject;
        tempObject.transform.Find("pistol1").gameObject.SetActive(true);
        Debug.Log("GunSpawned");
    }
}
