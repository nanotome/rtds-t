using UnityEngine;

public class BoardManager : MonoBehaviour {

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
}
