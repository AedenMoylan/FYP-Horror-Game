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

    // max spawn chance for the room generation obstacle and decoration spawn
    public int maxWardrobeSpawnChance;
    public int max1TileWallObstacleSpawnChance;
    public int max2x1TileWallObstacleSpawnChance;
    public int max2x2TileObstacleSpawnChance;
    public int maxFloorDecorationChance;
    public int maxPaintingSpawnChance;

    public GameObject tileObstacle1x1;
    public GameObject tileObstacle2x1;
    public GameObject tileObstacle2x2;
    public GameObject JumpscareObject;
    public AudioSource jumpscareAudio;
    private AudioManagerScript audioManagerScript;
    private MusicManagerScript musicManagerScript;
    public Canvas canvas;

    public GameObject deadBody;

    public GameObject bed;
    public Vector3 bedPositionOffset;

    public GameObject wheelchair;

    // lists of obstacles and decorations to be placed
    public List<GameObject> decorations;
    public List<GameObject> floorObstacles;
    public List<GameObject> wallObstacles1x1;
    // Start is called before the first frame update
    void Start()
    {
        killerScript = killer.GetComponent<KillerScript>();
        audioManagerScript = GameObject.Find("AudioManager").GetComponent<AudioManagerScript>();
        musicManagerScript = GameObject.Find("MusicManager").GetComponent<MusicManagerScript>();
    }

    /// <summary>
    /// does necessary checks when player enters wardrobe such as if killer saw him enter.
    /// </summary>
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

    /// <summary>
    /// if killer loses sight of player, does the necessary checks and actions to make him keep walking to end of corridor
    /// </summary>
    /// <param name="_moveDirection"></param>
    /// <param name="_currentRoom"></param>
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
    /// <summary>
    /// sets move position to random room
    /// </summary>
    public void setMovePositionToRandomRoom()
    {
        int randRoom = Random.Range(1, mapSpawnAlgorithmScript.straightCorridors.Count);

        Vector3 moveLocationDisplacement = new Vector3(-5, killerMovePosition.transform.position.y, 5);
        killerMovePosition.transform.position = mapSpawnAlgorithmScript.cells[mapSpawnAlgorithmScript.straightCorridors[randRoom]].gameObject.transform.position + moveLocationDisplacement;
    }

    /// <summary>
    /// calls functions needed when player has been killed
    /// </summary>
    public void playerHasBeenKilled()
    {
        player.SetActive(false);
        JumpscareObject.SetActive(true);
        canvas.enabled = false;
        audioManagerScript.playJumpscare();
        musicManagerScript.stopAllMusic();
    }
    /// <summary>
    /// plays chase music if player has been seen
    /// </summary>
    public void playerHasBeenSeen()
    {
        musicManagerScript.playChaseMusic();
    }
    /// <summary>
    /// does necessary actions after a chase has ended
    /// </summary>
    public void resetFromChase()
    {
        musicManagerScript.startFadeOut();
    }

    /// <summary>
    /// returns a random room decoration from the list
    /// </summary>
    /// <returns></returns>
    public GameObject getRandomFloorDecoration()
    {
       int randDecorationNumber =  Random.Range(0, decorations.Count);

        return decorations[randDecorationNumber];
    }

    /// <summary>
    /// returns a random floor obstacle from the list
    /// </summary>
    /// <returns></returns>
    public GameObject getRandomFloorObstacle()
    {
        int randObstacleNumber = Random.Range(0, floorObstacles.Count);

        return floorObstacles[randObstacleNumber];
    }

    /// <summary>
    /// returns a random 1x1 wall obstacle from the list
    /// </summary>
    /// <returns></returns>
    public GameObject getRandom1x1WallObstacles()
    {
        int randObstacleNumber = Random.Range(0, wallObstacles1x1.Count);

        return wallObstacles1x1[randObstacleNumber];
    }
}
