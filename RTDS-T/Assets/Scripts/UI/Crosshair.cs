using UnityEngine;

public class Crosshair : MonoBehaviour {

    // new position of the croshair obtained from the Player script
    Vector3 velocity;
    // speed of movement of the crosshair obtained from the Player script.
    // This can be affected by the weight of a gun so the speed is better placed
    // in the Player script.
    float aimSpeed;

	void Start () {
        // Remove the mouse cursor
        Cursor.visible = false;
	}

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + velocity, aimSpeed * Time.deltaTime);
    }

    public void Move(Vector3 _velocity, float _aimSpeed)
    {
        velocity = _velocity;
        aimSpeed = _aimSpeed;
    }
}
