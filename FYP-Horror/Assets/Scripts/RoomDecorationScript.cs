using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomDecorationScript : MonoBehaviour
{
    private GameManagerScript gameManagerScript;
    public bool rightWall;
    public bool leftWall;
    public bool topWall;
    public bool bottomWall;

    const int MAX_ROWS = 10;
    const int MAX_COLUMNS = 10;

    public int wardrobeCounter = 0;
    private int tile1x1Counter = 0;
    public int wardrobeRotationOffset = 0;
    private float wardrobePositionOffset = 0.5f;

    private List<int> nonPlaceableTiles = new List<int>();
    private List<int> wallTiles = new List<int>();
    private List<int> filledTiles = new List<int>();

    public List<int> oneTileWallObstacles = new List<int>();
    

    private GameObject roomTiles;


    // Start is called before the first frame update
    void Start()
    {
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManagerScript>();
        roomTiles = gameObject.transform.Find("RoomTiles").gameObject;

        //tileObstacle1x1 = GameObject.FindWithTag("1x1");
        //tileObstacle2x1 = GameObject.FindWithTag("2x1");
        //tileObstacle2x2 = GameObject.FindWithTag("2x2");

        findWallTiles();
        setCenterTiles();
        setCornersAsNonPlaceable();

        if (rightWall == true)
        {
            spawnWallObstacles("Right");
        }
        if (leftWall == true)
        {
            spawnWallObstacles("Left");
        }
        if (topWall == true)
        {
            spawnWallObstacles("Top");
        }
        if (bottomWall == true)
        {
            spawnWallObstacles("Bottom");
        }
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
            //tempRoomTile.GetComponent<Renderer>().material.color = Color.blue;
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
        //tempRoomTile.GetComponent<Renderer>().material.color = Color.red;
    }

    private void setCenterTiles()
    {
        for (int i = 1; i < (MAX_ROWS * MAX_COLUMNS) + 1; i++)
        {
            if (nonPlaceableTiles.Contains(i) == false && wallTiles.Contains(i) == false)
            {
                GameObject tempRoomTile = roomTiles.gameObject.transform.Find("RoomTile (" + i + ")").gameObject;
                //tempRoomTile.GetComponent<Renderer>().material.color = Color.green;
            }
        }
    }

    private void setCornersAsNonPlaceable()
    {
        setTileAsNonPlaceable(1);
        setTileAsNonPlaceable(10);
        setTileAsNonPlaceable(91);
        setTileAsNonPlaceable(100);
    }

    private void spawnWallObstacles(string _wallDirection)
    {
        
        switch (_wallDirection)
        {
            
            case "Left":
                wardrobeRotationOffset = 90;
                for (int i = 1; i < MAX_ROWS + 1; i++)
                {
                    int currentTile = 1 + ((i - 1) * 10);
                    if (nonPlaceableTiles.Contains(currentTile) == false)
                    {
                        if (randomWardrobeChance() == true && wardrobeChecks(currentTile, _wallDirection) == true && wardrobeCounter < 1)
                        {
                            Vector3 position = roomTiles.transform.Find(returnRoomTileName(currentTile)).transform.position;
                            position = new Vector3(position.x, position.y, position.z - wardrobePositionOffset);
                            //GameObject wardrobe = Instantiate(gameManagerScript.tileObstacle2x1, position, Quaternion.Euler(0, wardrobeRotationOffset, 0));
                            GameObject wardrobe2 = Instantiate(gameManagerScript.wardrobe, position, Quaternion.Euler(0, wardrobeRotationOffset, 0));
                            filledTiles.Add(currentTile);
                            filledTiles.Add(currentTile - 10);
                            wardrobeCounter++;
                        }

                        if (random2x1TileChance() == true && tile2x1Checks(currentTile, _wallDirection) == true /*&& wardrobeCounter < 1*/)
                        {
                            Vector3 position = roomTiles.transform.Find(returnRoomTileName(currentTile)).transform.position;
                            //position = new Vector3(position.x + wardrobePositionOffset, position.y, position.z);
                            GameObject wardrobe = Instantiate(gameManagerScript.tileObstacle2x1, position, Quaternion.Euler(0, wardrobeRotationOffset, 0));
                            GameObject wardrobe2 = Instantiate(gameManagerScript.deadBody, position, Quaternion.Euler(0, wardrobeRotationOffset, 0));
                            filledTiles.Add(currentTile);
                            filledTiles.Add(currentTile + 1);
                        }

                        if (random1TileChance() == true && checkIfObstacleAdjacent(currentTile) == false && filledTiles.Contains(currentTile) == false && tile1x1Counter < 3)
                        {
                            Vector3 position = roomTiles.transform.Find(returnRoomTileName(currentTile)).transform.position;
                            //position = new Vector3(position.x, position.y, position.z - wardrobePositionOffset);
                            GameObject tile1x1 = Instantiate(gameManagerScript.tileObstacle1x1, position, Quaternion.Euler(0, wardrobeRotationOffset, 0));
                            //GameObject wardrobe2 = Instantiate(gameManagerScript.deadBody, position, Quaternion.Euler(0, wardrobeRotationOffset, 0));
                            filledTiles.Add(currentTile);
                            filledTiles.Add(currentTile - 10);

                            tile1x1Counter++;
                        }
                    }
                }
                    break;

            case "Right":
                wardrobeRotationOffset = -90;
                for (int i = 1; i < MAX_ROWS + 1; i++)
                {
                    int currentTile = i * 10;
                    if (nonPlaceableTiles.Contains(currentTile) == false)
                    {
                        if (randomWardrobeChance() == true && wardrobeChecks(currentTile, _wallDirection) == true && wardrobeCounter < 1)
                        {
                            Vector3 position = roomTiles.transform.Find(returnRoomTileName(currentTile)).transform.position;
                            position = new Vector3(position.x, position.y, position.z - wardrobePositionOffset);
                            //GameObject wardrobe = Instantiate(gameManagerScript.tileObstacle2x1, position, Quaternion.Euler(0, wardrobeRotationOffset, 0));
                            GameObject wardrobe2 = Instantiate(gameManagerScript.wardrobe, position, Quaternion.Euler(0, wardrobeRotationOffset, 0));
                            filledTiles.Add(currentTile);
                            filledTiles.Add(currentTile - 10);
                            wardrobeCounter++;
                        }

                        if (random2x1TileChance() == true && tile2x1Checks(currentTile, _wallDirection) == true /*&& wardrobeCounter < 1*/)
                        {
                            Vector3 position = roomTiles.transform.Find(returnRoomTileName(currentTile)).transform.position;
                            //position = new Vector3(position.x + wardrobePositionOffset, position.y, position.z);
                            GameObject wardrobe = Instantiate(gameManagerScript.tileObstacle2x1, position, Quaternion.Euler(0, wardrobeRotationOffset, 0));
                            GameObject wardrobe2 = Instantiate(gameManagerScript.deadBody, position, Quaternion.Euler(0, wardrobeRotationOffset, 0));
                            filledTiles.Add(currentTile);
                            filledTiles.Add(currentTile - 1);
                        }

                        if (random1TileChance() == true && checkIfObstacleAdjacent(currentTile) == false && filledTiles.Contains(currentTile) == false && tile1x1Counter < 3)
                        {
                            Vector3 position = roomTiles.transform.Find(returnRoomTileName(currentTile)).transform.position;
                            //position = new Vector3(position.x, position.y, position.z - wardrobePositionOffset);
                            GameObject tile1x1 = Instantiate(gameManagerScript.tileObstacle1x1, position, Quaternion.Euler(0, wardrobeRotationOffset, 0));
                            //GameObject wardrobe2 = Instantiate(gameManagerScript.deadBody, position, Quaternion.Euler(0, wardrobeRotationOffset, 0));
                            filledTiles.Add(currentTile);
                            filledTiles.Add(currentTile - 10);
                        }
                    }
                }
                break;

            case "Top":
                wardrobeRotationOffset = 180;
                for (int i = 1; i < MAX_ROWS + 1; i++)
                {
                    int currentTile = i;
                    if (nonPlaceableTiles.Contains(currentTile) == false)
                    {
                        if (randomWardrobeChance() == true && wardrobeChecks(currentTile, _wallDirection) == true && wardrobeCounter < 1)
                        {
                            Vector3 position = roomTiles.transform.Find(returnRoomTileName(currentTile)).transform.position;
                            position = new Vector3(position.x + wardrobePositionOffset, position.y, position.z);
                            //GameObject wardrobe = Instantiate(gameManagerScript.tileObstacle2x1, position, Quaternion.Euler(0, wardrobeRotationOffset, 0));
                            GameObject wardrobe2 = Instantiate(gameManagerScript.wardrobe, position, Quaternion.Euler(0, wardrobeRotationOffset, 0));
                            filledTiles.Add(currentTile);
                            filledTiles.Add(currentTile + 1);
                            wardrobeCounter++;
                        }

                        if (random2x1TileChance() == true && tile2x1Checks(currentTile, _wallDirection) == true /*&& wardrobeCounter < 1*/)
                        {
                            Vector3 position = roomTiles.transform.Find(returnRoomTileName(currentTile)).transform.position;
                            //position = new Vector3(position.x + wardrobePositionOffset, position.y, position.z);
                            GameObject wardrobe = Instantiate(gameManagerScript.tileObstacle2x1, position, Quaternion.Euler(0, wardrobeRotationOffset, 0));
                            GameObject wardrobe2 = Instantiate(gameManagerScript.deadBody, position, Quaternion.Euler(0, wardrobeRotationOffset, 0));
                            filledTiles.Add(currentTile);
                            filledTiles.Add(currentTile + 10);
                        }

                        if (random1TileChance() == true && checkIfObstacleAdjacent(currentTile) == false && filledTiles.Contains(currentTile) == false && tile1x1Counter < 3)
                        {
                            Vector3 position = roomTiles.transform.Find(returnRoomTileName(currentTile)).transform.position;
                            //position = new Vector3(position.x, position.y, position.z - wardrobePositionOffset);
                            GameObject tile1x1 = Instantiate(gameManagerScript.tileObstacle1x1, position, Quaternion.Euler(0, wardrobeRotationOffset, 0));
                            //GameObject wardrobe2 = Instantiate(gameManagerScript.deadBody, position, Quaternion.Euler(0, wardrobeRotationOffset, 0));
                            filledTiles.Add(currentTile);
                            filledTiles.Add(currentTile - 10);
                        }
                    }
                }
                break;

            case "Bottom":
                wardrobeRotationOffset = 0;
                for (int i = 1; i < MAX_ROWS + 1; i++)
                {
                    int currentTile = i + 90;
                    if (nonPlaceableTiles.Contains(currentTile) == false)
                    {
                        if (randomWardrobeChance() == true && wardrobeChecks(currentTile, _wallDirection) == true && wardrobeCounter < 1)
                        {
                            Vector3 position = roomTiles.transform.Find(returnRoomTileName(currentTile)).transform.position;
                            position = new Vector3(position.x + wardrobePositionOffset, position.y, position.z);
                           // GameObject wardrobe = Instantiate(gameManagerScript.tileObstacle2x1, position, Quaternion.Euler(0,wardrobeRotationOffset, 0));
                            GameObject wardrobe2 = Instantiate(gameManagerScript.wardrobe, position, Quaternion.Euler(0, wardrobeRotationOffset, 0));
                            filledTiles.Add(currentTile);
                            filledTiles.Add(currentTile + 1);
                            wardrobeCounter++;
                        }

                        if (random2x1TileChance() == true && tile2x1Checks(currentTile, _wallDirection) == true /*&& wardrobeCounter < 1*/)
                        {
                            Vector3 position = roomTiles.transform.Find(returnRoomTileName(currentTile)).transform.position;
                            //position = new Vector3(position.x + wardrobePositionOffset, position.y, position.z);
                            GameObject wardrobe = Instantiate(gameManagerScript.tileObstacle2x1, position, Quaternion.Euler(0, wardrobeRotationOffset, 0));
                            GameObject wardrobe2 = Instantiate(gameManagerScript.deadBody, position, Quaternion.Euler(0, wardrobeRotationOffset, 0));
                            filledTiles.Add(currentTile);
                            filledTiles.Add(currentTile - 10);
                        }

                        if (random1TileChance() == true && checkIfObstacleAdjacent(currentTile) == false && filledTiles.Contains(currentTile) == false && tile1x1Counter < 3)
                        {
                            Vector3 position = roomTiles.transform.Find(returnRoomTileName(currentTile)).transform.position;
                            //position = new Vector3(position.x, position.y, position.z - wardrobePositionOffset);
                            GameObject tile1x1 = Instantiate(gameManagerScript.tileObstacle1x1, position, Quaternion.Euler(0, wardrobeRotationOffset, 0));
                            
                            filledTiles.Add(currentTile);

                            ///
                            /// probably wrong underneath
                            ///
                            filledTiles.Add(currentTile - 10);
                        }
                    }
                }
                break;
        }
    }

    private bool randomWardrobeChance()
    {
        int randNum = Random.Range(0, gameManagerScript.maxWardrobeSpawnChance);
        bool willBePlaced = false;

        if (randNum == 1)
        {
            willBePlaced = true;
        }

        return willBePlaced;
    }

    private bool random1TileChance()
    {
        int randNum = Random.Range(0, gameManagerScript.max1TileWallObstacleSpawnChance);
        bool willBePlaced = false;

        if (randNum == 1)
        {
            willBePlaced = true;
        }

        return willBePlaced;
    }

    private bool random2x1TileChance()
    {
        int randNum = Random.Range(0, gameManagerScript.max2x1TileWallObstacleSpawnChance);
        bool willBePlaced = false;

        if (randNum == 1)
        {
            willBePlaced = true;
        }

        return willBePlaced;
    }

    private string returnRoomTileName(int _id)
    {
        return "RoomTile (" + _id + ")";
    }

    private bool checkIfObstacleAdjacent(int _id)
    {
        int moduloAnswer = _id % 10;

        if (_id <= 10)
        {
            if (checkIfObstacleOnBottom(_id) == true || checkIfObstacleOnLeft(_id) == true || checkIfObstacleOnRight(_id) == true)
            {
                return true;
            }
            else
            {
                return false;
            }          
        }
        else if (_id >= 90)
        {
            if (checkIfObstacleOnTop(_id) == true || checkIfObstacleOnLeft(_id) == true || checkIfObstacleOnRight(_id) == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        switch (moduloAnswer)
        {
            case 0:
                if (checkIfObstacleOnTop(_id) == true || checkIfObstacleOnLeft(_id) == true || checkIfObstacleOnBottom(_id) == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 1:
                if (checkIfObstacleOnTop(_id) == true || checkIfObstacleOnRight(_id) == true || checkIfObstacleOnBottom(_id) == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
        }

        return false;
    }

    private bool checkIfObstacleOnRight(int _id)
    {
        if (filledTiles.Contains(_id + 1) == true)
        {
            return true;
        }
        return false;
    }

    private bool checkIfObstacleOnLeft(int _id)
    {
        if (filledTiles.Contains(_id - 1) == true)
        {
            return true;
        }
        return false;
    }

    private bool checkIfObstacleOnTop(int _id)
    {
        if (filledTiles.Contains(_id - 10) == true)
        {
            return true;
        }
        return false;
    }

    private bool checkIfObstacleOnBottom(int _id)
    {
        if (filledTiles.Contains(_id + 10) == true)
        {
            return true;
        }
        return false;
    }

    private bool wardrobeChecks(int _id, string _wallDirection)
    {
        int directionOffset = 0;

        switch (_wallDirection)
        {
            case "Right":
                directionOffset = -10;
                break;

            case "Left":
                directionOffset = -10;
                break;

            case "Top":
                directionOffset = 1;
                break;

            case "Bottom":
                directionOffset = 1;
                break;
        }

        if (checkIfObstacleAdjacent(_id) == false && checkIfObstacleAdjacent(_id + directionOffset) == false)
        {
            return true;
        }

        return false;
    }

    private bool tile2x1Checks(int _id, string _wallDirection)
    {
        int directionOffset = 0;

        switch (_wallDirection)
        {
            case "Right":
                directionOffset = -1;
                break;

            case "Left":
                directionOffset = +1;
                break;

            case "Top":
                directionOffset = 10;
                break;

            case "Bottom":
                directionOffset = -10;
                break;
        }

        if (checkIfObstacleAdjacent(_id) == false && checkIfObstacleAdjacent(_id + directionOffset) == false)
        {
            return true;
        }

        return false;
    }


}
