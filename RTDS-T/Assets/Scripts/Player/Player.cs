// The Player script receives input for the Player.

using UnityEngine;

[RequireComponent(typeof (PlayerController))]
public class Player : MonoBehaviour {

    // The PlayerController script handles the movement of the Player
    PlayerController playerController;
    Camera viewCamera;

    float moveSpeed = 5f;

	void Start () {
        playerController = GetComponent<PlayerController>();
        viewCamera = Camera.main;
	}
	
	void Update () {
        // Move Player based on input from the keyboard
        Vector3 moveInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        playerController.Move(moveInput.normalized * moveSpeed);

        // Turn Player to face the direction of the cursor
        // The screen is a 2d world but the game is in a 3d world so we need raycasting to get the
        // accurate position of the mouse cursor in the game world.

        // 1. Cast a ray from the camera through the mouse cursor's position
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
        // 2. draw a plane that's perpendicular to the y-axis and passes through a point at the gun height.
        // this would be a plane just above the ground and slicing the player at gun height.
        Plane groundPlane = new Plane(Vector3.up, Vector3.up * 1); // 1 here represents the height of the gun
        float rayDistance;

        // 3. if the ray drawn earlier intersects the plane
        if (groundPlane.Raycast(ray, out rayDistance))
        {
            // 4. get the point where the ray intersects the plane
            Vector3 point = ray.GetPoint(rayDistance);
            // 5. turn the Player to face that point
            playerController.LookAt(point);
        }
	}
}
