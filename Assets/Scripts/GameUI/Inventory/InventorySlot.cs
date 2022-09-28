using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public ItemObject item;

    public Image Icon;
    public GameObject EquipImg;
    private Button slotBtn;

    public bool isItemExist;
    public bool isEquipState;

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
    }

    public void SlotAddListener()
    {
        slotBtn = GetComponentInChildren<Button>();
        slotBtn.onClick.AddListener(() => { OnClickSlot(); });
    }

    public void OnClickSlot()
    {
        if (!isItemExist)
            return;

        UIGameMng.Instance.OpenUI<UIDetailPage>(UIGameType.DetailPage);
        UIDetailPage detailPage = GameObject.FindObjectOfType<UIDetailPage>();
        //detailPage.SendMessage("Receive", item, SendMessageOptions.DontRequireReceiver);
        detailPage.Receive(item, gameObject.GetComponent<InventorySlot>());
        Debug.Log("상세페이지 열림");
    }

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
}
