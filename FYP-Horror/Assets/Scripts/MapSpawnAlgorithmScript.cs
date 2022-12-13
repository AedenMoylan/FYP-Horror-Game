using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MapSpawnAlgorithmScript : MonoBehaviour
{
    int spawnDirection1;
    int spawnDirection2;

    int spawnCorridor1Amount;
    int spawnCorridor2Amount;

    int spawnCellID = 1275;


    const int MAX_CELLS = 50 * 50;
    public GameObject[] cells = new GameObject[MAX_CELLS];

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
    // Start is called before the first frame update
    void Start()
    {
        spawnRoom = GameObject.FindGameObjectWithTag("SpawnPoint");
        spawnDirection1 = Random.Range(0, 4);
        spawnDirection2 = Random.Range(0, 4);


        Debug.Log(spawnDirection1);
        spawnCorridor1Amount = Random.Range(3, 7);
        spawnCorridor2Amount = Random.Range(3, 7);

        while (spawnDirection2 == spawnDirection1)
        {
            spawnDirection2 = Random.Range(0, 4);
        }
        SetupGrid();
        assignBorder();
        SpawnStartCorridors();
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
            case 0: // TOP
                for (int i = 0; i < spawnCorridor1Amount; i++)
                {
                    cells[spawnCellID - (50 * (i + 1))].GetComponent<CellScript>().roomTypeName = "Vertical Corridor";
                    Instantiate(verticalCorridor, cells[spawnCellID - (50 * (i + 1))].GetComponent<CellScript>().position, Quaternion.identity);
                    if (i == spawnCorridor1Amount - 1)
                    {
                        cells[spawnCellID - (50 * (i + 1))].GetComponent<CellScript>().canCurvedCorridorBePlaced = true;
                        placeMultiDirectionalCurvedCorridor(spawnCellID - (50 * (i + 2)), 2);
                        placedCurvedCorridorPathways(spawnCellID - (50 * (i + 2)), 2);
                    }
                }
                break;
            case 1: // RIGHT
                for (int i = 0; i < spawnCorridor1Amount; i++)
                {
                    cells[spawnCellID + (i + 1)].GetComponent<CellScript>().roomTypeName = "Vertical Corridor";
                    Instantiate(horizontalCorridor, cells[spawnCellID + (i + 1)].GetComponent<CellScript>().position, Quaternion.identity);
                    if (i == spawnCorridor1Amount - 1)
                    {
                        cells[spawnCellID + (i + 1)].GetComponent<CellScript>().canCurvedCorridorBePlaced = true;
                        placeMultiDirectionalCurvedCorridor(spawnCellID + (i + 2), 3);
                        placedCurvedCorridorPathways(spawnCellID + (i + 2), 3);
                    }
                }
                break;
            case 2: // Down
                for (int i = 0; i < spawnCorridor1Amount; i++)
                {
                    cells[spawnCellID + (50 * (i + 1))].GetComponent<CellScript>().roomTypeName = "Vertical Corridor";
                    Instantiate(verticalCorridor, cells[spawnCellID + (50 * (i + 1))].GetComponent<CellScript>().position, Quaternion.identity);
                    if (i == spawnCorridor1Amount - 1)
                    {
                        cells[spawnCellID + (50 * (i + 1))].GetComponent<CellScript>().canCurvedCorridorBePlaced = true;
                        placeMultiDirectionalCurvedCorridor(spawnCellID + (50 * (i + 2)), 0);
                        placedCurvedCorridorPathways(spawnCellID + (50 * (i + 2)), 0);
                    }
                }
                break;
            case 3: // Left
                for (int i = 0; i < spawnCorridor1Amount; i++)
                {
                    cells[spawnCellID - (i + 1)].GetComponent<CellScript>().roomTypeName = "Vertical Corridor";
                    Instantiate(horizontalCorridor, cells[spawnCellID - (i + 1)].GetComponent<CellScript>().position, Quaternion.identity);
                    if (i == spawnCorridor1Amount - 1)
                    {
                        cells[spawnCellID - (i + 1)].GetComponent<CellScript>().canCurvedCorridorBePlaced = true;
                        placeMultiDirectionalCurvedCorridor(spawnCellID - (i + 2), 1);
                        placedCurvedCorridorPathways(spawnCellID - (i + 2), 1);
                    }
                }
                break;
            default:
                print("INVALID SWITCH STATEMENT");
                break;
        }

        switch (spawnDirection2)
        {
            case 0: // TOP
                for (int i = 0; i < spawnCorridor2Amount; i++)
                {
                    cells[spawnCellID - (50 * (i + 1))].GetComponent<CellScript>().roomTypeName = "Vertical Corridor";
                    Instantiate(verticalCorridor, cells[spawnCellID - (50 * (i + 1))].GetComponent<CellScript>().position, Quaternion.identity);
                    if (i == spawnCorridor2Amount - 1)
                    {
                        cells[spawnCellID - (50 * (i + 1))].GetComponent<CellScript>().canCurvedCorridorBePlaced = true;
                        placeMultiDirectionalCurvedCorridor(spawnCellID - (50 * (i + 2)), 2);
                        placedCurvedCorridorPathways(spawnCellID - (50 * (i + 2)), 2);
                    }
                }
                break;
            case 1: // RIGHT
                for (int i = 0; i < spawnCorridor2Amount; i++)
                {
                    cells[spawnCellID + (i + 1)].GetComponent<CellScript>().roomTypeName = "Horizontal Corridor";
                    Instantiate(horizontalCorridor, cells[spawnCellID + (i + 1)].GetComponent<CellScript>().position, Quaternion.identity);
                    if (i == spawnCorridor2Amount - 1)
                    {
                        cells[spawnCellID + (i + 1)].GetComponent<CellScript>().canCurvedCorridorBePlaced = true;
                        placeMultiDirectionalCurvedCorridor(spawnCellID + (i + 2), 3);
                        placedCurvedCorridorPathways(spawnCellID + (i + 2), 3);
                    }
                }
                break;
            case 2: // Down
                for (int i = 0; i < spawnCorridor2Amount; i++)
                {
                    cells[spawnCellID + (50 * (i + 1))].GetComponent<CellScript>().roomTypeName = "Vertical Corridor";
                    Instantiate(verticalCorridor, cells[spawnCellID + (50 * (i + 1))].GetComponent<CellScript>().position, Quaternion.identity);
                    if (i == spawnCorridor2Amount - 1)
                    {
                        cells[spawnCellID + (50 * (i + 1))].GetComponent<CellScript>().canCurvedCorridorBePlaced = true;
                        placeMultiDirectionalCurvedCorridor(spawnCellID + (50 * (i + 2)), 0);
                        placedCurvedCorridorPathways(spawnCellID + (50 * (i + 2)), 0);
                    }
                }
                break;
            case 3: // Left
                for (int i = 0; i < spawnCorridor2Amount; i++)
                {
                    cells[spawnCellID - (i + 1)].GetComponent<CellScript>().roomTypeName = "Horizontal Corridor";
                    Instantiate(horizontalCorridor, cells[spawnCellID - (i + 1)].GetComponent<CellScript>().position, Quaternion.identity);
                    if (i == spawnCorridor2Amount - 1)
                    {
                        cells[spawnCellID - (i + 1)].GetComponent<CellScript>().canCurvedCorridorBePlaced = true;
                        placeMultiDirectionalCurvedCorridor(spawnCellID - (i + 2), 1);
                        placedCurvedCorridorPathways(spawnCellID - (i + 2), 1);
                    }
                }
                break;
            default:
                print("INVALID SWITCH STATEMENT");
                break;
        }
    }

    void placeMultiDirectionalCurvedCorridor(int t_id, int t_previousRoomDirection)
    {
        switch (t_previousRoomDirection)
        {
            case 0: // TOP Previous room
                Instantiate(threeTLRCorridor, cells[t_id].GetComponent<CellScript>().position, Quaternion.identity);
                assignRoomType(t_id, "Curved Corridor");
                break;
            case 1: // Right Previous room
                Instantiate(threeRTBCorridor, cells[t_id].GetComponent<CellScript>().position, Quaternion.identity);
                assignRoomType(t_id, "Curved Corridor");
                break;
            case 2: // Bottom Previous room
                Instantiate(threeBLRCorridor, cells[t_id].GetComponent<CellScript>().position, Quaternion.identity);
                assignRoomType(t_id, "Curved Corridor");
                break;
            case 3: // Left Previous room
                Instantiate(threeLTBCorridor, cells[t_id].GetComponent<CellScript>().position, Quaternion.identity);
                assignRoomType(t_id, "Curved Corridor");
                break;
        }
    }

    void placedCurvedCorridorPathways(int t_id, int t_previousRoomDirection)
    {
        int randHallAmount = Random.Range(1, 7);
        int randHallAmount2 = Random.Range(1, 7);

        switch (t_previousRoomDirection)
        {
            case 0:
                spawnCorridorsLeft(t_id, randHallAmount);
                spawnCorridorsRight(t_id, randHallAmount2);
                break;

            case 1:
                spawnCorridorsDown(t_id, randHallAmount);
                spawnCorridorsUp(t_id, randHallAmount2);
                break;

            case 2:
                spawnCorridorsLeft(t_id, randHallAmount);
                spawnCorridorsRight(t_id, randHallAmount2);
                break;

            case 3:
                spawnCorridorsUp(t_id, randHallAmount);
                spawnCorridorsDown(t_id, randHallAmount2);
                break;

        }
    }

    void assignRoomType(int t_id, string t_roomType)
    {
        cells[t_id].GetComponent<CellScript>().roomTypeName = t_roomType;
    }

    bool checkIsRoomPlaceValid(int t_id, int t_previousRoomDirection)
    {
        switch (t_previousRoomDirection)
        {
            case 0: // Top Previous Room
                    //for (int i = 0; i < 3; i++)
                    //{
                    //    if (cells[t_id -51 + i].GetComponent<CellScript>().roomTypeName != "EMPTY")
                    //    {
                    //        Debug.Log("NOT VALID " + 1 + " id: " + t_id);
                    //        return false;
                    //    }
                    //    else if (cells[t_id - 101 + i].GetComponent<CellScript>().roomTypeName != "EMPTY")
                    //    {
                    //        Debug.Log("NOT VALID " + 1 + " id: " + t_id );
                    //        return false;
                    //    }
                    //}


                //for (int i = 0; i < 3; i++)
                //{
                //    if (cells[t_id + 49 + i].GetComponent<CellScript>().roomTypeName != "EMPTY")
                //    {
                //        return false;
                //    }
                //    else if (cells[t_id + 49 + 50 + i].GetComponent<CellScript>().roomTypeName != "EMPTY")
                //    {
                //        return false;
                //    }
                //}
                //break;


                for (int i = 0; i < 3; i++)
                {
                    if (cells[t_id - 51 + i].GetComponent<CellScript>().roomTypeName != "EMPTY")
                    {
                        //Debug.Log("NOT VALID " + 1 + " id: " + t_id);
                        return false;
                    }
                    else if (cells[t_id - 101 + i].GetComponent<CellScript>().roomTypeName != "EMPTY")
                    {
                        //Debug.Log("NOT VALID " + 1 + " id: " + t_id);
                        return false;
                    }
                }
                break;

            case 1: // Right Previous Room
                    //for (int i = 0; i < 3; i++)
                    //{
                    //    if (cells[t_id - 51 + (50 * i)].GetComponent<CellScript>().roomTypeName != "EMPTY")
                    //    {
                    //        return false;
                    //    }
                    //    else if (cells[t_id - 52 + (50 * i)].GetComponent<CellScript>().roomTypeName != "EMPTY")
                    //    {
                    //        return false;
                    //    }
                    //}


                for (int i = 0; i < 3; i++)
                {
                    if (cells[t_id - 49 + (50 * i)].GetComponent<CellScript>().roomTypeName != "EMPTY")
                    {
                        Debug.Log("Room is not valid Left as " + t_id);
                        return false;
                    }
                    else if (cells[t_id - 48 + (50 * i)].GetComponent<CellScript>().roomTypeName != "EMPTY")
                    {
                        Debug.Log("Room is not valid Left as " + t_id);
                        return false;
                    }
                }
                break;

            case 2: // Bottom Previous Room
                    //for (int i = 0; i < 3; i++)
                    //{
                    //    if (cells[t_id + 49 + i].GetComponent<CellScript>().roomTypeName != "EMPTY")
                    //    {
                    //        return false;
                    //    }
                    //    else if (cells[t_id + 49 + 50 + i].GetComponent<CellScript>().roomTypeName != "EMPTY")
                    //    {
                    //        return false;
                    //    }
                    //}


                //for (int i = 0; i < 3; i++)
                //{
                //    if (cells[t_id - 51 + i].GetComponent<CellScript>().roomTypeName != "EMPTY")
                //    {
                //        Debug.Log("NOT VALID " + 1 + " id: " + t_id);
                //        return false;
                //    }
                //    else if (cells[t_id - 101 + i].GetComponent<CellScript>().roomTypeName != "EMPTY")
                //    {
                //        Debug.Log("NOT VALID " + 1 + " id: " + t_id);
                //        return false;
                //    }
                //}
                //break;



                for (int i = 0; i < 3; i++)
                {
                    if (cells[t_id + 49 + 50 + i].GetComponent<CellScript>().roomTypeName != "EMPTY")
                    {
                        Debug.Log("Cringe");
                        return false;
                    }
                    else if (cells[t_id + 49 + 100 + i].GetComponent<CellScript>().roomTypeName != "EMPTY")
                    {
                        Debug.Log("Cringe2");
                        return false;
                    }
                    else if (cells[t_id + 49 + i].GetComponent<CellScript>().roomTypeName != "EMPTY")
                    {
                        Debug.Log("Cringe2");
                        return false;
                    }
                }
                break;

            case 3: // Left Previous Room
                    //for (int i = 0; i < 3; i++)
                    //{
                    //    if (cells[t_id - 49 + (50 * i)].GetComponent<CellScript>().roomTypeName != "EMPTY")
                    //    {
                    //        Debug.Log("Room is not valid Left as " + t_id);
                    //        return false;
                    //    }
                    //    else if (cells[t_id - 48 + (50 * i)].GetComponent<CellScript>().roomTypeName != "EMPTY")
                    //    {
                    //        Debug.Log("Room is not valid Left as " + t_id);
                    //        return false;
                    //    }
                    //}


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
                }
                break;
        }
        return true;
    }

    void placeCorridorEnd(int t_id, int t_previousRoomDirection)
    {
        switch (t_previousRoomDirection)
        {
            case 0:
                Instantiate(topEntranceCorridorDeadEnd, cells[t_id].GetComponent<CellScript>().position, Quaternion.identity);
                
                break;

            case 1:
                Instantiate(rightEntranceCorridorDeadEnd, cells[t_id].GetComponent<CellScript>().position, Quaternion.identity);
                break;

            case 2:
                Instantiate(bottomEntranceCorridorDeadEnd, cells[t_id].GetComponent<CellScript>().position, Quaternion.identity);
                break;

            case 3:              
                Instantiate(leftEntranceCorridorDeadEnd, cells[t_id].GetComponent<CellScript>().position, Quaternion.identity);
                break;
        }
    }

    void placeCurvedCorridor(int t_id, int t_previousRoomDirection)
    {
        int randNum = Random.Range(1, 3);
        int randHallAmount = Random.Range(1, 7);
        switch (t_previousRoomDirection)
        {
            case 0:
                if (randNum == 1)
                {
                    Instantiate(twoBLCorridor, cells[t_id].GetComponent<CellScript>().position, Quaternion.identity);
                    spawnCorridorsLeft(t_id, randHallAmount);
                }
                else
                {
                    Instantiate(twoBRCorridor, cells[t_id].GetComponent<CellScript>().position, Quaternion.identity);
                    spawnCorridorsRight(t_id, randHallAmount);
                }
                break;

            case 1:
                if (randNum == 1)
                {
                    Instantiate(twoTRCorridor, cells[t_id].GetComponent<CellScript>().position, Quaternion.identity);
                    spawnCorridorsUp(t_id, randHallAmount);
                }
                else
                {
                    Instantiate(twoBRCorridor, cells[t_id].GetComponent<CellScript>().position, Quaternion.identity);
                    spawnCorridorsDown(t_id, randHallAmount);
                }
                break;

            case 2:

                if (randNum == 1)
                {
                    Instantiate(twoTRCorridor, cells[t_id].GetComponent<CellScript>().position, Quaternion.identity);
                    spawnCorridorsRight(t_id, randHallAmount);
                }
                else
                {
                    Instantiate(twoTLCorridor, cells[t_id].GetComponent<CellScript>().position, Quaternion.identity);
                    spawnCorridorsLeft(t_id, randHallAmount);
                }
                break;

            case 3:
                if (randNum == 1)
                {
                    Instantiate(twoTLCorridor, cells[t_id].GetComponent<CellScript>().position, Quaternion.identity);
                    spawnCorridorsUp(t_id, randHallAmount);
                }
                else
                {
                    Instantiate(twoBLCorridor, cells[t_id].GetComponent<CellScript>().position, Quaternion.identity);
                    spawnCorridorsDown(t_id, randHallAmount);
                }
                break;
        }
    }

    void spawnCorridorsUp(int t_id, int t_numberOfRooms)
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
                    if (checkIsRoomPlaceValid(t_id - ((t_numberOfRooms + 3) * 50), 0) == false)
                    {
                        placeCorridorEnd(t_id - ((t_numberOfRooms + 1) * 50), 2);
                        assignRoomType(t_id - ((t_numberOfRooms + 1) * 50), "End Corridor");
                    }
                    else
                    {
                        placeCurvedCorridor(t_id - ((t_numberOfRooms + 1) * 50), 0);
                        assignRoomType(t_id - ((t_numberOfRooms + 1) * 50), "Curved Corridor");
                    }
                }
            }
        }
    }

    void spawnCorridorsDown(int t_id, int t_numberOfRooms)
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
                        placeCurvedCorridor(t_id + ((t_numberOfRooms + 1) * 50), 2);
                        assignRoomType(t_id + ((t_numberOfRooms + 1) * 50), "Curved Corridor");
                    }
                }
            }
        }
    }

    void spawnCorridorsRight(int t_id, int t_numberOfRooms)
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
                        placeCorridorEnd(t_id + t_numberOfRooms, 3);
                        assignRoomType(t_id + (t_numberOfRooms + 1), "End Corridor");
                    }
                    else
                    {
                        placeCurvedCorridor(t_id + t_numberOfRooms + 1, 3);
                        assignRoomType(t_id + t_numberOfRooms + 1, "Curved Corridor");
                    }
                }
            }
        }
    }

    void spawnCorridorsLeft(int t_id, int t_numberOfRooms)
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
                        placeCurvedCorridor(t_id - t_numberOfRooms - 1, 1);
                        assignRoomType(t_id - (t_numberOfRooms + 1), "Curved Corridor");
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
}
