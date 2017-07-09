// The Player script receives input for the Player.

using UnityEngine;

[RequireComponent(typeof (PlayerController))]
public class Player : MonoBehaviour {

    // The PlayerController script handles the movement of the Player
    PlayerController playerController;

    float moveSpeed = 5f;

	void Start () {
        playerController = GetComponent<PlayerController>();	
	}
	
	void Update () {
        // Move Player based on input from the keyboard
        Vector3 moveInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        playerController.Move(moveInput.normalized * moveSpeed);
	}
}
