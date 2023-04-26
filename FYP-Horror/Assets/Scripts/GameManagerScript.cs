using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public GameObject killer;
    public GameObject killerMovePosition;
    public GameObject player;
    public GameObject wardrobe;
    private GameObject wardrobeWithPlayer;
    private KillerScript killerScript;
    public MapSpawnAlgorithmScript mapSpawnAlgorithmScript;
    public bool hasPlayerBeenKilled = false;

    public int maxWardrobeSpawnChance;
    public int max1TileWallObstacleSpawnChance;
    public int max2x1TileWallObstacleSpawnChance;

    public GameObject tileObstacle1x1;
    public GameObject tileObstacle2x1;
    public GameObject tileObstacle2x2;
    public GameObject JumpscareObject;
    public AudioSource jumpscareAudio;
    private AudioManagerScript audioManagerScript;
    private MusicManagerScript musicManagerScript;
    public Canvas canvas;

    public GameObject deadBody;
    // Start is called before the first frame update
    void Start()
    {
        killerScript = killer.GetComponent<KillerScript>();
        //mapSpawnAlgorithmScript = this.gameObject.GetComponent<MapSpawnAlgorithmScript>();
        audioManagerScript = GameObject.Find("AudioManager").GetComponent<AudioManagerScript>();
        musicManagerScript = GameObject.Find("MusicManager").GetComponent<MusicManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playerEnteredWardrobe()
    {
        bool inVision = killer.GetComponent<FieldOfView>().getIsPlayerInVision();
        killerScript.willBearTrapBePlacedAtDestination = true;
        killerScript.isKillerMovePositionCollisionActive = true;
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

                killerMovePosition.transform.position = mapSpawnAlgorithmScript.cells[roomID + (counter * 50)].gameObject.transform.position + moveLocationDisplacement;
                break;
        }
    }

    public void setMovePositionToRandomRoom()
    {
        int randRoom = Random.Range(1, mapSpawnAlgorithmScript.straightCorridors.Count);

        Vector3 moveLocationDisplacement = new Vector3(-5, killerMovePosition.transform.position.y, 5);
        killerMovePosition.transform.position = mapSpawnAlgorithmScript.cells[mapSpawnAlgorithmScript.straightCorridors[randRoom]].gameObject.transform.position + moveLocationDisplacement;
    }

    public void playerHasBeenKilled()
    {
        player.SetActive(false);
        JumpscareObject.SetActive(true);
        canvas.enabled = false;
        audioManagerScript.playJumpscare();
        musicManagerScript.stopAllMusic();
    }

    public void playerHasBeenSeen()
    {
        musicManagerScript.playChaseMusic();
    }

    public void resetFromChase()
    {
        musicManagerScript.startFadeOut();
    }
}
