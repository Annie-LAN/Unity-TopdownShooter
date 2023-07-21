using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public Transform weaponHold;
    public Gun startingGun;
    Gun equippedGun;

    private void Start()
    {
        if(startingGun != null)
        {
            EquipGun(startingGun);
        }

    }
    public void EquipGun(Gun gunToEquip){
        if(equippedGun != null)
        {
            Destroy(equippedGun.gameObject);
        }
        // instantiate an object, then use 'as Gun' to change it to a Gun
        equippedGun = Instantiate(gunToEquip, weaponHold.position, weaponHold.rotation) as Gun;
        // make equippedGun a child of weaponHold so that it moves with the player
        equippedGun.transform.parent = weaponHold;
    }
}
