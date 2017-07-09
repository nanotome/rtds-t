using UnityEngine;

public class GunController : MonoBehaviour {

    public Transform primaryWeaponHold;
    public Transform secondaryWeaponHold;

    public Gun[] allGuns;
    Gun equippedGun;

    public void OnTriggerHold()
    {
        if (equippedGun != null)
        {
            equippedGun.OnTriggerHold();
        }
    }

    public void OnTriggerRelease()
    {
        if (equippedGun != null)
        {
            equippedGun.OnTriggerRelease();
        }
    }

    public void Aim(Vector3 aimPoint)
    {
        if (equippedGun != null)
        {
            equippedGun.Aim(aimPoint);
        }
    }
}
