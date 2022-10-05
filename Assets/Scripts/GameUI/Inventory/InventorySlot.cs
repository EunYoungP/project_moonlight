using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public ItemObject item;
    private InventoryDeck invenDeck;

    public Image Icon;
    public GameObject EquipImg;
    public GameObject SelectedImg;
    private Button slotBtn;

    public bool isItemExist;
    public bool isEquipState;
    public bool isSelectedState;

    // stackoverflow error
    public void Init()
    {
        invenDeck = GameObject.Find("InventoryDeck").GetComponent<InventoryDeck>();
    }

    public void AddItem(ItemObject newItem)
    {
        item = newItem;
        isItemExist = true;

        Item itemData = ItemDB.Instance.GetItemByName(newItem.Name);

        if(Icon != null )
        {
            Icon.enabled = true;
            Icon.sprite = itemData.icon;
        }
        SlotAddListener();
    }

    public void ClearSlot()
    {
        item = null;
        isItemExist = false;

        if( Icon != null )
            Icon.enabled = false;

        SetUnEquipState();
    }

    #region 버튼 이벤트

    public void SlotAddListener()
    {
        slotBtn = GetComponentInChildren<Button>();
        slotBtn.onClick.AddListener(() => { OnClickSlot(); });
    }

    private void OnClickSlot()
    {
        SetSelectedUI(true);
        OpenDetail();
        SetWaitEquipState();
    }

    public void SetSelectedUI(bool isShow)
    {
        InventorySlot[] invenSlots = UIGameMng.Instance.GetUI<UIInventory>(UIGameType.Inventory).InventorySlots;
        foreach(InventorySlot invenSlot in invenSlots)
        {
            if(invenSlot == this)
            {
                SelectedImg.SetActive(isShow);
            }
            else
            {
                SelectedImg.SetActive(false);
            }
        }
    }

    private void OpenDetail()
    {
        if (!isItemExist)
            return;

        UIGameMng.Instance.OpenUI<UIDetailPage>(UIGameType.DetailPage);
        UIDetailPage detailPage = GameObject.FindObjectOfType<UIDetailPage>();
        detailPage.Receive(item, gameObject.GetComponent<InventorySlot>());
        Debug.Log("상세페이지 열림");
    }

    private void SetWaitEquipState()
    {
        if (!isItemExist)
            return;

        invenDeck.WaitForEquipItem(item,this);
    }

    #endregion

    #region 장착중표시 제어
    public void SetEquipState()
    {
        EquipImg.SetActive(true);
        isEquipState = true;
    }

    public void SetUnEquipState()
    {
        EquipImg.SetActive(false);
        isEquipState = false;
    }
    #endregion
}
