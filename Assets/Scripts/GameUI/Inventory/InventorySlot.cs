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
    public int slotIndex;

    public void Init()
    {
        GameObject UIGameMngObj = GameObject.Find("UIGame(Clone)");
        GameObject inventory = UIGameMngObj.transform.Find("Inventory").gameObject;
        invenDeck = inventory.GetComponent<UIInventory>().InventoryDeck;
        SlotAddListener();
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
        //SlotAddListener();
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
        // 모든 인벤토리 슬롯
        InventorySlot[] invenSlots = UIGameMng.Instance.GetUI<UIInventory>(UIGameType.Inventory).InventorySlots;
        // 모든 인벤토리 슬롯 검색
        if(isShow)
        {
            foreach (InventorySlot invenSlot in invenSlots)
            {
                if (invenSlot.slotIndex == this.slotIndex)
                {
                    invenSlot.SelectedImg.SetActive(isShow);
                }
                else
                {
                    invenSlot.SelectedImg.SetActive(false);
                }
            }
        }
        else if(!isShow)
        {
            foreach (InventorySlot invenSlot in invenSlots)
            {
                invenSlot.SelectedImg.SetActive(isShow);
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
