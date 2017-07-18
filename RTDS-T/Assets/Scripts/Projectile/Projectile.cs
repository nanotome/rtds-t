using UnityEngine;

public class Projectile : MonoBehaviour {

    public LayerMask collisionLayer;
    // This allows us to create projectiles that don't move forward.
    float projectileSpeed = 10;
    float lifeTime = 3f;

    float projectileDamage = 1; // TODO: this should be configurable for different projectiles
    float skinWidth = .1f;

	void Start () {
        Destroy(gameObject, lifeTime);

        // compute collision detection for the first object the Projectile
        // collides with.
        Collider[] initialCollisions = Physics.OverlapSphere(transform.position, .1f, collisionLayer);
        if (initialCollisions.Length > 0)
        {
            OnHitObject(initialCollisions[0], transform.position);
        }
    }
	
	// Update is called once per frame
	void Update () {
        float moveDistance = projectileSpeed * Time.deltaTime;
        CheckCollisions(moveDistance);

        // propel the projectile
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

    public void SetDamage(float damage)
    {
        projectileDamage = damage;
    }

    // Collision detection using colliders
    void CheckCollisions(float dist)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        // Cast a ray from the projectile at a specified distance + the thickness of the
        // potential targets skin (this creates a hitBox for the target).
        // If the ray hits something on the specified layer, handle the collision.
        if (Physics.Raycast(ray, out hit, dist + skinWidth, collisionLayer, QueryTriggerInteraction.Collide))
        {
            OnHitObject(hit.collider, hit.point);
        }
    }

    // Handle a successful collision
    void OnHitObject(Collider collider, Vector3 hitPoint)
    {
        // Get a reference to the hit object's IDamageable
        IDamageable damageableObject = collider.GetComponent<IDamageable>();

        if (damageableObject != null)
        {
            damageableObject.TakeHit(projectileDamage, hitPoint, transform.forward);
        }

        Destroy(gameObject);
    }
}
