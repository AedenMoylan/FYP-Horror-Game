using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomDecorationScript : MonoBehaviour
{
    public bool rightWall;
    public bool leftWall;
    public bool topWall;
    public bool bottomWall;

    const int MAX_ROWS = 10;
    const int MAX_COLUMNS = 10;

    private List<int> nonPlaceableTiles = new List<int>();
    private List<int> wallTiles = new List<int>();

    private GameObject roomTiles;
    // Start is called before the first frame update
    void Start()
    {
        roomTiles = gameObject.transform.Find("RoomTiles").gameObject;

        findWallTiles();
        setCenterTiles();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void findWallTiles()
    {
        for (int i = 1; i < MAX_ROWS + 1; i++)
        {
            if (rightWall == true)
            {
                setTileAsWallTile(i * 10);
            }
            if (leftWall == true)
            {
                setTileAsWallTile(1 + ((i - 1) * 10));
            }
            if (topWall == true)
            {
                setTileAsWallTile(i);
            }
            if (bottomWall == true)
            {
                setTileAsWallTile(i + 90);
            }
        }
    }

    private void setTileAsWallTile(int _id)
    {
        if (nonPlaceableTiles.Contains(_id) == false)
        {
            wallTiles.Add(_id);
            GameObject tempRoomTile = roomTiles.gameObject.transform.Find("RoomTile (" + _id + ")").gameObject;
            tempRoomTile.GetComponent<Renderer>().material.color = Color.blue;
        }
    }

    public void setWallAsNonPlaceable(string _wallDirection)
    {
        switch (_wallDirection)
        {
            case "Right":
                for (int i = 1; i < MAX_ROWS + 1; i++)
                {
                    setTileAsNonPlaceable(i * 10);
                }
                    break;

            case "Left":
                for (int i = 1; i < MAX_ROWS + 1; i++)
                {
                    setTileAsNonPlaceable(1 + ((i - 1) * 10));
                }
                break;

            case "Top":
                for (int i = 1; i < MAX_ROWS + 1; i++)
                {
                    setTileAsNonPlaceable(i);
                }
                break;

            case "Bottom":
                for (int i = 1; i < MAX_ROWS + 1; i++)
                {
                    setTileAsNonPlaceable(i + 90);
                }
                break;
        }
            
    }

    private void setTileAsNonPlaceable(int _id)
    {
        roomTiles = gameObject.transform.Find("RoomTiles").gameObject;
        nonPlaceableTiles.Add(_id);
        GameObject tempRoomTile = roomTiles.gameObject.transform.Find("RoomTile (" + _id + ")").gameObject;
        tempRoomTile.GetComponent<Renderer>().material.color = Color.red;
    }

    private void setCenterTiles()
    {
        for (int i = 1; i < (MAX_ROWS * MAX_COLUMNS) + 1; i++)
        {
            if (nonPlaceableTiles.Contains(i) == false && wallTiles.Contains(i) == false)
            {
                GameObject tempRoomTile = roomTiles.gameObject.transform.Find("RoomTile (" + i + ")").gameObject;
                tempRoomTile.GetComponent<Renderer>().material.color = Color.green;
            }
        }
    }
}
