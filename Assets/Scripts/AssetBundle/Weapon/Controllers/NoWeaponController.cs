using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 기본 상태
public class NoWeaponController : WeaponController
{
    private Animator anim;

    public override void Init()
    {
        anim = GetComponent<Animator>();
        anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Prefabs/Animator/Player NoWeapon");
        WeaponManager.Instance.currWeaponAnim.runtimeAnimatorController = anim.runtimeAnimatorController;
        Player.Instance.playerController.m_animator.runtimeAnimatorController = anim.runtimeAnimatorController;
    }

    // 무기없는 상태로 변경
    public override void ChangeWeapon()
    {
        CancleWeapon();

        WeaponManager.Instance.currWeaponAnim.runtimeAnimatorController = anim.runtimeAnimatorController;
        Player.Instance.ChangeAnimator();
    }

    private void CancleWeapon()
    {
        if (WeaponManager.Instance.currWeaponL != null)
            Destroy(WeaponManager.Instance.currWeaponL.gameObject);
        if (WeaponManager.Instance.currWeaponR != null)
            Destroy(WeaponManager.Instance.currWeaponR.gameObject);
    }
}
