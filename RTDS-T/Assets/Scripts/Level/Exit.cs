using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour {

    // The kinds of LivingEntity objects that can utilize this pack.
    List<string> permissibleTags = new List<string>()
    {
        "Player"
    };

    private void OnTriggerEnter(Collider collider)
    {
        if (!permissibleTags.Contains(collider.tag))
        {
            return;
        }

        GameManager.instance.Restart();
    }
}
