using UnityEngine;

// This defines the unique behavior of a floor boss
// The floor boss contains the exit and spawns it on death.
public class FloorBoss : MonoBehaviour {

    public Exit exit;

	public void SpawnExit(Vector3 pos)
    {
        Instantiate(exit, pos, Quaternion.identity);
    }
}
