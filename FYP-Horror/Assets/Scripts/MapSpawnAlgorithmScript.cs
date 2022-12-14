using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MapSpawnAlgorithmScript : MonoBehaviour
{
    int spawnDirection1;
    int spawnDirection2;

    int spawnCorridor1Amount;
    int spawnCorridor2Amount;

    int spawnCellID = 1275;

    public List<int> roomCoordinates = new List<int>();
    public List<int> roomsToAdd = new List<int>();

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
        //spawnCorridorsDown(2130, 7); // looks good on border check spawns an extra corridor
        //spawnCorridorsUp(2499 - 2130, 7); // works but spawns TB corridor onto the end corrodor cell
        //spawnCorridorsRight(2144, 7); // same as up
        //spawnCorridorsLeft(2499 - 2144, 7); // same

        //placeCurvedCorridor(1942, 0);

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
        switch (spawnDirection1)
        {
            case 0: // up
                spawnCorridorsUp(spawnCellID, spawnCorridor1Amount, true);
                break;

            case 1: // RIGHT
                spawnCorridorsRight(spawnCellID, spawnCorridor1Amount, true);
                break;

            case 2: // Down
                spawnCorridorsDown(spawnCellID, spawnCorridor1Amount, true);
                break;

            case 3: // Left
                spawnCorridorsLeft(spawnCellID, spawnCorridor1Amount, true);
                break;

            default:
                print("INVALID SWITCH STATEMENT");
                break;
        }

        switch (spawnDirection2)
        {
            case 0: // TOP
                spawnCorridorsUp(spawnCellID, spawnCorridor2Amount, true);
                break;

            case 1: // RIGHT
                spawnCorridorsRight(spawnCellID, spawnCorridor2Amount, true);
                break;

            case 2: // Down
                spawnCorridorsDown(spawnCellID, spawnCorridor2Amount, true);
                break;

            case 3: // Left
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
                Instantiate(threeBLRCorridor, cells[t_id].GetComponent<CellScript>().position, Quaternion.identity);
                assignRoomType(t_id, "Curved Corridor");
                spawnCorridorsLeft(t_id, randHallAmount, false);
                spawnCorridorsRight(t_id, randHallAmount2, false);
                break;

            case 1: // Right Previous room
                Instantiate(threeLTBCorridor, cells[t_id].GetComponent<CellScript>().position, Quaternion.identity);
                assignRoomType(t_id, "Curved Corridor");
                spawnCorridorsUp(t_id, randHallAmount, false);
                spawnCorridorsDown(t_id, randHallAmount2, false);
                break;

            case 2: // Bottom Previous room
                Instantiate(threeTLRCorridor, cells[t_id].GetComponent<CellScript>().position, Quaternion.identity);
                assignRoomType(t_id, "Curved Corridor");
                spawnCorridorsLeft(t_id, randHallAmount, false);
                spawnCorridorsRight(t_id, randHallAmount2, false);
                break;

            case 3: // Left Previous room
                Instantiate(threeRTBCorridor, cells[t_id].GetComponent<CellScript>().position, Quaternion.identity);
                assignRoomType(t_id, "Curved Corridor");
                spawnCorridorsUp(t_id, randHallAmount, false);
                spawnCorridorsDown(t_id, randHallAmount2, false);
                break;
        }
    }

    void placedCurvedCorridorPathways(int t_id, int t_direction)
    {
        int randHallAmount = Random.Range(1, 7);
        int randHallAmount2 = Random.Range(1, 7);

        switch (t_direction)
        {
            case 0:
                spawnCorridorsLeft(t_id, randHallAmount, false);
                spawnCorridorsRight(t_id, randHallAmount2, false);
                break;

            case 1:
                spawnCorridorsDown(t_id, randHallAmount, false);
                spawnCorridorsUp(t_id, randHallAmount2, false);
                break;

            case 2:
                spawnCorridorsLeft(t_id, randHallAmount, false);
                spawnCorridorsRight(t_id, randHallAmount2, false);
                break;

            case 3:
                spawnCorridorsUp(t_id, randHallAmount, false);
                spawnCorridorsDown(t_id, randHallAmount2, false);
                break;

        }
    }

    void assignRoomType(int t_id, string t_roomType)
    {
        cells[t_id].GetComponent<CellScript>().roomTypeName = t_roomType;
    }

    bool checkIsRoomPlaceValid(int t_id, int t_direction)
    {
        switch (t_direction)
        {
            case 0: // Top Previous Room

                for (int i = 0; i < 3; i++)
                {
                    if (cells[t_id - 51 + i].GetComponent<CellScript>().roomTypeName != "EMPTY")
                    {
                        return false;
                    }
                    else if (cells[t_id - 101 + i].GetComponent<CellScript>().roomTypeName != "EMPTY")
                    {
                        return false;
                    }
                    else if (cells[t_id - 151 + i].GetComponent<CellScript>().roomTypeName != "EMPTY")
                    {
                        return false;
                    }
                }
                break;

            case 1: // Right Previous Room

                for (int i = 0; i < 3; i++)
                {
                    if (cells[t_id - 49 + (50 * i)].GetComponent<CellScript>().roomTypeName != "EMPTY")
                    {
                        return false;
                    }
                    else if (cells[t_id - 48 + (50 * i)].GetComponent<CellScript>().roomTypeName != "EMPTY")
                    {
                        return false;
                    }
                    else if (cells[t_id - 47 + (50 * i)].GetComponent<CellScript>().roomTypeName != "EMPTY")
                    {
                        return false;
                    }
                }
                break;

            case 2: // Bottom Previous Room
                for (int i = 0; i < 3; i++)
                {
                    if (cells[t_id + 49 + 50 + i].GetComponent<CellScript>().roomTypeName != "EMPTY")
                    {
                        return false;
                    }
                    else if (cells[t_id + 49 + 100 + i].GetComponent<CellScript>().roomTypeName != "EMPTY")
                    {
                        return false;
                    }
                    else if (cells[t_id + 49 + i].GetComponent<CellScript>().roomTypeName != "EMPTY")
                    {
                        return false;
                    }
                }
                break;

            case 3: // Left Previous Room
                for (int i = 0; i < 3; i++)
                {
                    if (cells[t_id - 51 + (50 * i)].GetComponent<CellScript>().roomTypeName != "EMPTY")
                    {
                        return false;
                    }
                    else if (cells[t_id - 52 + (50 * i)].GetComponent<CellScript>().roomTypeName != "EMPTY")
                    {
                        return false;
                    }
                    else if (cells[t_id - 53 + (50 * i)].GetComponent<CellScript>().roomTypeName != "EMPTY")
                    {
                        return false;
                    }
                }
                break;
        }
        return true;
    }

    void placeCorridorEnd(int t_id, int t_direction)
    {
        switch (t_direction)
        {
            case 0:
                Instantiate(topEntranceCorridorDeadEnd, cells[t_id].GetComponent<CellScript>().position, Quaternion.identity);
                assignRoomType(t_id , "End Corridor");

                break;

            case 1:
                Instantiate(rightEntranceCorridorDeadEnd, cells[t_id].GetComponent<CellScript>().position, Quaternion.identity);
                assignRoomType(t_id , "End Corridor");
                break;

            case 2:
                Instantiate(bottomEntranceCorridorDeadEnd, cells[t_id].GetComponent<CellScript>().position, Quaternion.identity);
                assignRoomType(t_id , "End Corridor");
                break;

            case 3:              
                Instantiate(leftEntranceCorridorDeadEnd, cells[t_id].GetComponent<CellScript>().position, Quaternion.identity);
                assignRoomType(t_id , "End Corridor");
                break;
        }
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
                    if (checkIsRoomPlaceValid(t_id, 3) == true)
                    {
                        Instantiate(twoBLCorridor, cells[t_id].GetComponent<CellScript>().position, Quaternion.identity);
                        spawnCorridorsLeft(t_id, randHallAmount, false);
                    }
                    else
                    {
                        placeCorridorEnd(t_id, 2);
                    }
                }
                else
                {
                    if (checkIsRoomPlaceValid(t_id, 1) == true)
                    {
                        Instantiate(twoBRCorridor, cells[t_id].GetComponent<CellScript>().position, Quaternion.identity);
                        spawnCorridorsRight(t_id, randHallAmount, false);
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
                    if (checkIsRoomPlaceValid(t_id, 0) == true)
                    {
                        Instantiate(twoTLCorridor, cells[t_id].GetComponent<CellScript>().position, Quaternion.identity);
                        spawnCorridorsUp(t_id, randHallAmount, false);
                    }
                    else
                    {
                        placeCorridorEnd(t_id, 3);
                    }
                }
                else
                {
                    if (checkIsRoomPlaceValid(t_id, 2) == true)
                    {
                        Instantiate(twoBLCorridor, cells[t_id].GetComponent<CellScript>().position, Quaternion.identity);
                        spawnCorridorsDown(t_id, randHallAmount, false);
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
                    if (checkIsRoomPlaceValid(t_id, 1) == true)
                    {
                        Instantiate(twoTRCorridor, cells[t_id].GetComponent<CellScript>().position, Quaternion.identity);
                        spawnCorridorsRight(t_id, randHallAmount, false);
                    }
                    else
                    {
                        placeCorridorEnd(t_id, 0);
                    }
                }
                else
                {
                    if (checkIsRoomPlaceValid(t_id, 3) == true)
                    {
                        Instantiate(twoTLCorridor, cells[t_id].GetComponent<CellScript>().position, Quaternion.identity);
                        spawnCorridorsLeft(t_id, randHallAmount, false);
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
                    if (checkIsRoomPlaceValid(t_id, 0) == true)
                    {
                        Instantiate(twoTRCorridor, cells[t_id].GetComponent<CellScript>().position, Quaternion.identity);
                        spawnCorridorsUp(t_id, randHallAmount, false);
                    }
                    else
                    {
                        placeCorridorEnd(t_id, 1);
                    }
                }
                else
                {
                    if (checkIsRoomPlaceValid(t_id, 2) == true)
                    {
                        Instantiate(twoBRCorridor, cells[t_id].GetComponent<CellScript>().position, Quaternion.identity);
                        spawnCorridorsDown(t_id, randHallAmount, false);
                    }
                    else
                    {
                        placeCorridorEnd(t_id, 1);
                    }
                }
                break;

        }
    }

    void spawnCorridorsUp(int t_id, int t_numberOfRooms, bool t_isCorridorMultidirectional)
    {
        for (int i = 0; i < t_numberOfRooms; i++)
        {
            if (checkIsRoomPlaceValid(t_id - (i * 50), 0) == false)
            {
                placeCorridorEnd(t_id - (i * 50), 2);
                assignRoomType(t_id - ((i + 1) * 50), "End Corridor");
                break;
            }
            else
            {
                Instantiate(verticalCorridor, cells[t_id - ((i + 1) * 50)].GetComponent<CellScript>().position, Quaternion.identity);
                assignRoomType(t_id - ((i + 1) * 50), "Vertical Corridor");

                if (i == t_numberOfRooms - 1)
                {
                    if (checkIsRoomPlaceValid(t_id - ((t_numberOfRooms + 1) * 50), 0) == false)
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

    void spawnCorridorsDown(int t_id, int t_numberOfRooms, bool t_isCorridorMultidirectional)
    {
        for (int i = 0; i < t_numberOfRooms; i++)
        {
            if (checkIsRoomPlaceValid(t_id + (i * 50), 2) == false)
            {
                placeCorridorEnd(t_id + (i * 50), 0);
                assignRoomType(t_id + ((i + 1) * 50), "End Corridor");
                break;
            }
            else
            {
                Instantiate(verticalCorridor, cells[t_id + ((i + 1) * 50)].GetComponent<CellScript>().position, Quaternion.identity);
                assignRoomType(t_id + ((i + 1) * 50), "Vertical Corridor");

                if (i == t_numberOfRooms - 1)
                {
                    if (checkIsRoomPlaceValid(t_id + ((t_numberOfRooms + 1) * 50), 2) == false)
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
            if (checkIsRoomPlaceValid(t_id + i, 1) == false)
            {
                placeCorridorEnd(t_id + i, 3);
                assignRoomType(t_id + i, "End Corridor");
                break;
            }
            else
            {
                Instantiate(horizontalCorridor, cells[t_id + (i + 1)].GetComponent<CellScript>().position, Quaternion.identity);
                assignRoomType(t_id + (i + 1), "Horizontal Corridor");

                if (i == t_numberOfRooms - 1)
                {
                    if (checkIsRoomPlaceValid(t_id + (t_numberOfRooms + 1), 1) == false)
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
            if (checkIsRoomPlaceValid(t_id - i, 3) == false)
            {
                placeCorridorEnd(t_id - i, 1);
                assignRoomType(t_id - (i + 1), "End Corridor");
                break;
            }
            else
            {
                Instantiate(horizontalCorridor, cells[t_id - (i + 1)].GetComponent<CellScript>().position, Quaternion.identity);
                assignRoomType(t_id - (i + 1), "Horizontal Corridor");

                if (i == t_numberOfRooms - 1)
                {
                    if (checkIsRoomPlaceValid(t_id - (t_numberOfRooms + 1), 3) == false)
                    {
                        placeCorridorEnd(t_id - t_numberOfRooms, 1);
                        assignRoomType(t_id - (t_numberOfRooms + 1), "End Corridor");
                    }
                    else
                    {
                        if (t_isCorridorMultidirectional == false)
                        {
                            placeCurvedCorridor(t_id - t_numberOfRooms - 1, 3);
                            assignRoomType(t_id - (t_numberOfRooms + 1), "Curved Corridor");
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

            if (cells[i].GetComponent<CellScript>().roomTypeName != "BORDER")
            {
                if (cells[i + 1].GetComponent<CellScript>().roomTypeName != "BORDER" && cells[i + 1].GetComponent<CellScript>().roomTypeName != "EMPTY" 
                    && cells[i + 1].GetComponent<CellScript>().roomTypeName != "Spawn" && cells[i + 1].GetComponent<CellScript>().roomTypeName != "Room")
                {
                    counter++;
                }
                if (cells[i - 1].GetComponent<CellScript>().roomTypeName != "BORDER" && cells[i - 1].GetComponent<CellScript>().roomTypeName != "EMPTY" 
                    && cells[i - 1].GetComponent<CellScript>().roomTypeName != "Spawn" && cells[i - 1].GetComponent<CellScript>().roomTypeName != "Room")
                {
                    counter++;
                }
                if (cells[i + 50].GetComponent<CellScript>().roomTypeName != "BORDER" && cells[i + 50].GetComponent<CellScript>().roomTypeName != "EMPTY" 
                    && cells[i + 50].GetComponent<CellScript>().roomTypeName != "Spawn" && cells[i + 50].GetComponent<CellScript>().roomTypeName != "Room")
                {
                    counter++;
                }
                if (cells[i - 50].GetComponent<CellScript>().roomTypeName != "BORDER" && cells[i - 50].GetComponent<CellScript>().roomTypeName != "EMPTY" 
                    && cells[i - 50].GetComponent<CellScript>().roomTypeName != "Spawn" && cells[i - 50].GetComponent<CellScript>().roomTypeName != "Room")
                {
                    counter++;
                }

                if (counter == 1)
                {
                    roomCoordinates.Add(i);
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

            if (roomsToAdd.Contains(roomCoordinates.ElementAt<int>(randNum)) == false)
            {
                roomsToAdd.Add(roomCoordinates.ElementAt<int>(randNum));
            }
            else
            {
                if (i != 0)
                i--;
            }
        }

        for (int i = 0; i < roomSpawnAmount; i++)
        {
            Instantiate(TestRoom2, cells[roomsToAdd.ElementAt<int>(i)].GetComponent<CellScript>().position, Quaternion.identity);
            assignRoomType(roomsToAdd.ElementAt<int>(i), "Room");
        }
    }
}
