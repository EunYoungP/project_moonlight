using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPosL : MonoBehaviour
{
    private PickUpItem currWeapon;

    private void Init()
    {
        currWeapon = GetComponentInChildren<PickUpItem>();
    }
}
