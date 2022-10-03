using UnityEngine;
using UnityEngine.UI;

public enum NoButtonType
{
    NONE,
    DROP,
    UNEQUIP,
}

public enum YesButtonType
{
    NONE,
    EQUIP,
    DETAIL,
    EAT,
    USE,
}

public class UIDetailPage :BaseGameUI
{
    private ItemObject item;
    private InventorySlot selectedSlot;
    private UIEquipment uiEquipment;
    private UIInventory uiInventory;
    private bool isEquipItem;
    private bool isEquipTypeItem;

    public GameObject selectedItemDetailPage;
    public GameObject equipItemDetailPage;

    [Header("선택아이템 상세페이지")]
    public Button sEquipBtn;
    public Button sUnEquipBtn;
    public Button sCloseBtn;
    public Button sLockBtn;
    public Text sName;
    public Text sDetailType;
    public Text sTier;
    public Image sIcon;
    public Text sWeight;

    [Header("착용아이템 상세페이지")]
    public Text eName;
    public Text eDetailType;
    public Text eTier;
    public Image eIcon;
    public Text eWeight;

    public override void Init()
    {
        uiEquipment = UIGameMng.Instance.gameObject.GetComponentInChildren<UIEquipment>(true)as UIEquipment;
        uiInventory = UIGameMng.Instance.gameObject.GetComponentInChildren<UIInventory>(true)as UIInventory;
        InitBtnAddListener();
    }

    public bool CheckEquipEqualItem(InventorySlot selectedSlot)
    {
        if (selectedSlot.isEquipState)
        {
            Debug.Log("선택된 슬롯은 장착중인 아이템이 등록된 슬롯입니다.");
            return true;
        }
        return false;
    }

    public ItemObject CheckEquipEqualType()
    {
        EquipSlot equilSlot = uiEquipment.FindEqualTypeEquipSlot(item);
        bool isEquipEqualType = equilSlot.isItemExist;
        if (isEquipEqualType)
            return equilSlot.item;
        return null;
    }

    public void Receive(ItemObject item, InventorySlot selectedSlot)
    {
        this.item = item;
        this.selectedSlot = selectedSlot;

        ItemManager.Instance.OpenDetailPage(item,selectedSlot);

        //SetselectedItemData();
        //SetBtnActive();
    }

    public void OpenSelectedItemData()
    {
        selectedItemDetailPage.gameObject.SetActive(true);
        equipItemDetailPage.gameObject.SetActive(false);

        SetSelectedItemPage(item);

        SetBtnSelectedItem();
    }

    public void OpenEquipItemData()
    {
        selectedItemDetailPage.gameObject.SetActive(true);
        equipItemDetailPage.gameObject.SetActive(false);

        SetSelectedItemPage(item);

        SetBtnEquipItem();
    }

    public void OpenCompareItemData(ItemObject equipItem)
    {
        selectedItemDetailPage.gameObject.SetActive(true);
        equipItemDetailPage.gameObject.SetActive(true);

        SetEquipItemPage(item);
        SetEquipItemPage(equipItem);

        SetBtnSelectedItem();
    }

    private void SetSelectedItemPage(ItemObject item)
    {
        sName.text = item.Name;
        sDetailType.text = item.DetailType.ToString();
        sTier.text = item.Tier;
        sIcon.sprite = ItemDB.Instance.GetItemByName(item.Name).icon;
        sWeight.text = item.Weight.ToString();
    }

    private void SetEquipItemPage(ItemObject equipItem)
    {
        eName.text = equipItem.Name;
        eDetailType.text = equipItem.DetailType.ToString();
        eTier.text = equipItem.Tier;
        eIcon.sprite = ItemDB.Instance.GetItemByName(equipItem.Name).icon;
        eWeight.text = equipItem.Weight.ToString();
    }

    // Weapon의 버리기만 구현함
    private void SetBtnSelectedItem()
    {
        switch (item.ItemType)
        {
            case ItemType.Use:
                ChangeBtnAddListener(NoButtonType.DROP, YesButtonType.USE);
                break;
            case ItemType.Food:
                ChangeBtnAddListener(NoButtonType.DROP, YesButtonType.EAT);
                break;
            case ItemType.Ingredient:
                ChangeBtnAddListener(NoButtonType.DROP, YesButtonType.DETAIL);
                break;
            case ItemType.Quest:
                ChangeBtnAddListener(NoButtonType.NONE, YesButtonType.DETAIL);
                break;
            case ItemType.Etc:
                ChangeBtnAddListener(NoButtonType.NONE, YesButtonType.DETAIL);
                break;
            case ItemType.Accessories:
                ChangeBtnAddListener(NoButtonType.DROP, YesButtonType.EQUIP);
                break;
            case ItemType.Equipment:
                ChangeBtnAddListener(NoButtonType.DROP, YesButtonType.EQUIP);
                break;
            case ItemType.Weapon:
                ChangeBtnAddListener(NoButtonType.DROP, YesButtonType.EQUIP);
                break;
        }
    }

