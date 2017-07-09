using UnityEngine;

public class Projectile : MonoBehaviour {

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
}
