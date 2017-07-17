using System;
using Random = System.Random;
using static BoardManager;

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

    Random prng;

    public TileInfo(Coord mapPos, TilePosition tilePos, PrefabType tileType, string posId)
    {
        pos = mapPos;
        position = tilePos;
        id = posId;

        prefabType = tileType;

        prng = new Random();
    }
}
