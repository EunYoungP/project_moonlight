using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//애니메이션 레이어교체
//Animator animator = Player.Instance.m_animator;
//animator.SetLayerWeight(1, 1);
//animator.SetLayerWeight(0, 0);

public class UIDetailPage :BaseGameUI
{
    private ItemObject item;

    public Button EquipBtn;
    public Button UnEquipBtn;
    public Button CloseBtn;

    public Text m_name;
    public Text m_detailType;
    public Text m_tier;
    public Image m_icon;
    public Text weight;

    private UIEquipment uiEquipment;
    private UIInventory uiInventory;
    private InventorySlot selectedSlot;
    
    //private int gold;

    public override void Init()
    {
        uiEquipment = UIGameMng.Instance.gameObject.GetComponentInChildren<UIEquipment>(true)as UIEquipment;
        uiInventory = UIGameMng.Instance.gameObject.GetComponentInChildren<UIInventory>(true)as UIInventory;
        SlotAddListener();
    }

    public void Receive(ItemObject item, InventorySlot selectedSlot)
    {
        this.item = item;
        this.selectedSlot = selectedSlot;
        SetItemData();
        SetBtnActive();
    }

    public void SetItemData()
    {
        m_name.text = item.Name;
        m_detailType.text = item.DetailType.ToString();
        m_tier.text = item.Tier;
        m_icon.sprite = ItemDB.Instance.GetItemByName(item.Name).icon;
        weight.text = item.Weight.ToString();
    }

    private void SetBtnActive()
    {
        switch (item.ItemType)
        {
            case ItemType.Use:
                UnEquipBtn.gameObject.SetActive(false);
                EquipBtn.GetComponentInChildren<Text>().text = "사용";
                break;
            case ItemType.Food:
                UnEquipBtn.gameObject.SetActive(false);
                EquipBtn.GetComponentInChildren<Text>().text = "먹기";
                break;
            case ItemType.Ingredient:
                UnEquipBtn.gameObject.SetActive(false);
                EquipBtn.GetComponentInChildren<Text>().text = "사용";
                break;
            default:
                UnEquipBtn.gameObject.SetActive(true);
                EquipBtn.GetComponentInChildren<Text>().text = "장착";
                break;
        }
    }

    public override void Open()
    {
        base.Open();
        gameObject.SetActive(true);
        Debug.Log("상세페이지 열림");
    }

    public override void Close()
    {
        base.Close();
        gameObject.SetActive(false);
    }

    public void SlotAddListener()
    {
        EquipBtn.onClick.AddListener(() => { OnClickEquip(); });
        UnEquipBtn.onClick.AddListener(() => { OnClickUnEquip(); });
        CloseBtn.onClick.AddListener(() => { Close(); });
    }

    // 아이템 장착해제
    private void OnClickUnEquip()
    {
        // 무기가 해제상태인지 검사
        if (WeaponManager.Instance.CanUnEquip(item))
        {
            // 인벤토리에서 장착상태인지 검사 후, 장착중 표시 해제
            if (uiInventory.SetEquipState(item, false) == false)
                return;

            // 해제 아이템을 EquipSlot에서 삭제하는 부분
            uiEquipment.unEquipItem(item);
            Debug.Log("아이템 착용해제");

            // 캐릭터의 실질적 무기해제
            WeaponManager.Instance.StartWeaponUnEquip();

            UIGameMng.Instance.CloseUI(UIGameType.DetailPage);
            Debug.Log("아이템이 장착해제 되었습니다.");
        }
        else
            Debug.Log("장착된 무기가 없습니다.");
    }

    // 아이템 장착
    public void OnClickEquip()
    {
        if (CanChange())
        {
            // 장착된 아이템 인벤토리에서 장착중으로 표시
            if (uiInventory.SetEquipState(item, true) == false)
                return;

            // 장착아이템을 EquipSlot에 채우는 부분
            // uiEquipment null exception 발생
            uiEquipment.EquipItem(item);
            Debug.Log("아이템 착용");

            UIGameMng.Instance.CloseUI(UIGameType.DetailPage);

            // 실질적으로 손에 아이템을 찾아서 들게하는 코드
            // ItemDB가 아닌 DropItem::Dropitems 에서 찾아야함
            foreach (GameObject gameObject in ResourceManager.Instance.ITEM)
            {
                if (gameObject.name == item.Name)
                    ChangeEquipment(gameObject);
            }
        }
        else
        {
            Debug.Log("아이템을 장착할 수 없습니다.");
        }
    }

    // 추후 무기제외 장착품의 조건검사 추가예정
    public bool CanChange()
    {
        if (item.ItemType == ItemType.Weapon)
        {
           return WeaponManager.Instance.CanChange(item.DetailType, item.Name);
        }
        return false;
    }

    // detailType 에 따라 다른 장착법을 실행
    private void ChangeEquipment(GameObject newWeapon)
    {
        // Weapon 장착
        if (item.DetailType == DetailType.Bow
        || item.DetailType == DetailType.DoubleSword
        || item.DetailType == DetailType.MagicWand
        || item.DetailType == DetailType.TwoHandSword
        || item.DetailType == DetailType.Sword
        || item.DetailType == DetailType.Shield)
        {
            WeaponManager.Instance.StartWeaponChange(newWeapon, item.DetailType);
        }
        else
            Debug.Log("해당하는 무기가 존재하지 않습니다.");
    }
}
