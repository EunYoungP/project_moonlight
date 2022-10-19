using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPosR : MonoBehaviour
{
    // weaponPos 의 자식 PickupItem
    private PickUpItem currWeapon;

    private static WeaponPosR instance;
    public static WeaponPosR Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<WeaponPosR>();
            }

            instance.Init();
            return instance;
        }
    }

    public void Init()
    {
        // 현재무기 얻어오기
        currWeapon = GetComponentInChildren<PickUpItem>();
    }
}
