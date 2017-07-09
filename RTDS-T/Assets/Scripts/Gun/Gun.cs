using UnityEngine;

public class Gun : MonoBehaviour {

    public Transform projectileSpawn;

    public enum FireMode { Auto, Single };
    public FireMode fireMode;

    public Projectile bulletType;
    public float msBetweenShots;
    public float bulletSpeed;

    float nextShotTime;
    bool isTriggerReleased;

	void Shoot()
    {
        if (Time.time > nextShotTime)
        {
            // for the Single fire mode, the gun should not shoot unless
            // the player has released the trigger beforehand. This constrains
            // the gun to one shot at a time.
            if (fireMode == FireMode.Single)
            {
                if (!isTriggerReleased)
                {
                    return;
                }
            }

            nextShotTime = Time.time + msBetweenShots / 1000;
            Projectile newProjectile = Instantiate(bulletType, projectileSpawn.position, projectileSpawn.rotation);
            newProjectile.SetSpeed(bulletSpeed);
        }
    }

    public void OnTriggerHold()
    {
        Shoot();
        isTriggerReleased = false;
    }

    public void OnTriggerRelease()
    {
        isTriggerReleased = true;
    }

    public void Aim(Vector3 aimPoint)
    {
        transform.LookAt(aimPoint);
    }
}
