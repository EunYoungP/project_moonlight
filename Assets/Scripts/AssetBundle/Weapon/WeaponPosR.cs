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
    
    //public void SetWeapon(GameObject weapon)
    //{
    //    foreach(GameObject gameObject in ResourceManager.Instance.ITEM)
    //    {
    //        if(gameObject == weapon && currWeapon != weapon)
    //        {
    //            if(currWeapon != null)
    //                Destroy(currWeapon.gameObject);

    //            GameObject nextWeapon =  Instantiate(gameObject, this.transform);
    //            nextWeapon.transform.rotation = Quaternion.Euler(180, 0, 0);
    //            currWeapon = nextWeapon.GetComponent<PickUpItem>();
    //        }
    //    }
    //}
}
