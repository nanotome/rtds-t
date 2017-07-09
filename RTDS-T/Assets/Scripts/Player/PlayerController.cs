using UnityEngine;

[RequireComponent(typeof (Rigidbody))]
public class PlayerController : MonoBehaviour {

    Vector3 velocity;
    Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Handle movement of Physics objects
    private void FixedUpdate()
    {
        // Move the Player to a position specified by the input received
        rb.MovePosition(rb.position + velocity * Time.deltaTime);
    }

    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }
}
