using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// R : 지팡이
public class MagicWandController : WeaponController
{
    private Animator anim;

    public override void Init()
    {
        anim = GetComponent<Animator>();
    }

    public override bool CanChange(DetailType newWeaponType, string newItemName)
    {
        if (WeaponManager.Instance.currWeaponR.PPikcUpItem.Name != newItemName)
            return true;
        else
            Debug.Log("이미 같은 무기를 장착중입니다.");
        return false;
    }

    public override void ChangeWeapon()
    {

    }
}
