using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1. 각 무기 스크립트 완성
// 2021.04.07 무기교체시 무기객체가 늦게 손에들림

public enum WeaponState
{
    SwordShield,
    Bow,
    DoubleSword,
    MagicWand,
    NoWeapon,
    TwoHandSword
}

// Player - root에 붙여놓음
public class WeaponManager : MonoBehaviour
{
    private bool isChangeWeapon = false;
    private bool isUnEquipWeapon = false;

    // 무기가 장착될 위치
    public Transform weaponPosR;
    public Transform weaponPosL;
    private Transform shiledPos;

    // 무기
    public PickUpItem currWeaponR;
    public PickUpItem currWeaponL;
    public Animator currWeaponAnim;
    public WeaponState currWeaponType;
    public WeaponState newWeaponType;
    public GameObject newWeapon;

    // 임의로 설정
    private float changeWeaponDelayTime = 0.4f;
    private float changeWeaponEndDelayTime = 0.57f;

    private List<Item> Weapons = new List<Item>();
    private Dictionary<WeaponState, WeaponController> WeaponControllerDic = new Dictionary<WeaponState, WeaponController>();

    private static WeaponManager isntance;
    public static WeaponManager Instance
    {
        get
        {
            if (isntance == null)
            {
                isntance = FindObjectOfType<WeaponManager>();
            }
            return isntance;
        }
    }

    // GameMng 에서 실행
    public void Init()
    {
        currWeaponAnim = GetComponent<Animator>();

        weaponPosR = GetComponentInChildren<WeaponPosR>().transform;
        weaponPosL = GetComponentInChildren<WeaponPosL>().transform;
        shiledPos = GetComponentInChildren<ShieldPos>().transform;
        
        AddWeaponController<SwordShieldController>(WeaponState.SwordShield);
        AddWeaponController<BowController>(WeaponState.Bow);
        AddWeaponController<DoubleSwordController>(WeaponState.DoubleSword);
        AddWeaponController<MagicWandController>(WeaponState.MagicWand);
        AddWeaponController<NoWeaponController>(WeaponState.NoWeapon);
        AddWeaponController<TwoHandSwordController>(WeaponState.TwoHandSword);

        currWeaponR = weaponPosR.GetComponentInChildren<PickUpItem>();
        currWeaponL = weaponPosL.GetComponentInChildren<PickUpItem>();
        currWeaponType = WeaponState.NoWeapon;
    }

    private void AddWeaponController<T>(WeaponState weaponType) where T : WeaponController
    {
        if(!WeaponControllerDic.ContainsKey(weaponType))
        {
            T t = GetComponentInChildren<T>();

            if (t == null)
                return;

            WeaponControllerDic.Add(weaponType,t);
            WeaponControllerDic[weaponType].Init();
        }
    }

    // UIDetailPage의 장착 버튼을 누르면 실행
    public bool CanChange(DetailType newWeaponType, string newItemName)
    {
        // 무기 교환중인지 체크
        if (!isChangeWeapon && !isUnEquipWeapon)
        {
            switch (newWeaponType)
        {
            case DetailType.DoubleSword:
                if (currWeaponType == WeaponState.DoubleSword)
                    return WeaponControllerDic[WeaponState.DoubleSword].CanChange(newWeaponType, newItemName);
                else if (currWeaponType != WeaponState.DoubleSword)
                    return true;
                break;
            case DetailType.TwoHandSword:
                if (currWeaponType == WeaponState.TwoHandSword)
                    return WeaponControllerDic[WeaponState.TwoHandSword].CanChange(newWeaponType, newItemName);
                else if (currWeaponType != WeaponState.TwoHandSword)
                    return true;
                break;
            case DetailType.MagicWand:
                if (currWeaponType == WeaponState.MagicWand)
                    return WeaponControllerDic[WeaponState.MagicWand].CanChange(newWeaponType, newItemName);
                else if (currWeaponType != WeaponState.MagicWand)
                    return true;
                break;
            case DetailType.Bow:
                if (currWeaponType == WeaponState.Bow)
                    return WeaponControllerDic[WeaponState.Bow].CanChange(newWeaponType, newItemName);
                else if (currWeaponType != WeaponState.Bow)
                    return true;
                break;
            case DetailType.Sword:
                if (currWeaponType == WeaponState.DoubleSword)
                {
                    return WeaponControllerDic[WeaponState.DoubleSword].CanChange(newWeaponType, newItemName);
                }
                else if (currWeaponType != WeaponState.DoubleSword)
                    return true;
                break;
            case DetailType.Shield:
                if (currWeaponType == WeaponState.DoubleSword)
                    return WeaponControllerDic[WeaponState.DoubleSword].CanChange(newWeaponType, newItemName);
                else if (currWeaponType != WeaponState.DoubleSword)
                    return true;
                break;
        }
        }
        // 바꿀무기와 현재무기가 같은무기인지 검사
        return false;
    }

