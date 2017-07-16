using System;
using System.Collections.Generic;
using UnityEngine;
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
    // List of prefabs this tile can spawn.
    public List<GameObject> prefabs;

    Random prng;

    public TileInfo(Coord mapPos, TilePosition tilePos, string posId)
    {
        pos = mapPos;
        position = tilePos;
        id = posId;

        prefabType = PrefabType.None;
        prefabs = new List<GameObject>();

        prng = new Random();
    }

    public GameObject SelectedPrefab()
    {
        // pick a random prefab from the list of prefabs and spawn it
        return prefabs[prng.Next(prefabs.Count)];
    }
}
