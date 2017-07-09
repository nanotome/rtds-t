using UnityEngine;

public class GunController : MonoBehaviour {

    public Transform primaryWeaponHold;
    public Transform secondaryWeaponHold;

    public Gun[] allGuns;
    Gun[] equippedGuns = new Gun[2];

    // these are used to keep track of which weapon is equipped in which hold
    int primaryEquip = 0;
    int secondaryEquip = 1;

    private void Start()
    {
        equippedGuns[0] = Instantiate(allGuns[0], primaryWeaponHold.position, primaryWeaponHold.rotation);
        equippedGuns[1] = Instantiate(allGuns[1], secondaryWeaponHold.position, secondaryWeaponHold.rotation);
    }

    public void OnTriggerHold()
    {
        if (equippedGuns[0] != null)
        {
            equippedGuns[0].OnTriggerHold();
        }
    }

    public void OnTriggerRelease()
    {
        if (equippedGuns[0] != null)
        {
            equippedGuns[0].OnTriggerRelease();
        }
    }

    public void Aim(Vector3 aimPoint)
    {
        if (equippedGuns[0] != null)
        {
            equippedGuns[0].Aim(aimPoint);
        }
    }

    public void EquipGun(Gun gunToEquip, int gunPriority, Transform weaponHold)
    {
        if (equippedGuns[gunPriority] != null)
        {
            Destroy(equippedGuns[gunPriority].gameObject);
        }

        equippedGuns[gunPriority] = Instantiate(gunToEquip, weaponHold.position, weaponHold.rotation);
        equippedGuns[gunPriority].transform.parent = weaponHold;
    }

    public void EquipSecondaryGun(int gunIndex)
    {
        EquipGun(allGuns[gunIndex], 1, secondaryWeaponHold);
        secondaryEquip = gunIndex;
    }

    public void EquipPrimaryGun(int gunIndex)
    {
        EquipGun(allGuns[gunIndex], 0, primaryWeaponHold);
        primaryEquip = gunIndex;
    }

    public void SwitchGuns()
    {
        // Swap secondary and primary equip indexes
        int temp = primaryEquip;
        primaryEquip = secondaryEquip;
        secondaryEquip = temp;

        // equip guns
        EquipPrimaryGun(primaryEquip);
        EquipSecondaryGun(secondaryEquip);
    }
}
