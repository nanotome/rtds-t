using System.Collections.Generic;
using UnityEngine;

public class DestructibleObstacle : LivingEntity, IItemCase
{
    public ParticleSystem deathEffect;
    // List of items that may be spawned when the obstacle is destroyed dies
    public List<Pack> deathItems;

    System.Random prng = new System.Random();

    public void Explode(ParticleSystem deathEffect, Vector3 hitPoint)
    {
        Destroy(Instantiate(deathEffect.gameObject, hitPoint, transform.rotation));
    }

    public void SpawnItems(List<Pack> items)
    {
        // pick a random item from the list of items and spawn it
        Pack itemToSpawn = deathItems[prng.Next(items.Count)];
        Instantiate(itemToSpawn, transform.position, transform.rotation);
    }

    public override void Die()
    {
        Explode(deathEffect, transform.position);
        SpawnItems(deathItems);
        base.Die();
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
    }
}
