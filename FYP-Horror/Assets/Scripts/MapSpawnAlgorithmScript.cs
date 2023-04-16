
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class MapSpawnAlgorithmScript : MonoBehaviour
{
    int spawnDirection1;
    int spawnDirection2;

    int spawnCorridor1Amount;
    int spawnCorridor2Amount;

    int spawnCellID = 1275;

    public List<int> roomCoordinates = new List<int>();
    public List<int> roomsToAdd = new List<int>();
    public List<int> nonStraightCorridors = new List<int>();
    public List<int> straightCorridors = new List<int>();

    const int MAX_CELLS = 50 * 50;
    public GameObject[] cells = new GameObject[MAX_CELLS];

    private int roomSpawnAmount = 10;

    public GameObject spawnRoom;
    public GameObject horizontalCorridor;
    public GameObject verticalCorridor;
    public GameObject threeTLRCorridor;
    public GameObject threeRTBCorridor;
    public GameObject threeBLRCorridor;
    public GameObject threeLTBCorridor;
    public GameObject TestRoom;
    public GameObject topEntranceCorridorDeadEnd;
    public GameObject rightEntranceCorridorDeadEnd;
    public GameObject bottomEntranceCorridorDeadEnd;
    public GameObject leftEntranceCorridorDeadEnd;
    public GameObject twoBLCorridor;
    public GameObject twoBRCorridor;
    public GameObject twoTRCorridor;
    public GameObject twoTLCorridor;
    public GameObject TestRoom2;

    public NavMeshSurface navSurface;

    GameObject roomObject = null;

    public KillerSpawnScript killerSpawnScript;
    // Start is called before the first frame update
    void Start()
    {
        spawnRoom = GameObject.FindGameObjectWithTag("SpawnPoint");
        spawnDirection1 = Random.Range(0, 4);
        spawnDirection2 = Random.Range(0, 4);

        spawnCorridor1Amount = Random.Range(3, 7);
        spawnCorridor2Amount = Random.Range(3, 7);

        while (spawnDirection2 == spawnDirection1)
        {
            spawnDirection2 = Random.Range(0, 4);
        }
        SetupGrid();
        assignBorder();
        assignRoomType(spawnCellID, "Spawn");

        SpawnStartCorridors();
        findValidIdsForRoomPlacement();
        spawnImportantRooms();

        navSurface.BuildNavMesh();
        //spawnCorridorsDown(2130, 7); // looks good on border check spawns an extra corridor
        //spawnCorridorsUp(2499 - 2130, 7); // works but spawns TB corridor onto the end corrodor cell
        //spawnCorridorsRight(2144, 7); // same as up
        //spawnCorridorsLeft(2499 - 2144, 7); // same

        //placeCurvedCorridor(1942, 0);

        deleteUnusedCells();

        killerSpawnScript.spawnKiller();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetupGrid()
    {
        int xValuedifference = 25 * 10;
        int zValuedifference = 25 * 10;
        int modulusAmount = 50;
        for (int i = 0; i < MAX_CELLS; i++)
        {
            if (i % modulusAmount == 0 && i != 0)
            {
                modulusAmount += 50;
                zValuedifference -= 1* 10;
                xValuedifference = 25 * 10;
            }
            Vector3 spawnPosition = new Vector3(- xValuedifference, 0, zValuedifference);
            cells[i] = GameObject.Instantiate(GameObject.FindGameObjectWithTag("CellTag"));
            cells[i].transform.position = spawnPosition;
            xValuedifference -= 10;

            cells[i].GetComponent<CellScript>().id = i;
            cells[i].GetComponent<CellScript>().position = spawnPosition;
        }
    }
    void SpawnStartCorridors()
    {
        Transform spawnTopWall = spawnRoom.transform.Find("Top Wall");
        Transform spawnBottomWall = spawnRoom.transform.Find("Bottom Wall");
        Transform spawnLeftWall = spawnRoom.transform.Find("Left Wall");
        Transform spawnRightWall = spawnRoom.transform.Find("Right Wall");
        switch (spawnDirection1)
        {
            case 0: // up
                spawnTopWall.gameObject.SetActive(false);
                spawnCorridorsUp(spawnCellID, spawnCorridor1Amount, true);

                break;

            case 1: // RIGHT
                spawnRightWall.gameObject.SetActive(false);
                spawnCorridorsRight(spawnCellID, spawnCorridor1Amount, true);
                break;

            case 2: // Down
                spawnBottomWall.gameObject.SetActive(false);
                spawnCorridorsDown(spawnCellID, spawnCorridor1Amount, true);
                break;

            case 3: // Left
                spawnLeftWall.gameObject.SetActive(false);
                spawnCorridorsLeft(spawnCellID, spawnCorridor1Amount, true);
                break;

            default:
                print("INVALID SWITCH STATEMENT");
                break;
        }

        switch (spawnDirection2)
        {
            case 0: // TOP
                spawnTopWall.gameObject.SetActive(false);
                spawnCorridorsUp(spawnCellID, spawnCorridor2Amount, true);
                break;

            case 1: // RIGHT
                spawnRightWall.gameObject.SetActive(false);
                spawnCorridorsRight(spawnCellID, spawnCorridor2Amount, true);
                break;

            case 2: // Down
                spawnBottomWall.gameObject.SetActive(false);
                spawnCorridorsDown(spawnCellID, spawnCorridor2Amount, true);
                break;

            case 3: // Left
                spawnLeftWall.gameObject.SetActive(false);
                spawnCorridorsLeft(spawnCellID, spawnCorridor2Amount, true);
                break;

            default:
                print("INVALID SWITCH STATEMENT");
                break;
        }
    }

    void placeMultiDirectionalCurvedCorridor(int t_id, int t_previousRoomDirection)
    {
        int randHallAmount = Random.Range(1, 7);
        int randHallAmount2 = Random.Range(1, 7);

        switch (t_previousRoomDirection)
        {
            case 0: // TOP Previous room
                roomObject = Instantiate(threeBLRCorridor, cells[t_id].GetComponent<CellScript>().position, Quaternion.identity);
                cells[t_id].GetComponent<CellScript>().setRoomObject(roomObject);
                assignRoomType(t_id, "Curved Corridor");
                spawnCorridorsLeft(t_id, randHallAmount, false);
                spawnCorridorsRight(t_id, randHallAmount2, false);
                break;

            case 1: // Right Previous room
                roomObject = Instantiate(threeLTBCorridor, cells[t_id].GetComponent<CellScript>().position, Quaternion.identity);
                cells[t_id].GetComponent<CellScript>().setRoomObject(roomObject);
                assignRoomType(t_id, "Curved Corridor");
                spawnCorridorsUp(t_id, randHallAmount, false);
                spawnCorridorsDown(t_id, randHallAmount2, false);
                break;

            case 2: // Bottom Previous room
                roomObject = Instantiate(threeTLRCorridor, cells[t_id].GetComponent<CellScript>().position, Quaternion.identity);
                cells[t_id].GetComponent<CellScript>().setRoomObject(roomObject);
                assignRoomType(t_id, "Curved Corridor");
                spawnCorridorsLeft(t_id, randHallAmount, false);
                spawnCorridorsRight(t_id, randHallAmount2, false);
                break;

            case 3: // Left Previous room
                roomObject = Instantiate(threeRTBCorridor, cells[t_id].GetComponent<CellScript>().position, Quaternion.identity);
                cells[t_id].GetComponent<CellScript>().setRoomObject(roomObject);
                assignRoomType(t_id, "Curved Corridor");
                spawnCorridorsUp(t_id, randHallAmount, false);
                spawnCorridorsDown(t_id, randHallAmount2, false);
                break;
        }
        nonStraightCorridors.Add(t_id);

    }

    void assignRoomType(int t_id, string t_roomType)
    {
        cells[t_id].GetComponent<CellScript>().roomTypeName = t_roomType;
    }


    int checkIfCorridorPlaceValid(int t_id, int t_direction)
    {
        switch (t_direction)
        {
            case 0: // Top Previous Room

                for (int i = 0; i < 3; i++)
                {
                    if (cells[t_id - 51 + i].GetComponent<CellScript>().roomTypeName != "EMPTY")
                    {
                        return -1;
                    }
                    else if (cells[t_id - 101 + i].GetComponent<CellScript>().roomTypeName != "EMPTY")
                    {
                        return 0;
                    }
                    else if (cells[t_id - 151 + i].GetComponent<CellScript>().roomTypeName != "EMPTY")
                    {
                        return 0;
                    }
                }
                break;

            case 1: // Right Previous Room

                for (int i = 0; i < 3; i++)
                {
                    if (cells[t_id - 49 + (50 * i)].GetComponent<CellScript>().roomTypeName != "EMPTY")
                    {
                        return -1;
                    }
                    else if (cells[t_id - 48 + (50 * i)].GetComponent<CellScript>().roomTypeName != "EMPTY")
                    {
                        return 0;
                    }
                    else if (cells[t_id - 47 + (50 * i)].GetComponent<CellScript>().roomTypeName != "EMPTY")
                    {
                        return 0;
                    }
                }
                break;

            case 2: // Bottom Previous Room
                for (int i = 0; i < 3; i++)
                {
                    if (cells[t_id + 49 + 50 + i].GetComponent<CellScript>().roomTypeName != "EMPTY")
                    {
                        return -1;
                    }
                    else if (cells[t_id + 49 + 100 + i].GetComponent<CellScript>().roomTypeName != "EMPTY")
                    {
                        return 0;
                    }
                    else if (cells[t_id + 49 + i].GetComponent<CellScript>().roomTypeName != "EMPTY")
                    {
                        return 0;
                    }
                }
                break;

            case 3: // Left Previous Room
                for (int i = 0; i < 3; i++)
                {
                    if (cells[t_id - 51 + (50 * i)].GetComponent<CellScript>().roomTypeName != "EMPTY")
                    {
                        return -1;
                    }
                    else if (cells[t_id - 52 + (50 * i)].GetComponent<CellScript>().roomTypeName != "EMPTY")
                    {
                        return 0;
                    }
                    else if (cells[t_id - 53 + (50 * i)].GetComponent<CellScript>().roomTypeName != "EMPTY")
                    {
                        return 0;
                    }
                }
                break;
        }
        return 1;
    }


    void placeCorridorEnd(int t_id, int t_direction)
    {
        switch (t_direction)
        {
            case 0:
                roomObject = Instantiate(topEntranceCorridorDeadEnd, cells[t_id].GetComponent<CellScript>().position, Quaternion.identity);
                assignRoomType(t_id , "End Corridor");
                killerSpawnScript.addSpawnCell(cells[t_id]);

                break;

            case 1:
                roomObject = Instantiate(rightEntranceCorridorDeadEnd, cells[t_id].GetComponent<CellScript>().position, Quaternion.identity);
                assignRoomType(t_id , "End Corridor");
                killerSpawnScript.addSpawnCell(cells[t_id]);
                break;

            case 2:
                roomObject = Instantiate(bottomEntranceCorridorDeadEnd, cells[t_id].GetComponent<CellScript>().position, Quaternion.identity);
                assignRoomType(t_id , "End Corridor");
                killerSpawnScript.addSpawnCell(cells[t_id]);
                break;

            case 3:              
                roomObject = Instantiate(leftEntranceCorridorDeadEnd, cells[t_id].GetComponent<CellScript>().position, Quaternion.identity);
                assignRoomType(t_id , "End Corridor");
                killerSpawnScript.addSpawnCell(cells[t_id]);
                break;
        }
        cells[t_id].GetComponent<CellScript>().setRoomObject(roomObject);
        nonStraightCorridors.Add(t_id);
    }

    void placeCurvedCorridor(int t_id, int t_direction)
    {
        int randNum = Random.Range(1, 3);
        int randHallAmount = Random.Range(1, 7);
        switch (t_direction)
        {
            case 0:
                if (randNum == 1)
                {
                    if (checkIfCorridorPlaceValid(t_id, 3) == 1)
                    {
                        roomObject = Instantiate(twoBLCorridor, cells[t_id].GetComponent<CellScript>().position, Quaternion.identity);
                        cells[t_id].GetComponent<CellScript>().setRoomObject(roomObject);
                        spawnCorridorsLeft(t_id, randHallAmount, false);
                        nonStraightCorridors.Add(t_id);
                    }
                    else
                    {
                        placeCorridorEnd(t_id, 2);
                    }
                }
                else
                {
                    if (checkIfCorridorPlaceValid(t_id, 1) == 1)
                    {
                        roomObject = Instantiate(twoBRCorridor, cells[t_id].GetComponent<CellScript>().position, Quaternion.identity);
                        cells[t_id].GetComponent<CellScript>().setRoomObject(roomObject);
                        spawnCorridorsRight(t_id, randHallAmount, false);
                        nonStraightCorridors.Add(t_id);
                    }
                    else
                    {
                        placeCorridorEnd(t_id, 2);
                    }
                }
                break;

            case 1:


                if (randNum == 1)
                {
                    if (checkIfCorridorPlaceValid(t_id, 0) == 1)
                    {
                        roomObject = Instantiate(twoTLCorridor, cells[t_id].GetComponent<CellScript>().position, Quaternion.identity);
                        cells[t_id].GetComponent<CellScript>().setRoomObject(roomObject);
                        spawnCorridorsUp(t_id, randHallAmount, false);
                        nonStraightCorridors.Add(t_id);
                    }
                    else
                    {
                        placeCorridorEnd(t_id, 3);
                    }
                }
                else
                {
                    if (checkIfCorridorPlaceValid(t_id, 2) == 1)
                    {
                        roomObject = Instantiate(twoBLCorridor, cells[t_id].GetComponent<CellScript>().position, Quaternion.identity);
                        cells[t_id].GetComponent<CellScript>().setRoomObject(roomObject);
                        spawnCorridorsDown(t_id, randHallAmount, false);
                        nonStraightCorridors.Add(t_id);
                    }
                    else
                    {
                        placeCorridorEnd(t_id, 3);
                    }
                }
                break;



            case 2:

                if (randNum == 1)
                {
                    if (checkIfCorridorPlaceValid(t_id, 1) == 1)
                    {
                        roomObject = Instantiate(twoTRCorridor, cells[t_id].GetComponent<CellScript>().position, Quaternion.identity);
                        cells[t_id].GetComponent<CellScript>().setRoomObject(roomObject);
                        spawnCorridorsRight(t_id, randHallAmount, false);
                        nonStraightCorridors.Add(t_id);
                    }
                    else
                    {
                        placeCorridorEnd(t_id, 0);
                    }
                }
                else
                {
                    if (checkIfCorridorPlaceValid(t_id, 3) == 1)
                    {
                        roomObject = Instantiate(twoTLCorridor, cells[t_id].GetComponent<CellScript>().position, Quaternion.identity);
                        cells[t_id].GetComponent<CellScript>().setRoomObject(roomObject);
                        spawnCorridorsLeft(t_id, randHallAmount, false);
                        nonStraightCorridors.Add(t_id);
                    }
                    else
                    {
                        placeCorridorEnd(t_id, 0);
                    }
                }
                break;

            case 3:
                if (randNum == 1)
                {
                    if (checkIfCorridorPlaceValid(t_id, 0) == 1)
                    {
                        roomObject = Instantiate(twoTRCorridor, cells[t_id].GetComponent<CellScript>().position, Quaternion.identity);
                        cells[t_id].GetComponent<CellScript>().setRoomObject(roomObject);
                        spawnCorridorsUp(t_id, randHallAmount, false);
                        nonStraightCorridors.Add(t_id);
                    }
                    else
                    {
                        placeCorridorEnd(t_id, 1);
                    }
                }
                else
                {
                    if (checkIfCorridorPlaceValid(t_id, 2) == 1)
                    {
                        roomObject = Instantiate(twoBRCorridor, cells[t_id].GetComponent<CellScript>().position, Quaternion.identity);
                        cells[t_id].GetComponent<CellScript>().setRoomObject(roomObject);
                        spawnCorridorsDown(t_id, randHallAmount, false);
                        nonStraightCorridors.Add(t_id);
                    }
                    else
                    {
                        placeCorridorEnd(t_id, 1);
                    }
                }
                break;
        }
    }
    // changed
    void spawnCorridorsUp(int t_id, int t_numberOfRooms, bool t_isCorridorMultidirectional)
    {
        for (int i = 0; i < t_numberOfRooms; i++)
        {
            if (checkIfCorridorPlaceValid(t_id - (i * 50), 0) == 0)
            {
                placeCorridorEnd((t_id - 50) - (i * 50), 2);
                assignRoomType((t_id - 50) - (i * 50), "End Corridor");
                break;
            }
            else
            {
                roomObject = Instantiate(verticalCorridor, cells[t_id - ((i + 1) * 50)].GetComponent<CellScript>().position, Quaternion.identity);
                straightCorridors.Add(t_id - ((i + 1) * 50));
                cells[t_id - ((i + 1) * 50)].GetComponent<CellScript>().setRoomObject(roomObject);
                assignRoomType(t_id - ((i + 1) * 50), "Vertical Corridor");

                if (i == t_numberOfRooms - 1)
                {
                    if (checkIfCorridorPlaceValid(t_id - ((t_numberOfRooms + 1) * 50), 0) == 0)
                    {
                        placeCorridorEnd(t_id - ((t_numberOfRooms + 1) * 50), 2);
                        //assignRoomType(t_id - ((t_numberOfRooms + 1) * 50), "End Corridor");
                    }
                    else
                    {
                        if (t_isCorridorMultidirectional == false)
                        {
                            placeCurvedCorridor(t_id - ((t_numberOfRooms + 1) * 50), 0);
                            assignRoomType(t_id - ((t_numberOfRooms + 1) * 50), "Curved Corridor");
                        }
                        else
                        {
                            placeMultiDirectionalCurvedCorridor(t_id - ((t_numberOfRooms + 1) * 50), 0);
                        }
                    }
                }
                
            }
        }
    }
    // changing
    void spawnCorridorsDown(int t_id, int t_numberOfRooms, bool t_isCorridorMultidirectional)
    {
        for (int i = 0; i < t_numberOfRooms; i++)
        {
            if (checkIfCorridorPlaceValid(t_id + (i * 50), 2) == 0)
            {
                placeCorridorEnd(t_id + ((i + 1) * 50), 0);
                assignRoomType(t_id + ((i + 1) * 50), "End Corridor");
                break;
            }
            else
            {
                roomObject = Instantiate(verticalCorridor, cells[t_id + ((i + 1) * 50)].GetComponent<CellScript>().position, Quaternion.identity);
                straightCorridors.Add(t_id + ((i + 1) * 50));
                cells[t_id + ((i + 1) * 50)].GetComponent<CellScript>().setRoomObject(roomObject);
                assignRoomType(t_id + ((i + 1) * 50), "Vertical Corridor");

                if (i == t_numberOfRooms - 1)
                {
                    if (checkIfCorridorPlaceValid(t_id + ((t_numberOfRooms + 1) * 50), 2) == 0)
                    {
                        placeCorridorEnd(t_id + ((t_numberOfRooms + 1) * 50), 0);
                        assignRoomType(t_id + ((t_numberOfRooms + 1) * 50), "End Corridor");
                    }
                    else
                    {
                        if (t_isCorridorMultidirectional == false)
                        {
                            placeCurvedCorridor(t_id + ((t_numberOfRooms + 1) * 50), 2);
                            assignRoomType(t_id + ((t_numberOfRooms + 1) * 50), "Curved Corridor");
                        }
                        else
                        {
                            placeMultiDirectionalCurvedCorridor(t_id + ((t_numberOfRooms + 1) * 50), 2);
                        }

                    }
                }
            }
        }
    }

    void spawnCorridorsRight(int t_id, int t_numberOfRooms, bool t_isCorridorMultidirectional)
    {
        for (int i = 0; i < t_numberOfRooms; i++)
        {
            if (checkIfCorridorPlaceValid(t_id + i, 1) == 0)
            {
                placeCorridorEnd(t_id + i, 3);
                assignRoomType(t_id + i, "End Corridor");
                break;
            }
            else
            {
                roomObject = Instantiate(horizontalCorridor, cells[t_id + (i + 1)].GetComponent<CellScript>().position, Quaternion.identity);
                straightCorridors.Add(t_id + (i + 1));
                cells[t_id + (i + 1)].GetComponent<CellScript>().setRoomObject(roomObject);
                assignRoomType(t_id + (i + 1), "Horizontal Corridor");

                if (i == t_numberOfRooms - 1)
                {
                    if (checkIfCorridorPlaceValid(t_id + (t_numberOfRooms + 1), 1) == 0)
                    {
                        placeCorridorEnd(t_id + (t_numberOfRooms + 1), 3);
                        assignRoomType(t_id + (t_numberOfRooms + 1), "End Corridor");
                    }
                    else
                    {
                        if (t_isCorridorMultidirectional == false)
                        {
                            placeCurvedCorridor(t_id + t_numberOfRooms + 1, 1);
                            assignRoomType(t_id + t_numberOfRooms + 1, "Curved Corridor");
                        }
                        else
                        {
                            placeMultiDirectionalCurvedCorridor(t_id + t_numberOfRooms + 1, 1);
                        }


                    }
                }

            }
        }
    }

    void spawnCorridorsLeft(int t_id, int t_numberOfRooms, bool t_isCorridorMultidirectional)
    {
        for (int i = 0; i < t_numberOfRooms; i++)
        {
            if (checkIfCorridorPlaceValid(t_id - i, 3) == 0)
            {
                placeCorridorEnd(t_id - (i + 1), 1);
                assignRoomType(t_id - (i + 1), "End Corridor");
                break;
            }
            else
            {
                roomObject = Instantiate(horizontalCorridor, cells[t_id - (i + 1)].GetComponent<CellScript>().position, Quaternion.identity);
                straightCorridors.Add(t_id - (i + 1));
                cells[t_id - (i + 1)].GetComponent<CellScript>().setRoomObject(roomObject);
                assignRoomType(t_id - (i + 1), "Horizontal Corridor");

                if (i == t_numberOfRooms - 1)
                {
                    if (checkIfCorridorPlaceValid(t_id - (t_numberOfRooms + 1), 3) == 0)
                    {
                        placeCorridorEnd(t_id - t_numberOfRooms, 1);
                        assignRoomType(t_id - t_numberOfRooms, "End Corridor");
                    }
                    else
                    {
                        if (t_isCorridorMultidirectional == false)
                        {
                            placeCurvedCorridor(t_id - t_numberOfRooms - 1, 3);
                            assignRoomType(t_id - t_numberOfRooms - 1, "Curved Corridor");
                        }
                        else
                        {
                            placeMultiDirectionalCurvedCorridor(t_id - t_numberOfRooms - 1, 3);
                        }


                    }
                }
            }
        }
    }

    void assignBorder()
    {
        for (int i = 0; i < 50; i++)
        {
            cells[i].GetComponent<CellScript>().roomTypeName = "BORDER";
            cells[50 * i].GetComponent<CellScript>().roomTypeName = "BORDER";
            cells[2499 - i].GetComponent<CellScript>().roomTypeName = "BORDER";
            cells[2499 - (50 * i)].GetComponent<CellScript>().roomTypeName = "BORDER";
        }
    }

    void findValidIdsForRoomPlacement()
    {
        for (int i = 0; i < MAX_CELLS; i++)
        {
            int counter = 0;
            int entranceDirection = -1;

            if (cells[i].GetComponent<CellScript>().roomTypeName != "BORDER")
            {
                if (cells[i + 1].GetComponent<CellScript>().roomTypeName != "BORDER" && cells[i + 1].GetComponent<CellScript>().roomTypeName != "EMPTY" 
                    /*&& cells[i + 1].GetComponent<CellScript>().roomTypeName != "Spawn"*/ && cells[i + 1].GetComponent<CellScript>().roomTypeName != "Room")
                {
                    entranceDirection = 1;
                    counter++;
                }
                if (cells[i - 1].GetComponent<CellScript>().roomTypeName != "BORDER" && cells[i - 1].GetComponent<CellScript>().roomTypeName != "EMPTY" 
                    && /*cells[i - 1].GetComponent<CellScript>().roomTypeName != "Spawn" &&*/ cells[i - 1].GetComponent<CellScript>().roomTypeName != "Room")
                {
                    entranceDirection = 3;
                    counter++;
                }
                if (cells[i + 50].GetComponent<CellScript>().roomTypeName != "BORDER" && cells[i + 50].GetComponent<CellScript>().roomTypeName != "EMPTY" 
                    && /*cells[i + 50].GetComponent<CellScript>().roomTypeName != "Spawn" &&*/ cells[i + 50].GetComponent<CellScript>().roomTypeName != "Room")
                {
                    entranceDirection = 2;
                    counter++;
                }
                if (cells[i - 50].GetComponent<CellScript>().roomTypeName != "BORDER" && cells[i - 50].GetComponent<CellScript>().roomTypeName != "EMPTY" 
                    /*&& cells[i - 50].GetComponent<CellScript>().roomTypeName != "Spawn"*/ && cells[i - 50].GetComponent<CellScript>().roomTypeName != "Room")
                {
                    entranceDirection = 0;
                    counter++;
                }

                if (counter == 1)
                {
                    roomCoordinates.Add(i);
                    cells[i].GetComponent<CellScript>().specialRoomEntranceDirection = entranceDirection;
                }
            }
        }
    }

    void spawnImportantRooms()
    {
        int count = roomCoordinates.Count;

        for (int i = 0; i < roomSpawnAmount; i++)
        {
            int randNum = Random.Range(1, count + 1);

            if (roomsToAdd.Contains(roomCoordinates.ElementAt<int>(randNum)) == false && isRoomPlaceValid(roomCoordinates.ElementAt<int>(randNum)) == true)
            {
                roomsToAdd.Add(roomCoordinates.ElementAt<int>(randNum));
                assignRoomType(roomsToAdd.ElementAt<int>(i), "Room");
            }
            else
            {
                if (i != 0)
                i--;
            }
        }

        for (int i = 0; i < roomSpawnAmount; i++)
        {
            int roomCoordinate = cells[roomsToAdd.ElementAt<int>(i)].GetComponent<CellScript>().id;
            int rotation = 90 * cells[roomsToAdd.ElementAt<int>(i)].GetComponent<CellScript>().specialRoomEntranceDirection;
            float rotationPositionXCorrecter = 0;
            float rotationPositionZCorrecter = 0;
            Vector3 position = cells[roomsToAdd.ElementAt<int>(i)].GetComponent<CellScript>().position;

            if (rotation == 0)
            {
                //removeCorridorWalls(cells[roomCoordinate - 50].GetComponent<CellScript>().id, 2);
            }
            else if (rotation == 90)
            {
                rotationPositionXCorrecter = -10;
            }
            else if (rotation == 180)
            {
                rotationPositionXCorrecter = -10;
                rotationPositionZCorrecter = 10;
            }
            else if (rotation == 270)
            {
                rotationPositionZCorrecter = 10;
            }
            roomObject = Instantiate(TestRoom2, new Vector3(position.x + rotationPositionXCorrecter, position.y - 0.5f,  position.z + rotationPositionZCorrecter), Quaternion.Euler(0,rotation,0));
            cells[roomsToAdd.ElementAt<int>(i)].GetComponent<CellScript>().setRoomObject(roomObject);
            //room.transform.Find("Collision Cube").transform.position
        }
    }

    bool isRoomPlaceValid(int t_id)
    {
        if (roomsToAdd.Contains(t_id - 50) || roomsToAdd.Contains(t_id + 50)
            || roomsToAdd.Contains(t_id + 1) || roomsToAdd.Contains((t_id - 1)))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void deleteUnusedCells()
    {
        for (int i = 0; i < MAX_CELLS; i++)
        {
            if (cells[i].GetComponent<CellScript>().roomTypeName == "EMPTY" || cells[i].GetComponent<CellScript>().roomTypeName == "BORDER")
            {
                Destroy(cells[i]);
            }
        }
    }
}
