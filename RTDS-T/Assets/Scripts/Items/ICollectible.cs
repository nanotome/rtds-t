public interface ICollectible {
    // When a LivingEntity collides with an object that implements this interface,
    // the object should pick an attribute from the LivingEntity and modify it.
    // a true isQuantity signifies that the item will raise the LivingEntity's attribute by
    // a value. false isQuantity means the LivingEntity will simply keep this item.
    // a true isRaise signifies that the raise from isQuantity will be positive.
    void ModifyCollectorAttribute(LivingEntity entity, string attributeName, bool isQuantity, bool isRaise);
}
