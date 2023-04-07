using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public GameObject killer;
    public GameObject killerMovePosition;
    public GameObject player;
    private GameObject wardrobeWithPlayer;
    private KillerScript killerScript;
    public MapSpawnAlgorithmScript mapSpawnAlgorithmScript;
    // Start is called before the first frame update
    void Start()
    {
        killerScript = killer.GetComponent<KillerScript>();
        //mapSpawnAlgorithmScript = this.gameObject.GetComponent<MapSpawnAlgorithmScript>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playerEnteredWardrobe()
    {
        bool inVision = killer.GetComponent<FieldOfView>().getIsPlayerInVision();
        killerScript.willBearTrapBePlacedAtDestination = true;
        wardrobeWithPlayer = player.GetComponent<PlayerScript>().wardrobe;
        wardrobeWithPlayer.GetComponentInParent<WardrobeScript>().setIsPlayerInside(true);
        if (inVision == true)
        {
            killerMovePosition.transform.position = wardrobeWithPlayer.transform.Find("PlayerLeavePosition").gameObject.transform.position;
        }
    }

    public void setKillerMoveLocationToEndOfCorridor(string _moveDirection, GameObject _currentRoom)
    {
        int counter = 0;
        int roomID = _currentRoom.transform.parent.GetComponent<CellScript>().id;
        bool hasEndRoomBeenFound = false;
        Vector3 moveLocationDisplacement = new Vector3(-5, killerMovePosition.transform.position.y, 5);
        //mapSpawnAlgorithmScript = this.GetComponent<MapSpawnAlgorithmScript>();
        switch (_moveDirection)
        {
            case "East":

                while (hasEndRoomBeenFound == false)
                {
                    if (mapSpawnAlgorithmScript.cells[roomID + counter].GetComponent<CellScript>().roomTypeName.Equals("Curved Corridor") == true
                        || mapSpawnAlgorithmScript.cells[roomID + counter].GetComponent<CellScript>().roomTypeName.Equals("End Corridor") == true
                        || mapSpawnAlgorithmScript.cells[roomID + counter].GetComponent<CellScript>().roomTypeName.Equals("EMPTY") == true)
                    {
                        hasEndRoomBeenFound = true;
                    }
                    else
                    {
                        Debug.Log(mapSpawnAlgorithmScript.cells[roomID + counter].GetComponent<CellScript>().roomTypeName + " " + (roomID + counter));
                        counter++;
                    }
                }
                //while (mapSpawnAlgorithmScript.cells[roomID + counter].GetComponent<CellScript>().roomTypeName != "Curved Corridor" 
                //    || mapSpawnAlgorithmScript.cells[roomID + counter].GetComponent<CellScript>().roomTypeName != "End Corridor"
                //    || mapSpawnAlgorithmScript.cells[roomID + counter].GetComponent<CellScript>().roomTypeName != "EMPTY")
                //{
                //    Debug.Log(mapSpawnAlgorithmScript.cells[roomID + counter].GetComponent<CellScript>().roomTypeName + " " + (roomID + counter));
                //    counter++;
                //}

                killerMovePosition.transform.position = mapSpawnAlgorithmScript.cells[roomID + counter].gameObject.transform.position + moveLocationDisplacement;
                break;
            case "West":

                while (hasEndRoomBeenFound == false)
                {
                    if (mapSpawnAlgorithmScript.cells[roomID - counter].GetComponent<CellScript>().roomTypeName.Equals("Curved Corridor") == true
                        || mapSpawnAlgorithmScript.cells[roomID - counter].GetComponent<CellScript>().roomTypeName.Equals("End Corridor") == true
                        || mapSpawnAlgorithmScript.cells[roomID - counter].GetComponent<CellScript>().roomTypeName.Equals("EMPTY") == true)
                    {
                        hasEndRoomBeenFound = true;
                    }
                    else
                    {
                        Debug.Log(mapSpawnAlgorithmScript.cells[roomID - counter].GetComponent<CellScript>().roomTypeName + " " + (roomID - counter));
                        counter++;
                    }
                }

                //while (mapSpawnAlgorithmScript.cells[roomID - counter].GetComponent<CellScript>().roomTypeName != "Curved Corridor" 
                //    || mapSpawnAlgorithmScript.cells[roomID - counter].GetComponent<CellScript>().roomTypeName != "End Corridor"
                //    || mapSpawnAlgorithmScript.cells[roomID - counter].GetComponent<CellScript>().roomTypeName != "EMPTY")
                //{
                //    Debug.Log(mapSpawnAlgorithmScript.cells[roomID + counter].GetComponent<CellScript>().roomTypeName + " " + (roomID - counter));
                //    counter++;
                //}

                killerMovePosition.transform.position = mapSpawnAlgorithmScript.cells[roomID - counter].gameObject.transform.position + moveLocationDisplacement;
                break;
            case "North":

                while (hasEndRoomBeenFound == false)
                {
                    if (mapSpawnAlgorithmScript.cells[roomID - (counter * 50)].GetComponent<CellScript>().roomTypeName.Equals("Curved Corridor") == true
                        || mapSpawnAlgorithmScript.cells[roomID - (counter * 50)].GetComponent<CellScript>().roomTypeName.Equals("End Corridor") == true
                        || mapSpawnAlgorithmScript.cells[roomID - (counter * 50)].GetComponent<CellScript>().roomTypeName.Equals("EMPTY") == true)
                    {
                        hasEndRoomBeenFound = true;
                    }
                    else
                    {
                        Debug.Log(mapSpawnAlgorithmScript.cells[roomID - (counter * 50)].GetComponent<CellScript>().roomTypeName + " " + (roomID - (counter * 50)));
                        counter++;
                    }
                }
                //while (mapSpawnAlgorithmScript.cells[roomID - (counter * 50)].GetComponent<CellScript>().roomTypeName != "Curved Corridor"
                //    || mapSpawnAlgorithmScript.cells[roomID - (counter * 50)].GetComponent<CellScript>().roomTypeName != "End Corridor"
                //    || mapSpawnAlgorithmScript.cells[roomID - counter].GetComponent<CellScript>().roomTypeName != "EMPTY")
                //{
                //    Debug.Log(mapSpawnAlgorithmScript.cells[roomID + counter].GetComponent<CellScript>().roomTypeName + " " + (roomID - counter));
                //    counter++;
                //}

                killerMovePosition.transform.position = mapSpawnAlgorithmScript.cells[roomID - (counter * 50)].gameObject.transform.position + moveLocationDisplacement;
                break;
            case "South":

                while (hasEndRoomBeenFound == false)
                {
                    if (mapSpawnAlgorithmScript.cells[roomID + (counter * 50)].GetComponent<CellScript>().roomTypeName.Equals("Curved Corridor") == true
                        || mapSpawnAlgorithmScript.cells[roomID + (counter * 50)].GetComponent<CellScript>().roomTypeName.Equals("End Corridor") == true
                        || mapSpawnAlgorithmScript.cells[roomID + (counter * 50)].GetComponent<CellScript>().roomTypeName.Equals("EMPTY") == true
                        || mapSpawnAlgorithmScript.cells[roomID + (counter * 50)].GetComponent<CellScript>().roomTypeName.Equals("Spawn") == true)
                    {
                        hasEndRoomBeenFound = true;
                    }
                    else
                    {
                        Debug.Log(mapSpawnAlgorithmScript.cells[roomID + (counter * 50)].GetComponent<CellScript>().roomTypeName + " " + (roomID + (counter * 50)));
                        counter++;
                    }
                }

                //while (mapSpawnAlgorithmScript.cells[roomID + (counter * 50)].GetComponent<CellScript>().roomTypeName != "Curved Corridor"
                //    || mapSpawnAlgorithmScript.cells[roomID + (counter * 50)].GetComponent<CellScript>().roomTypeName != "End Corridor"
                //    || mapSpawnAlgorithmScript.cells[roomID + counter].GetComponent<CellScript>().roomTypeName != "EMPTY")
                //{
                //    Debug.Log(mapSpawnAlgorithmScript.cells[roomID + counter].GetComponent<CellScript>().roomTypeName + " " + (roomID + counter));
                //    counter++;
                //}

                killerMovePosition.transform.position = mapSpawnAlgorithmScript.cells[roomID + (counter * 50)].gameObject.transform.position + moveLocationDisplacement;
                break;
        }
    }
}
