using UnityEngine;

[RequireComponent(typeof (Rigidbody))]
public class PlayerController : MonoBehaviour {

    Vector3 velocity;
    Rigidbody rb;

    bool isDashing;
    float dashspeed;

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
        if (isDashing)
        {
            rb.MovePosition(transform.position + transform.forward * dashspeed * Time.deltaTime);
            isDashing = false;
        } else
        {
            // Move the Player to a position specified by the input received
            rb.MovePosition(rb.position + velocity * Time.deltaTime);
        }
        
    }

    // move the object that has this script as a component
    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }

    public void LookAt(Vector3 lookPoint)
    {
        // Set the point to the same height as the Player
        Vector3 newPoint = new Vector3(lookPoint.x, transform.position.y, lookPoint.z);
        transform.LookAt(newPoint);
    }

    public void Dash(float speed)
    {
        isDashing = true;
        dashspeed = speed;
    }
}
