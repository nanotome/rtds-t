using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour {

    public enum TilePosition { Wall, Room, Corridor };
    public enum PrefabType
    {
        Wall, None, Obstacle, Enemy, Player
    };
    public enum Direction
    {
        North, East, South, West
    };

    public int columns = 50;
    public int rows = 50;
    public IntRange numRooms = new IntRange(4, 5);
    public IntRange roomWidth = new IntRange(4, 10);
    public IntRange roomHeight = new IntRange(8, 10);
    public IntRange corridorLength = new IntRange(2, 4);
    public IntRange innerWallsPerRoom = new IntRange(2, 3);
    public IntRange enemiesPerRoom = new IntRange(4, 6);
    public IntRange obstaclesPerRoom = new IntRange(2, 3);

    public Transform[] floorTiles;
    public Transform[] wallTiles;
    public Transform[] obstacleTiles;
    public Transform[] enemies;
    public Transform exit;
    public Transform player;

    // GameObject that acts as a container for all other tiles.
    private Transform boardHolder;
    // GameObject to act as walking plane for LivingEntity objects
    private Transform groundPlane;
    private NavMeshSurface groundSurface;

    TileInfo[,] tileMap;
    List<Room> rooms;
    List<Corridor> corridors;

    // Filter the tileMap by specific properties; returns a queue.
    // Currently, this method only filters by the tile's id but it can be improved
    // to filter by any property of the TileInfo.
    List<TileInfo> FilterTileMap(string tileId)
    {
        var query = from TileInfo tile in tileMap
                    where tile.id == tileId
                    select tile;

        return query.ToList();
    }

    Queue<T> ShuffleList<T>(List<T> itemList)
    {
        int n = itemList.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = itemList[k];
            itemList[k] = itemList[n];
            itemList[n] = value;
        }

        return new Queue<T>(itemList);
    }

    void Start () {
        boardHolder = transform.Find("BoardHolder").transform;
        groundPlane = transform.Find("Ground").transform;
        groundSurface = groundPlane.GetComponent<NavMeshSurface>();

        // Position and scale the ground plane
        groundPlane.position = new Vector3(columns / 2 - .5f, 0, rows / 2 - .5f);
        groundPlane.localScale = new Vector3(columns / 10, 1, rows / 10);
        // Rebuild the ground's navmesh
        groundSurface.BuildNavMesh();

        tileMap = new TileInfo[columns, rows];

        CreateRoomsAndCorridors();
        SetTileValuesForRooms();
        SetTileValuesForCorridors();

        LayoutFloor();
        LayoutRoomsAndCorridors();

        LayoutRoomObjects();
    }

    void CreateRoomsAndCorridors()
    {
        // initialize the rooms List with a random size
        rooms = new List<Room>(numRooms.RandomInt);

        // initialize the corridors List with one less the number of rooms
        corridors = new List<Corridor>(rooms.Capacity - 1);

        // create the first room and corridor
        rooms.Insert(0, new Room());
        corridors.Insert(0, new Corridor());

        // set up the first room as there is no corridor
        rooms[0].SetupRoom("start", roomWidth, roomHeight, columns, rows);

        // set up the first corridor using the first room
        corridors[0].SetupCorridor(rooms[0], "corridor-1", corridorLength, roomWidth, roomHeight, columns, rows, true);

        for (int i = 1; i < rooms.Capacity; i++)
        {
            // Create a room.
            rooms.Insert(i, new Room());

            // Setup the room based on the previous corridor.
            rooms[i].SetupRoom($"room-{i + 1}", roomWidth, roomHeight, columns, rows, corridors[i - 1]);

            // If we haven't reached the end of the corridors array...
            if (i < corridors.Capacity)
            {
                // ... create a corridor.
                corridors.Insert(i, new Corridor());

                // Setup the corridor based on the room that was just created.
                corridors[i].SetupCorridor(rooms[i], $"corridor-{i + 1}", corridorLength, roomWidth, roomHeight, columns, rows, false);
            }
        }
    }

    void SetTileValuesForRooms()
    {
        // Go through all the rooms...
        for (int i = 0; i < rooms.Count; i++)
        {
            Room currentRoom = rooms[i];

            // ... and for each room go through it's width.
            for (int j = 0; j < currentRoom.width; j++)
            {
                int xCoord = currentRoom.bottom_left_x + j;

                // For each horizontal tile, go up vertically through the room's height.
                for (int k = 0; k < currentRoom.height; k++)
                {
                    int yCoord = currentRoom.bottom_left_y + k;

                    // The coordinates in the jagged array are based on the room's position and it's width and height.
                    tileMap[xCoord, yCoord] = new TileInfo(new Coord(xCoord, yCoord), TilePosition.Room, PrefabType.None, currentRoom.id);
                }
            }
        }
    }

    void SetTileValuesForCorridors()
    {
        // Go through every corridor...
        for (int i = 0; i < corridors.Count; i++)
        {
            Corridor currentCorridor = corridors[i];

            // and go through it's length.
            for (int j = 0; j < currentCorridor.corridorLength; j++)
            {
                // Start the coordinates at the start of the corridor.
                int xCoord = currentCorridor.startXPos;
                int yCoord = currentCorridor.startYPos;

                // Depending on the direction, add or subtract from the appropriate
                // coordinate based on how far through the length the loop is.
                switch (currentCorridor.direction)
                {
                    case Direction.North:
                        yCoord += j;
                        break;
                    case Direction.East:
                        xCoord += j;
                        break;
                    case Direction.South:
                        yCoord -= j;
                        break;
                    case Direction.West:
                        xCoord -= j;
                        break;
                }

                // Set the tile at these coordinates to Floor.
                tileMap[xCoord, yCoord] = new TileInfo(new Coord(xCoord, yCoord), TilePosition.Corridor, PrefabType.None, currentCorridor.id);
            }
        }
    }

    void LayoutFloor()
    {
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                Transform floorPrefab = floorTiles[Random.Range(0, floorTiles.Length - 1)];
                Transform floorTile = Instantiate(floorPrefab, new Vector3(i, -.25f, j), Quaternion.identity);
                floorTile.parent = boardHolder;
            }
        }
    }

    void LayoutRoomsAndCorridors()
    {
        // iterate through TileInfo and instantiate objects
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                TileInfo tile = tileMap[i, j];
                switch (tile.prefabType)
                {
                    case PrefabType.Wall:
                        Transform wallPrefab = wallTiles[Random.Range(0, wallTiles.Length - 1)];
                        Transform wallTile = Instantiate(wallPrefab, new Vector3(i, 1, j), Quaternion.identity);
                        wallTile.parent = boardHolder;
                        break;
                    case PrefabType.None:
                        break;
                    case PrefabType.Obstacle:
                        break;
                    case PrefabType.Enemy:
                        break;
                    case PrefabType.Player:
                        break;
                    default:
                        break;
                }
            }
        }
    }

    // There is a LOT of repetition in this method which calls for
    // an extract method operation. I'm too tired to think of that though.
    // TODO: extract the object instantiation into a method.
    void LayoutRoomObjects()
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            Room selectedRoom = rooms[i];
            Queue<TileInfo> roomSlots = ShuffleList(FilterTileMap(selectedRoom.id));

            int wallCount = innerWallsPerRoom.RandomInt;
            for (int j = 0; j < wallCount; j++)
            {
                TileInfo wallTileInfo = roomSlots.Dequeue();
                wallTileInfo.prefabType = PrefabType.Wall;
                Transform wallPrefab = wallTiles[Random.Range(0, wallTiles.Length - 1)];
                Transform wallTile = Instantiate(wallPrefab, new Vector3(wallTileInfo.pos.x, 1, wallTileInfo.pos.y), Quaternion.identity);
                wallTile.parent = boardHolder;
            }

            int enemyCount = enemiesPerRoom.RandomInt;
            for (int k = 0; k < enemyCount; k++)
            {
                TileInfo enemyTileInfo = roomSlots.Dequeue();
                enemyTileInfo.prefabType = PrefabType.Enemy;
                Transform enemyPrefab = enemies[Random.Range(0, enemies.Length - 1)];
                Instantiate(enemyPrefab, new Vector3(enemyTileInfo.pos.x, 1, enemyTileInfo.pos.y), Quaternion.identity);
            }

            int obstacleCount = obstaclesPerRoom.RandomInt;
            for (int m = 0; m < obstacleCount; m++)
            {
                TileInfo obstacleTileInfo = roomSlots.Dequeue();
                obstacleTileInfo.prefabType = PrefabType.Obstacle;
                Transform obstaclePrefab = obstacleTiles[Random.Range(0, obstacleTiles.Length - 1)];
                Instantiate(obstaclePrefab, new Vector3(obstacleTileInfo.pos.x, 1, obstacleTileInfo.pos.y), Quaternion.identity);
            }
        }
    }
}
