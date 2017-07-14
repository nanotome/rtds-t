using UnityEngine;

public class Pack : MonoBehaviour, ICollectible {

    public int itemValue;

    public virtual void ModifyCollectorAttribute(LivingEntity entity)
    {
        
    }
}