    // 현재무기가 이미 NoWeapon 상태인지 검사
    public bool CanUnEquip(ItemObject weapon)
    {
        if (!isChangeWeapon && !isUnEquipWeapon)
        {
            if (currWeaponType != WeaponState.NoWeapon)
                return true;
        }
        return false;
    }

    // 다른 스크립트에서 코루틴 시작되게 하기위해 만든 함수
    public void StartWeaponChange(GameObject newWeapon, DetailType newWeaponType)
    {
        this.newWeapon = newWeapon;

        switch (newWeaponType)
        {
            case DetailType.DoubleSword:
                this.newWeaponType = WeaponState.DoubleSword;
                break;
            case DetailType.TwoHandSword:
                this.newWeaponType = WeaponState.TwoHandSword;
                break;
            case DetailType.MagicWand:
                this.newWeaponType = WeaponState.MagicWand;
                break;
            case DetailType.Bow:
                this.newWeaponType = WeaponState.Bow;
                break;
            case DetailType.Sword:
                this.newWeaponType = WeaponState.SwordShield;
                break;
            case DetailType.Shield:
                this.newWeaponType = WeaponState.SwordShield;
                break;
        }

        StartCoroutine(ChangeWeaponCoroutine());
    }

    // 무기교체과정 코루틴
    private IEnumerator ChangeWeaponCoroutine()
    {
        isChangeWeapon = true;
        
        Player.Instance.playerController.animator.SetTrigger("WeaponChange");
        // 현재는 DoubleSword Sound 만 나오지만, 추후 무기별로 SFX 추가
        SoundManager.Instance.Play("Weapon/AWP_Dagger_UnSheath 1");

        yield return new WaitForSeconds(changeWeaponDelayTime);

        ChangeWeaponAction();

        isChangeWeapon = false;
    }

    // 각무기 실질적 무기교체 실행함수
    private void ChangeWeaponAction()
    {
        switch(newWeaponType)
        {
            case WeaponState.DoubleSword:
                WeaponControllerDic[WeaponState.DoubleSword].ChangeWeapon();
                currWeaponType = WeaponState.DoubleSword;
                return;
            case WeaponState.TwoHandSword:
                WeaponControllerDic[WeaponState.TwoHandSword].ChangeWeapon();
                currWeaponType = WeaponState.TwoHandSword;
                return;
            case WeaponState.MagicWand:
                WeaponControllerDic[WeaponState.MagicWand].ChangeWeapon();
                currWeaponType = WeaponState.MagicWand;
                return;
            case WeaponState.Bow:
                WeaponControllerDic[WeaponState.Bow].ChangeWeapon();
                currWeaponType = WeaponState.Bow;
                return;
            case WeaponState.SwordShield:
                WeaponControllerDic[WeaponState.SwordShield].ChangeWeapon();
                currWeaponType = WeaponState.SwordShield;
                return;
        }
    }

    // 무기 해제 루틴
    public void StartWeaponUnEquip()
    {
        this.newWeapon = null;
        newWeaponType = WeaponState.NoWeapon;

        StartCoroutine(SetNoWeapon());
    }

    private IEnumerator SetNoWeapon()
    {
        isUnEquipWeapon = true;

        Player.Instance.playerController.animator.SetTrigger("WeaponChange");

        yield return new WaitForSeconds(changeWeaponDelayTime);

        WeaponControllerDic[newWeaponType].ChangeWeapon();

        currWeaponType = WeaponState.NoWeapon;
        isUnEquipWeapon = false;
    }
}
