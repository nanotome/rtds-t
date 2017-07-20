using System.Collections.Generic;
using UnityEngine;

public class Pack : MonoBehaviour, ICollectible {

    // The different types of packs. Ammo packs increase ammo; health packs
    // increase health.
    public enum PackType { Ammo, Health, Experience };
    public PackType packType;
    // The amount the collider's attribute will be raised by when they
    // pick up this pack.
    public int itemValue;
    // The kinds of LivingEntity objects that can utilize this pack.
    List<string> permissibleTags = new List<string>()
    {
        "Player"
    };

    public void OnTriggerEnter(Collider collider)
    {
        if (!permissibleTags.Contains(collider.tag))
        {
            return;
        }

        if (packType == PackType.Ammo)
        {
            collider.GetComponent<GunController>().UpdateAmmo(itemValue);
        } else if (packType == PackType.Health)
        {
            collider.GetComponent<Player>().UpdateHealth(itemValue);
        } else if (packType == PackType.Experience)
        {
            collider.GetComponent<Player>().UpdateXP(itemValue);
        }

        Destroy(gameObject);
    }
}
