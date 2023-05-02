using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomDecorationScript : MonoBehaviour
{
    private GameManagerScript gameManagerScript;
    // used to determine which walls are active
    public bool rightWall;
    public bool leftWall;
    public bool topWall;
    public bool bottomWall;

    // max rows and columns of the tiles in the room
    const int MAX_ROWS = 10;
    const int MAX_COLUMNS = 10;

    // these are used to limit spawn amounts on some objects
    public int wardrobeCounter = 0;
    private int tile1x1Counter = 0;
    private int max2x2Obstacles = 2;
    private int obstacles2x2Counter = 0;
    // used to get wardrobe positioned correctly
    public int wardrobeRotationOffset = 0;
    private float wardrobePositionOffset = 0.5f;
    private Vector3 obstacle2x2PositionOffset = new Vector3 (0.5f, 0, -0.5f);

    // different lists for which tiles have an obstacle on it, are not placeable, are against the wall, and are in the middle
    private List<int> nonPlaceableTiles = new List<int>();
    private List<int> wallTiles = new List<int>();
    private List<int> filledTiles = new List<int>();
    private List<int> centerTiles = new List<int>();

    public List<int> oneTileWallObstacles = new List<int>();

    // the tiles in the room
    private GameObject roomTiles;


    // Start is called before the first frame update
    void Start()
    {
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManagerScript>();
        roomTiles = gameObject.transform.Find("RoomTiles").gameObject;


        // calls necessary functions for room decoration
        findWallTiles();
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
        setCenterTiles();
        spawnObstacles();
        spawnDecorations();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// when a wall is active, the tiles next to it are se3t to wall tiles
    /// </summary>
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

    /// <summary>
    /// used to set tiles as a wall tile
    /// </summary>
    /// <param name="_id"></param>
    private void setTileAsWallTile(int _id)
    {
        if (nonPlaceableTiles.Contains(_id) == false)
        {
            wallTiles.Add(_id);
            GameObject tempRoomTile = roomTiles.gameObject.transform.Find("RoomTile (" + _id + ")").gameObject;
            //tempRoomTile.GetComponent<Renderer>().material.color = Color.blue;
        }
    }

    /// <summary>
    /// used for special rooms as it does not count as a wall for placing obstacles
    /// </summary>
    /// <param name="_wallDirection"></param>
    public void setWallAsNonPlaceable(string _wallDirection)
    {
        switch (_wallDirection)
        {
            case "Right":
                for (int i = 1; i < MAX_ROWS + 1; i++)
                {
                    setTileAsNonPlaceable(i * 10);
                    setTileAsNonPlaceable(i * 10 - 1);
                }
                break;

            case "Left":
                for (int i = 1; i < MAX_ROWS + 1; i++)
                {
                    setTileAsNonPlaceable(1 + ((i - 1) * 10));
                    setTileAsNonPlaceable(1 + ((i - 1) * 10) + 1);
                }
                break;

            case "Top":
                for (int i = 1; i < MAX_ROWS + 1; i++)
                {
                    setTileAsNonPlaceable(i);
                    setTileAsNonPlaceable(i + 10);
                }
                break;

            case "Bottom":
                for (int i = 1; i < MAX_ROWS + 1; i++)
                {
                    setTileAsNonPlaceable(i + 90);
                    setTileAsNonPlaceable(i + 80);
                }
                break;
        }

    }

    /// <summary>
    /// used to set where an obstacle cannot be placed
    /// </summary>
    /// <param name="_id"></param>
    private void setTileAsNonPlaceable(int _id)
    {
        if (_id >= 0 && _id <= 100)
        {
            roomTiles = gameObject.transform.Find("RoomTiles").gameObject;
            nonPlaceableTiles.Add(_id);
            GameObject tempRoomTile = roomTiles.gameObject.transform.Find("RoomTile (" + _id + ")").gameObject;
        }


        //tempRoomTile.GetComponent<Renderer>().material.color = Color.red;
    }

    private void setMultipleTilesAsNonPlaceable(int[] _ids)
    {
        for (int i = 0; i < _ids.Length; i++)
        {
            setTileAsNonPlaceable(_ids.ElementAt(i));
        }
    }

    /// <summary>
    /// tiles that are not wall tiles or non placeable tiles are center tiles
    /// </summary>
    private void setCenterTiles()
    {
        for (int i = 1; i < (MAX_ROWS * MAX_COLUMNS) + 1; i++)
        {
            if (nonPlaceableTiles.Contains(i) == false && wallTiles.Contains(i) == false)
            {
                GameObject tempRoomTile = roomTiles.gameObject.transform.Find("RoomTile (" + i + ")").gameObject;
                //tempRoomTile.GetComponent<Renderer>().material.color = Color.green;
                centerTiles.Add(i);
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

    /// <summary>
    /// algorithm for spawning obstacles next to walls
    /// </summary>
    /// <param name="_wallDirection"></param>
    private void spawnWallObstacles(string _wallDirection)
    {      
        switch (_wallDirection)
        {            
            case "Left":
                wardrobeRotationOffset = 90;
                randomPaintingSpawn(gameObject.transform.Find("Left Wall").gameObject);
                for (int i = 1; i < MAX_ROWS + 1; i++)
                {
                    int currentTile = 1 + ((i - 1) * 10);
                    if (nonPlaceableTiles.Contains(currentTile) == false)
                    {
                        if (randomWardrobeChance() == true && wardrobeChecks(currentTile, _wallDirection) == true && wardrobeCounter < 1)
                        {
                            Vector3 position = roomTiles.transform.Find(returnRoomTileName(currentTile)).transform.position;
                            position = new Vector3(position.x, position.y, position.z - wardrobePositionOffset);
                            GameObject wardrobe = Instantiate(gameManagerScript.tileObstacle2x1, position, Quaternion.Euler(0, wardrobeRotationOffset, 0));
                            GameObject wardrobe2 = Instantiate(gameManagerScript.wardrobe, position, Quaternion.Euler(0, wardrobeRotationOffset, 0));
                            filledTiles.Add(currentTile);
                            filledTiles.Add(currentTile - 10);
                            int[] idsToMakeNonTraversable = { currentTile + 1, currentTile - 10 + 1, currentTile + 10 + 1, currentTile + 20 + 1 };
                            setMultipleTilesAsNonPlaceable(idsToMakeNonTraversable);

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
                            int[] idsToMakeNonTraversable = { currentTile - 10 + 1, currentTile + 10 + 1, currentTile - 10 + 2, currentTile + 10 + 2, currentTile + 2};
                            setMultipleTilesAsNonPlaceable(idsToMakeNonTraversable);
                        }

                        if (random1TileChance() == true && checkIfObstacleAdjacent(currentTile) == false && filledTiles.Contains(currentTile) == false && tile1x1Counter < 3)
                        {
                            Vector3 position = roomTiles.transform.Find(returnRoomTileName(currentTile)).transform.position;
                            //position = new Vector3(position.x, position.y, position.z - wardrobePositionOffset);
                            GameObject tile1x1 = Instantiate(gameManagerScript.tileObstacle1x1, position, Quaternion.Euler(0, wardrobeRotationOffset, 0));
                            GameObject tile1x1test = Instantiate(gameManagerScript.getRandom1x1WallObstacles(), position, Quaternion.Euler(0, wardrobeRotationOffset, 0));
                            //GameObject wardrobe2 = Instantiate(gameManagerScript.deadBody, position, Quaternion.Euler(0, wardrobeRotationOffset, 0));
                            filledTiles.Add(currentTile);
                            //filledTiles.Add(currentTile - 10);
                            int[] idsToMakeNonTraversable = { currentTile - 10 + 1, currentTile + 10 + 1, currentTile + 1, };
                            setMultipleTilesAsNonPlaceable(idsToMakeNonTraversable);

                            tile1x1Counter++;
                        }
                    }
                }
                    break;

            case "Right":
                wardrobeRotationOffset = -90;
                randomPaintingSpawn(gameObject.transform.Find("Right Wall").gameObject);
                for (int i = 1; i < MAX_ROWS + 1; i++)
                {
                    int currentTile = i * 10;
                    if (nonPlaceableTiles.Contains(currentTile) == false)
                    {
                        if (randomWardrobeChance() == true && wardrobeChecks(currentTile, _wallDirection) == true && wardrobeCounter < 1)
                        {
                            Vector3 position = roomTiles.transform.Find(returnRoomTileName(currentTile)).transform.position;
                            position = new Vector3(position.x, position.y, position.z - wardrobePositionOffset);
                            GameObject wardrobe = Instantiate(gameManagerScript.tileObstacle2x1, position, Quaternion.Euler(0, wardrobeRotationOffset, 0));
                            GameObject wardrobe2 = Instantiate(gameManagerScript.wardrobe, position, Quaternion.Euler(0, wardrobeRotationOffset, 0));
                            filledTiles.Add(currentTile);
                            filledTiles.Add(currentTile - 10);
                            int[] idsToMakeNonTraversable = { currentTile - 1, currentTile - 10 - 1, currentTile + 10 - 1, currentTile + 20 - 1 };
                            setMultipleTilesAsNonPlaceable(idsToMakeNonTraversable);
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
                            int[] idsToMakeNonTraversable = { currentTile - 10 - 1, currentTile + 10 - 1, currentTile - 10 - 2, currentTile + 10 - 2, currentTile - 2 };
                            setMultipleTilesAsNonPlaceable(idsToMakeNonTraversable);
                        }

                        if (random1TileChance() == true && checkIfObstacleAdjacent(currentTile) == false && filledTiles.Contains(currentTile) == false && tile1x1Counter < 3)
                        {
                            Vector3 position = roomTiles.transform.Find(returnRoomTileName(currentTile)).transform.position;
                            //position = new Vector3(position.x, position.y, position.z - wardrobePositionOffset);
                            GameObject tile1x1 = Instantiate(gameManagerScript.tileObstacle1x1, position, Quaternion.Euler(0, wardrobeRotationOffset, 0));
                            GameObject tile1x1test = Instantiate(gameManagerScript.getRandom1x1WallObstacles(), position, Quaternion.Euler(0, wardrobeRotationOffset, 0));
                            //GameObject wardrobe2 = Instantiate(gameManagerScript.deadBody, position, Quaternion.Euler(0, wardrobeRotationOffset, 0));
                            filledTiles.Add(currentTile);
                            //filledTiles.Add(currentTile - 10);
                            int[] idsToMakeNonTraversable = { currentTile - 10 - 1, currentTile + 10 - 1, currentTile - 1 };
                            setMultipleTilesAsNonPlaceable(idsToMakeNonTraversable);
                        }
                    }
                }
                break;

            case "Top":
                wardrobeRotationOffset = 180;
                randomPaintingSpawn(gameObject.transform.Find("Top Wall").gameObject);
                for (int i = 1; i < MAX_ROWS + 1; i++)
                {
                    int currentTile = i;
                    if (nonPlaceableTiles.Contains(currentTile) == false)
                    {
                        if (randomWardrobeChance() == true && wardrobeChecks(currentTile, _wallDirection) == true && wardrobeCounter < 1)
                        {
                            Vector3 position = roomTiles.transform.Find(returnRoomTileName(currentTile)).transform.position;
                            position = new Vector3(position.x + wardrobePositionOffset, position.y, position.z);
                            GameObject wardrobe = Instantiate(gameManagerScript.tileObstacle2x1, position, Quaternion.Euler(0, wardrobeRotationOffset, 0));
                            GameObject wardrobe2 = Instantiate(gameManagerScript.wardrobe, position, Quaternion.Euler(0, wardrobeRotationOffset, 0));
                            filledTiles.Add(currentTile);
                            filledTiles.Add(currentTile + 1);
                            int[] idsToMakeNonTraversable = { currentTile + 10, currentTile + 10 - 1, currentTile + 10 + 1, currentTile + 10 + 2 };
                            setMultipleTilesAsNonPlaceable(idsToMakeNonTraversable);
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
                            int[] idsToMakeNonTraversable = { currentTile + 10 - 1, currentTile + 10 + 1, currentTile + 20 - 1, currentTile + 20 + 1, currentTile + 20 };
                            setMultipleTilesAsNonPlaceable(idsToMakeNonTraversable);

                        }

                        if (random1TileChance() == true && checkIfObstacleAdjacent(currentTile) == false && filledTiles.Contains(currentTile) == false && tile1x1Counter < 3)
                        {
                            Vector3 position = roomTiles.transform.Find(returnRoomTileName(currentTile)).transform.position;
                            //position = new Vector3(position.x, position.y, position.z - wardrobePositionOffset);
                            GameObject tile1x1 = Instantiate(gameManagerScript.tileObstacle1x1, position, Quaternion.Euler(0, wardrobeRotationOffset, 0));
                            GameObject tile1x1test = Instantiate(gameManagerScript.getRandom1x1WallObstacles(), position, Quaternion.Euler(0, wardrobeRotationOffset, 0));
                            //GameObject wardrobe2 = Instantiate(gameManagerScript.deadBody, position, Quaternion.Euler(0, wardrobeRotationOffset, 0));
                            filledTiles.Add(currentTile);
                            //filledTiles.Add(currentTile - 10);
                            int[] idsToMakeNonTraversable = { currentTile + 10 - 1, currentTile + 10 + 1, currentTile + 10 };
                            setMultipleTilesAsNonPlaceable(idsToMakeNonTraversable);

                        }
                    }
                }
                break;

            case "Bottom":
                wardrobeRotationOffset = 0;
                randomPaintingSpawn(gameObject.transform.Find("Bottom Wall").gameObject);
                for (int i = 1; i < MAX_ROWS + 1; i++)
                {
                    int currentTile = i + 90;
                    if (nonPlaceableTiles.Contains(currentTile) == false)
                    {
                        if (randomWardrobeChance() == true && wardrobeChecks(currentTile, _wallDirection) == true && wardrobeCounter < 1)
                        {
                            Vector3 position = roomTiles.transform.Find(returnRoomTileName(currentTile)).transform.position;
                            position = new Vector3(position.x + wardrobePositionOffset, position.y, position.z);
                            GameObject wardrobe = Instantiate(gameManagerScript.tileObstacle2x1, position, Quaternion.Euler(0,wardrobeRotationOffset, 0));
                            GameObject wardrobe2 = Instantiate(gameManagerScript.wardrobe, position, Quaternion.Euler(0, wardrobeRotationOffset, 0));
                            filledTiles.Add(currentTile);
                            filledTiles.Add(currentTile + 1);
                            int[] idsToMakeNonTraversable = { currentTile - 10, currentTile - 10 - 1, currentTile - 10 + 1, currentTile - 10 + 2 };
                            setMultipleTilesAsNonPlaceable(idsToMakeNonTraversable);
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
                            int[] idsToMakeNonTraversable = { currentTile - 10 - 1, currentTile - 10 + 1, currentTile - 20 - 1, currentTile - 20 + 1, currentTile - 20 };
                            setMultipleTilesAsNonPlaceable(idsToMakeNonTraversable);
                        }

                        if (random1TileChance() == true && checkIfObstacleAdjacent(currentTile) == false && filledTiles.Contains(currentTile) == false && tile1x1Counter < 3)
                        {
                            Vector3 position = roomTiles.transform.Find(returnRoomTileName(currentTile)).transform.position;
                            //position = new Vector3(position.x, position.y, position.z - wardrobePositionOffset);
                            GameObject tile1x1 = Instantiate(gameManagerScript.tileObstacle1x1, position, Quaternion.Euler(0, wardrobeRotationOffset, 0));
                            GameObject tile1x1test = Instantiate(gameManagerScript.getRandom1x1WallObstacles(), position, Quaternion.Euler(0, wardrobeRotationOffset, 0));

                            filledTiles.Add(currentTile);

                            ///
                            /// probably wrong underneath
                            ///
                            //filledTiles.Add(currentTile - 10);
                            int[] idsToMakeNonTraversable = { currentTile - 10 - 1, currentTile - 10 + 1, currentTile - 10 };
                            setMultipleTilesAsNonPlaceable(idsToMakeNonTraversable);
                        }
                    }
                }
                break;
        }
    }

    /// <summary>
    /// algorithm used for spawning obstacles on center tiles
    /// </summary>
    private void spawnObstacles()
    {
        List<int> validIds = new List<int>();
        for (int i = 0; i < centerTiles.Count(); i++)
        {
            int randNum = Random.Range(0, gameManagerScript.max2x2TileObstacleSpawnChance);
            if (largeObstacleValidationCheck(centerTiles.ElementAt(i)) == true && randNum == 1)
            {
                validIds.Add(centerTiles.ElementAt(i));
                SetImpassableBorderFor2x2Obstacle(validIds.ElementAt(validIds.Count - 1));
            }
        }
        for (int i = 0; i < validIds.Count;i++)
        {
            if (obstacles2x2Counter > max2x2Obstacles)
            {
                break;
            }
            else
            {
                obstacles2x2Counter++;
                Vector3 position = roomTiles.transform.Find(returnRoomTileName(validIds.ElementAt(i))).transform.position + obstacle2x2PositionOffset;
                GameObject wardrobe = Instantiate(gameManagerScript.tileObstacle2x2, position, Quaternion.Euler(0, wardrobeRotationOffset, 0));
                GameObject Obstacle = Instantiate(gameManagerScript.getRandomFloorObstacle(), position , Quaternion.Euler(0, Random.Range(1, 4) * 90, 0));
            }
        }
    }

    /// <summary>
    /// algorithm used for spawning decorations
    /// </summary>
    private void spawnDecorations()
    {
        for (int i = 0; i < centerTiles.Count(); i++)
        {
            if (nonPlaceableTiles.Contains(centerTiles.ElementAt(i)) == false && filledTiles.Contains(centerTiles.ElementAt(i)) == false)
            {
                if (Random.Range(0, gameManagerScript.maxFloorDecorationChance) == 1)
                {
                    Vector3 position = roomTiles.transform.Find(returnRoomTileName(centerTiles.ElementAt(i))).transform.position;
                    GameObject decoration = Instantiate(gameManagerScript.getRandomFloorDecoration(), position , Quaternion.Euler(0, Random.Range(1, 360) /** 90*/, 0));
                }

            }
        }
    }

    // sets the border of obstacles to be a non placeable area 
    private void SetImpassableBorderFor2x2Obstacle(int t_id)
    {
        List<int> validIds = new List<int>();

        int cutShortNumber = 9999;
        if (t_id % 10 == 9 )
        {
            cutShortNumber = 2;
        }
        else if (t_id % 10 == 0 )
        {
            cutShortNumber = 1;
        }

        for (int i = 0; i < 3; i++)
        {
            if (i == cutShortNumber)
            {
                break;
            }

            else if (i == 0)
            {
                if (t_id % 10 != 1)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        validIds.Add(t_id - 11 + (j * 10));
                    }
                }
                validIds.Add(t_id - 10);
                validIds.Add(t_id + 20);
            }
            else if (i == 1)
            {
                validIds.Add(t_id - 9);
                validIds.Add(t_id + 21);
            }
            else if (i == 2)
            {
                for (int j = 0; j < 4; j++)
                {
                    validIds.Add(t_id - 8 + (j * 10));
                }
            }
        }
        for (int i = 0; i < validIds.Count;i++)
        {
            if (validIds.ElementAt(i) > 0 && validIds.ElementAt(i) < 100)
            {
                setTileAsNonPlaceable(validIds.ElementAt(i));
            }
        }

    }

    /// <summary>
    /// checks if placement is valid for large 2x2 obstacles
    /// </summary>
    /// <param name="_id"></param>
    /// <returns></returns>
    private bool largeObstacleValidationCheck(int _id)
    {
        int[] validationCheckOffsets = { 1, 10, 11 };
        if (filledTiles.Contains(_id) == false && nonPlaceableTiles.Contains(_id) == false
            && filledTiles.Contains(_id + validationCheckOffsets[0]) == false && nonPlaceableTiles.Contains(_id + validationCheckOffsets[0]) == false
            && filledTiles.Contains(_id + validationCheckOffsets[1]) == false && nonPlaceableTiles.Contains(_id + validationCheckOffsets[1]) == false
            && filledTiles.Contains(_id + validationCheckOffsets[2]) == false && nonPlaceableTiles.Contains(_id + validationCheckOffsets[2]) == false)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// ramdomises if a wardrobe will be placed on that tile
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// ramdomises if a 1x1 tile object will be placed
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// ramdomises if a 2x1 tile object will be placed
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// gets the name of the tile
    /// </summary>
    /// <param name="_id"></param>
    /// <returns></returns>
    private string returnRoomTileName(int _id)
    {
        return "RoomTile (" + _id + ")";
    }

    /// <summary>
    /// checks if there is already an adjacent obstacle at a certain point
    /// </summary>
    /// <param name="_id"></param>
    /// <returns></returns>
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

    /// <summary>
    /// used by the CheckIfObstacleAdjacent() function
    /// </summary>
    /// <param name="_id"></param>
    /// <returns></returns>
    private bool checkIfObstacleOnRight(int _id)
    {
        if (filledTiles.Contains(_id + 1) == true)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// used by the CheckIfObstacleAdjacent() function
    /// </summary>
    /// <param name="_id"></param>
    /// <returns></returns>
    private bool checkIfObstacleOnLeft(int _id)
    {
        if (filledTiles.Contains(_id - 1) == true)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// used by the CheckIfObstacleAdjacent() function
    /// </summary>
    /// <param name="_id"></param>
    /// <returns></returns>
    private bool checkIfObstacleOnTop(int _id)
    {
        if (filledTiles.Contains(_id - 10) == true)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// used by the CheckIfObstacleAdjacent() function
    /// </summary>
    /// <param name="_id"></param>
    /// <returns></returns>
    private bool checkIfObstacleOnBottom(int _id)
    {
        if (filledTiles.Contains(_id + 10) == true)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// checks if wardrobe placement is valid
    /// </summary>
    /// <param name="_id"></param>
    /// <param name="_wallDirection"></param>
    /// <returns></returns>
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

    /// <summary>
    /// checks if 2x2 tile placement is valid
    /// </summary>
    /// <param name="_id"></param>
    /// <param name="_wallDirection"></param>
    /// <returns></returns>
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

    /// <summary>
    /// randomly chooses if a painting becomes active
    /// </summary>
    /// <param name="_wall"></param>
    private void randomPaintingSpawn(GameObject _wall)
    {
        int spawnChance = Random.Range(0,gameManagerScript.maxPaintingSpawnChance);
        GameObject tempGameObject = _wall.gameObject.transform.Find("Painting1").gameObject;

        if (spawnChance == 1)
        {
            tempGameObject.SetActive(true);
        }

    }


}
