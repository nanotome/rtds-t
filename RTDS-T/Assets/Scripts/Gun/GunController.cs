using UnityEngine;

public class GunController : MonoBehaviour {

    public Transform primaryWeaponHold;
    public Transform secondaryWeaponHold;

    public Gun[] allGuns;
    Gun equippedGun;

    // these are used to keep track of which weapon is equipped in which hold
    int primaryEquip = 0;
    int secondaryEquip = 1;

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

    public void EquipGun(Gun gunToEquip, Transform weaponHold)
    {
        if (equippedGun != null)
        {
            Destroy(equippedGun.gameObject);
        }

        equippedGun = Instantiate(gunToEquip, weaponHold.position, weaponHold.rotation);
        equippedGun.transform.parent = weaponHold;
    }

    public void EquipSecondaryGun(int gunIndex)
    {
        EquipGun(allGuns[gunIndex], secondaryWeaponHold);
        secondaryEquip = gunIndex;
    }

    public void EquipPrimaryGun(int gunIndex)
    {
        EquipGun(allGuns[gunIndex], primaryWeaponHold);
        primaryEquip = gunIndex;
    }
}
