using System.Collections.Generic;
using UnityEngine;

// IItemCase are objects that release items when they 'die'. For example, an
// Enemy that drops health or ammo would need to implement IItemCase
public interface IItemCase
{
    // Explosion behaviour is common to both enemies and destructible obstacles so
    // it will be placed here.
    // This method instantiates the particles
    void Explode(ParticleSystem deathEffect, Vector3 hitPoint);
    void SpawnItems(List<Pack> items);
}
