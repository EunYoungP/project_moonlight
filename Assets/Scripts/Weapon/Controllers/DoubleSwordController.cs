using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// L : 검
// R : 검

// 넘겨받은 weapon 오브젝트의 타입에따라 다른곳에 장착
public class DoubleSwordController : WeaponController
{
    private Animator anim;

    public override void Init()
    {
        anim = GetComponent<Animator>();
        anim.runtimeAnimatorController =Resources.Load<RuntimeAnimatorController>("Prefabs/Animator/Player DoubleSword");
    }

    // 새로운 무기와 같은 무기인지 검사
    public override bool CanChange(DetailType newWeaponType, string newItemName)
    {
        if(WeaponManager.Instance.currWeaponL.PPikcUpItem.Name != newItemName
         || WeaponManager.Instance.currWeaponL == null)
            return true;
        else
            Debug.Log("이미 같은 무기를 장착중입니다.");
        return false;
    }

    // 현재무기를 없애고
    // 받아온 새로운 무기를 맞는위치에 장착 시키는 코드
    public override void ChangeWeapon()
    {
        CancleWeapon();
        SetWeaponPos();
        

        WeaponManager.Instance.currWeaponAnim.runtimeAnimatorController = anim.runtimeAnimatorController;
        Player.Instance.ChangeAnimator();
    }

    // 손에 무기가 있을경우 삭제
    private void CancleWeapon()
    {
        if (WeaponManager.Instance.currWeaponL != null)
            Destroy(WeaponManager.Instance.currWeaponL.gameObject);
        if (WeaponManager.Instance.currWeaponR != null)
            Destroy(WeaponManager.Instance.currWeaponR.gameObject);
    }

    protected override void SetWeaponPos()
    {
        if (WeaponManager.Instance.newWeapon == null)
            return;

        GameObject newWeaponL = Instantiate(WeaponManager.Instance.newWeapon, WeaponManager.Instance.weaponPosL);
        //newWeaponL.transform.rotation = Quaternion.Euler(180, 0, 0);
        GameObject newWeaponR = Instantiate(WeaponManager.Instance.newWeapon, WeaponManager.Instance.weaponPosR);
        //newWeaponR.transform.rotation = Quaternion.Euler(180, 0, 0);

        WeaponManager.Instance.currWeaponL = newWeaponL.GetComponent<PickUpItem>();
        WeaponManager.Instance.currWeaponR = newWeaponR.GetComponent<PickUpItem>();
    }
}
