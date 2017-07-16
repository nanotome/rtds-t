using System;
using System.Collections.Generic;
using Random = System.Random;
using UnityEngine;

public class BoardManager : MonoBehaviour {

    public enum TilePosition { Wall, Room, Corridor };
    public enum PrefabType
    {
        Wall, Obstacles, Enemy, Player
    };

    public int columns = 50;
    public int rows = 50;
    public IntRange numRooms = new IntRange(4, 5);
    public IntRange roomWidth = new IntRange(4, 10);
    public IntRange roomHeight = new IntRange(8, 10);
    public IntRange corridorLength = new IntRange(2, 4);

    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] obstacleTiles;
    public GameObject[] enemies;
    public GameObject exit;
    public GameObject player;

    // GameObject that acts as a container for all other tiles.
    private Transform boardHolder;

    void Start () {
        boardHolder = transform.Find("BoardHolder").transform;
	}

    [Serializable]
    public struct TileInfo
    {
        // The position of the tile on the map's grid.
        public Coord pos;
        // The TilePosition determines if the tile is a wall or if it's in
        // a room or corridor.
        public TilePosition position;
        // ID of the room or corridor
        public string id;

        // The type of initial prefab placed on this tile (on the Floor tile).
        public PrefabType prefabType;
        // List of prefabs this tile can spawn.
        public List<GameObject> prefabs;

        Random prng;

        public TileInfo(Coord mapPos, int xPos, int yPos, TilePosition tilePos, string posId)
        {
            pos = mapPos;
            position = tilePos;
            id = posId;

            prefabType = PrefabType.Wall;
            prefabs = new List<GameObject>();

            prng = new Random();
        }

        public void SpawnPrefab()
        {
            // pick a random prefab from the list of prefabs and spawn it
            GameObject itemToSpawn = prefabs[prng.Next(prefabs.Count)];
            Instantiate(itemToSpawn, new Vector3(pos.x, 0, pos.y), Quaternion.identity);
        }
    }
}
