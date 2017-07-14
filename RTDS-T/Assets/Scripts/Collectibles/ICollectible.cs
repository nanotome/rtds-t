using UnityEngine;

// ICollectible are objects that can be collected by LivingEntity
// objects.
public interface ICollectible {
    void OnTriggerEnter(Collider collider);
}
