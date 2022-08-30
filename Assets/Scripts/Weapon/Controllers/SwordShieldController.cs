using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// L : 방패
// R : 성검
// 유일한 양손 무기
public class SwordShieldController : WeaponController
{
    private Animator anim;

    public override void Init()
    {
        anim = GetComponent<Animator>();
    }

    public override bool CanChange(DetailType newWeaponType, string newItemName)
    {
        // 같은 타입일경우 같은 무기인지 검사
        if(newWeaponType == DetailType.Sword)
        {
            if (WeaponManager.Instance.currWeaponR != null)
            {
                if (WeaponManager.Instance.currWeaponR.PPikcUpItem.Name != newItemName)
                    return true;
                else
                    Debug.Log("이미 같은 무기를 장착중입니다.");
            }
        }
        else if(newWeaponType == DetailType.Shield)
        {
            if (WeaponManager.Instance.currWeaponL != null)
            {
                if (WeaponManager.Instance.currWeaponL.PPikcUpItem.Name != newItemName)
                    return true;
                else
                    Debug.Log("이미 같은 무기를 장착중입니다.");
            }
        }
        return false;
    }

    private void CancleWeapon()
    {

    }

    public override void ChangeWeapon()
    {

    }
}
