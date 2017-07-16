using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {

    public enum TilePosition { Wall, Room, Corridor };
    public enum PrefabType
    {
        None, Wall, Obstacles, Enemy, Player
    };
    public enum Direction
    {
        North, East, South, West
    };

    public int columns = 50;
    public int rows = 50;
    public IntRange numRooms = new IntRange(4, 5);
    public IntRange width = new IntRange(4, 10);
    public IntRange height = new IntRange(8, 10);
    public IntRange corridorLength = new IntRange(2, 4);

    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] obstacleTiles;
    public GameObject[] enemies;
    public GameObject exit;
    public GameObject player;

    // GameObject that acts as a container for all other tiles.
    private Transform boardHolder;

    List<TileInfo> allTiles;
    List<Room> rooms;
    List<Corridor> corridors;

    void Start () {
        boardHolder = transform.Find("BoardHolder").transform;
        allTiles = new List<TileInfo>(columns * rows);
	}
}
