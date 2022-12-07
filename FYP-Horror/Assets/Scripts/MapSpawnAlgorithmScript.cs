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
    // Start is called before the first frame update
    void Start()
    {
        spawnRoom = GameObject.FindGameObjectWithTag("SpawnPoint");
        spawnDirection1 =  Random.Range(0, 4);
        spawnDirection1 = Random.Range(0, 4);
        Debug.Log(spawnDirection1);
        //spawnCorridor1Amount = Random.Range(3, 7);
        //spawnCorridor2Amount = Random.Range(3, 7);

        spawnCorridor1Amount = Random.Range(2, 2);
        spawnCorridor2Amount = Random.Range(2, 2);

        while (spawnDirection2 == spawnDirection1)
        {
            spawnDirection2 = Random.Range(0, 4);
        }
        SetupGrid();
        SpawnStartCorridors();
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
                        placeCurvedCorridor(spawnCellID - (50 * (i + 2)), 2);
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
                        placeCurvedCorridor(spawnCellID + (i + 2), 3);
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
                        placeCurvedCorridor(spawnCellID + (50 * (i + 2)), 0);
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
                        placeCurvedCorridor(spawnCellID - (i + 2), 1);
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
                        placeCurvedCorridor(spawnCellID - (50 * (i + 2)), 2);
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
                        placeCurvedCorridor(spawnCellID + (i + 2), 3);
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
                        placeCurvedCorridor(spawnCellID + (50 * (i + 2)), 0);
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
                        placeCurvedCorridor(spawnCellID - (i + 2), 1);
                        placedCurvedCorridorPathways(spawnCellID - (i + 2), 1);
                    }
                }
                break;
            default:
                print("INVALID SWITCH STATEMENT");
                break;
        }
    }

    void placeCurvedCorridor(int t_id, int t_previousRoomDirection)
    {
        switch (t_previousRoomDirection)
        {
            case 0: // TOP Previous room
                Instantiate(threeTLRCorridor, cells[t_id].GetComponent<CellScript>().position, Quaternion.identity);
                break;
            case 1: // Right Previous room
                Instantiate(threeRTBCorridor, cells[t_id].GetComponent<CellScript>().position, Quaternion.identity);
                break;
            case 2: // Bottom Previous room
                Instantiate(threeBLRCorridor, cells[t_id].GetComponent<CellScript>().position, Quaternion.identity);
                break;
            case 3: // Left Previous room
                Instantiate(threeLTBCorridor, cells[t_id].GetComponent<CellScript>().position, Quaternion.identity);
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
                for (int i = 0; i < randHallAmount; i++)
                {
                    if (checkIsRoomPlaceValid(t_id, t_previousRoomDirection) == false)
                    {
                        Instantiate(spawnRoom, cells[t_id - (i + 1)].GetComponent<CellScript>().position, Quaternion.identity);
                    }
                    else
                    {
                        Instantiate(horizontalCorridor, cells[t_id - (i + 1)].GetComponent<CellScript>().position, Quaternion.identity);
                    }
                }
                for (int i = 0; i < randHallAmount2; i++)
                {
                    if (checkIsRoomPlaceValid(t_id, t_previousRoomDirection) == false)
                    {
                        Instantiate(spawnRoom, cells[t_id + (i + 1)].GetComponent<CellScript>().position, Quaternion.identity);
                    }
                    else
                    {
                        Instantiate(horizontalCorridor, cells[t_id + (i + 1)].GetComponent<CellScript>().position, Quaternion.identity);
                    }
                }
                break;
            case 1:
                for (int i = 0; i < randHallAmount; i++)
                {
                    if (checkIsRoomPlaceValid(t_id, t_previousRoomDirection) == false)
                    {
                        Instantiate(spawnRoom, cells[t_id + ((i + 1) * 50)].GetComponent<CellScript>().position, Quaternion.identity);
                    }
                    else
                    {
                        Instantiate(verticalCorridor, cells[t_id - ((i + 1) * 50)].GetComponent<CellScript>().position, Quaternion.identity);
                    }
                }
                for (int i = 0; i < randHallAmount2; i++)
                {
                    if (checkIsRoomPlaceValid(t_id, t_previousRoomDirection) == false)
                    {
                        Instantiate(spawnRoom, cells[t_id + ((i + 1) * 50)].GetComponent<CellScript>().position, Quaternion.identity);
                    }
                    else
                    {
                        Instantiate(verticalCorridor, cells[t_id + ((i + 1) * 50)].GetComponent<CellScript>().position, Quaternion.identity);
                    }
                }
                break;
            case 2:
                for (int i = 0; i < randHallAmount; i++)
                {
                    if (checkIsRoomPlaceValid(t_id, t_previousRoomDirection) == false)
                    {
                        Instantiate(spawnRoom, cells[t_id - (i + 1)].GetComponent<CellScript>().position, Quaternion.identity);
                    }
                    else
                    {
                        Instantiate(horizontalCorridor, cells[t_id - (i + 1)].GetComponent<CellScript>().position, Quaternion.identity);
                    }
                }
                for (int i = 0; i < randHallAmount2; i++)
                {
                    if (checkIsRoomPlaceValid(t_id, t_previousRoomDirection) == false)
                    {
                        Instantiate(spawnRoom, cells[t_id + (i + 1)].GetComponent<CellScript>().position, Quaternion.identity);
                    }
                    else
                    {
                        Instantiate(horizontalCorridor, cells[t_id + (i + 1)].GetComponent<CellScript>().position, Quaternion.identity);
                    }
                }
                break;
            case 3:
                for (int i = 0; i < randHallAmount; i++)
                {
                    if (checkIsRoomPlaceValid(t_id, t_previousRoomDirection) == false)
                    {
                        Instantiate(spawnRoom, cells[t_id - ((i + 1) * 50)].GetComponent<CellScript>().position, Quaternion.identity);
                    }
                    else
                    {
                        Instantiate(verticalCorridor, cells[t_id - ((i + 1) * 50)].GetComponent<CellScript>().position, Quaternion.identity);
                    }
                }
                for (int i = 0; i < randHallAmount2; i++)
                {
                    if (checkIsRoomPlaceValid(t_id, t_previousRoomDirection) == false)
                    {
                        Instantiate(spawnRoom, cells[t_id + ((i + 1) * 50)].GetComponent<CellScript>().position, Quaternion.identity);
                    }
                    else
                    {
                        Instantiate(verticalCorridor, cells[t_id + ((i + 1) * 50)].GetComponent<CellScript>().position, Quaternion.identity);
                    }
                }
                break;

        }
    }

    void assignRoomType()
    {

    }
    bool checkIsRoomPlaceValid(int t_id, int t_previousRoomDirection)
    {
        switch (t_previousRoomDirection)
        {
            case 0: // Top Previous Room
                for (int i = 0; i < 3; i++)
                {
                    if (cells[t_id + 50 - (i + 1)].GetComponent<CellScript>().roomTypeName != "EMPTY")
                    {
                        Debug.Log("UH OH Retard");
                        return false;
                    }
                    else if (cells[t_id + 100 - (i + 1)].GetComponent<CellScript>().roomTypeName != "EMPTY")
                    {
                        Debug.Log("UH OH Retard");
                        return false;
                    }
                }
                break;

            case 1: // Right Previous Room
                for (int i = 0; i < 3; i++)
                {
                    if (cells[t_id - 1 + (50 * (i - 1))].GetComponent<CellScript>().roomTypeName != "EMPTY")
                    {
                        Debug.Log("UH OH Retard");
                        return false;
                    }
                    else if (cells[t_id - 1 + (100 * (i - 1))].GetComponent<CellScript>().roomTypeName != "EMPTY")
                    {
                        Debug.Log("UH OH Retard");
                        return false;
                    }
                }
                break;

            case 2: // Bottom Previous Room
                for (int i = 0; i < 3; i++)
                {
                    if (cells[t_id + 50 - (i + 1)].GetComponent<CellScript>().roomTypeName != "EMPTY")
                    {
                        Debug.Log("UH OH Retard");
                        return false;
                    }
                    else if (cells[t_id + 100 - (i + 1)].GetComponent<CellScript>().roomTypeName != "EMPTY")
                    {
                        Debug.Log("UH OH Retard");
                        return false;
                    }
                }
                break;

            case 3: // Left Previous Room
                for (int i = 0; i < 3; i++)
                {
                    if (cells[t_id + 50 - (i + 1)].GetComponent<CellScript>().roomTypeName != "EMPTY")
                    {
                        Debug.Log("UH OH Retard");
                        return false;
                    }
                    else if (cells[t_id + 100 - (i + 1)].GetComponent<CellScript>().roomTypeName != "EMPTY")
                    {
                        Debug.Log("UH OH Retard");
                        return false;
                    }
                }
                break;
        }
        return true;
    }

}