    private void SetBtnEquipItem()
    {
        if(item.ItemType == ItemType.Accessories
            || item.ItemType == ItemType.Equipment
            || item.ItemType == ItemType.Weapon)
        {
            ChangeBtnAddListener(NoButtonType.UNEQUIP, YesButtonType.NONE);
        }
        else
        {
            Debug.Log("장착아이템이 아닙니다.");
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

    public void InitBtnAddListener()
    {
        sEquipBtn.onClick.AddListener(() => { OnClickEquip(); });
        sUnEquipBtn.onClick.AddListener(() => { OnClickUnEquip(); });
        sCloseBtn.onClick.AddListener(() => { Close(); });
    }

    private void ChangeBtnAddListener(NoButtonType nBtnType, YesButtonType yBtnType)
    {
        sUnEquipBtn.gameObject.SetActive(true);
        sEquipBtn.gameObject.SetActive(true);

        switch (nBtnType)
        {
            case NoButtonType.DROP:
                sUnEquipBtn.onClick.RemoveAllListeners();
                sUnEquipBtn.onClick.AddListener(() => OnClickDrop());
                sUnEquipBtn.GetComponentInChildren<Text>().text = "버리기";
                break;
            case NoButtonType.UNEQUIP:
                sUnEquipBtn.onClick.RemoveAllListeners();
                sUnEquipBtn.onClick.AddListener(() => OnClickUnEquip());
                sUnEquipBtn.GetComponentInChildren<Text>().text = "벗기";
                break;
            case NoButtonType.NONE:
                sUnEquipBtn.gameObject.SetActive(false);
                break;
        }

        // 임시로 전부 Equip 상태로 변경
        switch (yBtnType)
        {
            case YesButtonType.DETAIL:
                sEquipBtn.onClick.RemoveAllListeners();
                sEquipBtn.onClick.AddListener(() => OnClickEquip());
                sEquipBtn.GetComponentInChildren<Text>().text = "상세보기";
                break;
            case YesButtonType.EAT:
                sEquipBtn.onClick.RemoveAllListeners();
                sEquipBtn.onClick.AddListener(() => OnClickEquip());
                sEquipBtn.GetComponentInChildren<Text>().text = "먹기";
                break;
            case YesButtonType.EQUIP:
                sEquipBtn.onClick.RemoveAllListeners();
                sEquipBtn.onClick.AddListener(() => OnClickEquip());
                sEquipBtn.GetComponentInChildren<Text>().text = "착용";
                break;
            case YesButtonType.USE:
                sEquipBtn.onClick.RemoveAllListeners();
                sEquipBtn.onClick.AddListener(() => OnClickUse());
                sEquipBtn.GetComponentInChildren<Text>().text = "사용";
                break;
            case YesButtonType.NONE:
                sEquipBtn.gameObject.SetActive(false);
                break;
        }
    }

    #region NBtnEvent
    // 아이템 버리기
    private void OnClickDrop()
    {
        if (uiInventory.SetEquipState(item, selectedSlot, false) == true)
            return;

        uiInventory.RemoveItem(item);

        UIGameMng.Instance.CloseUI(UIGameType.DetailPage);
        Debug.Log("아이템이 장착해제 되었습니다.");
    }

    // 아이템 장착해제
    private void OnClickUnEquip()
    {
        // 무기가 해제상태인지 검사
        if (WeaponManager.Instance.CanUnEquip(item))
        {
            // 인벤토리에서 장착상태인지 검사 후, 장착중 표시 해제
            if (uiInventory.SetEquipState(item, selectedSlot, false) == false)
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
    #endregion

    #region YBynEvent
    // 아이템 장착
    public void OnClickEquip()
    {
        if (CanChange())
        {
            // 장착된 아이템 인벤토리에서 장착중으로 표시
            if (uiInventory.SetEquipState(item, selectedSlot, true) == false)
                return;

            // 장착아이템을 EquipSlot에 채우는 부분
            uiEquipment.EquipItem(item, selectedSlot);
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

    private void OnClickUse()
    {
        //ItemManager.Instance.
    }
    #endregion

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
