using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// L : 활
// R : 화살
public class BowController : WeaponController
{
    private Animator anim;

    public override void Init()
    {
        anim = GetComponent<Animator>();
    }

    public override bool CanChange(DetailType newWeaponType, string newItemName)
    {
        if (WeaponManager.Instance.currWeaponL.PPikcUpItem.Name != newItemName)
            return true;
        else
            Debug.Log("이미 같은 무기를 장착중입니다.");
        return false;
    }

    public override void ChangeWeapon()
    {

    }
}
