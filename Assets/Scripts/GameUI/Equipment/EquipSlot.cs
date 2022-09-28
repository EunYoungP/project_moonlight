using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EquipSlot : MonoBehaviour
{
    public ItemObject item { get; set; }
    public Image Icon;
    public Image bkIcon;
    public Button slotBtn;

    public bool isItemExist { get; set; }

    public EquipSlotType equipType;
    private InventorySlot inventorySlot;

    public void AddEquipItem(ItemObject equipItem, InventorySlot invenSlot)
    {
        if (Icon == null)
            return;

        item = equipItem;
        inventorySlot = invenSlot;
        isItemExist = true;

        bkIcon.enabled = false;
        Icon.enabled = true;
        Icon.sprite = ItemDB.Instance.GetItemByName(item.Name).icon;
    }

    public void RemoveEquipItem(ItemObject unequipItem)
    {
        if (Icon == null)
            return;

        item = unequipItem;
        isItemExist = false;

        bkIcon.enabled = true;
        Icon.enabled = false;
    }

    public void ClearSlot()
    {
        item = null;
        isItemExist = false;

        if (Icon != null)
        {
            Icon.enabled = false;
            bkIcon.enabled = true;
        }
    }

    public void SlotAddListener()
    {
        slotBtn.onClick.AddListener(() => { OnClickSlot(); });
    }

    public void OnClickSlot()
    {
        if (!isItemExist)
            return;

        UIGameMng.Instance.OpenUI<UIDetailPage>(UIGameType.DetailPage);
        UIDetailPage detailPage = GameObject.FindObjectOfType<UIDetailPage>();
        //detailPage.SendMessage("Receive", item, SendMessageOptions.DontRequireReceiver);
        detailPage.Receive(item, inventorySlot);
    }
}
