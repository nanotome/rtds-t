using UnityEngine;

public class Projectile : MonoBehaviour {

    // This allows us to create projectiles that don't move forward.
    float projectileSpeed = 10;
    float lifeTime = 3f;

	void Start () {
        Destroy(gameObject, lifeTime);
	}
	
	// Update is called once per frame
	void Update () {
        float moveDistance = projectileSpeed * Time.deltaTime;
        transform.Translate(Vector3.forward * moveDistance);
	}

    public void SetSpeed(float newSpeed)
    {
        projectileSpeed = newSpeed;
    }

    // This allows us to define projectiles that stay on the field for shorter
    // or longer periods.
    public void SetLifeTime(float newTime)
    {
        lifeTime = newTime;
    }
}
