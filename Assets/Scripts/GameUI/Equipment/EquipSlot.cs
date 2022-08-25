using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EquipSlot : MonoBehaviour
{
    private ItemObject item;
    public Image Icon;
    public Image bkIcon;
    public Button slotBtn;

    private bool itemExist;

    public EquipSlotType equipType;

    public void AddEquipItem(ItemObject equipItem)
    {
        if (Icon == null)
            return;

        item = equipItem;
        itemExist = true;

        bkIcon.enabled = false;
        Icon.enabled = true;
        Icon.sprite = ItemDB.Instance.GetItemByName(item.Name).icon;
    }

    public void RemoveEquipItem(ItemObject unequipItem)
    {
        if (Icon == null)
            return;

        item = unequipItem;
        itemExist = false;

        bkIcon.enabled = true;
        Icon.enabled = false;
    }

    public void ClearSlot()
    {
        item = null;
        itemExist = false;

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
        if (!itemExist)
            return;

        UIGameMng.Instance.OpenUI<UIDetailPage>(UIGameType.DetailPage);
        UIDetailPage detailPage = GameObject.FindObjectOfType<UIDetailPage>();
        detailPage.SendMessage("Receive", item, SendMessageOptions.DontRequireReceiver);
    }
}
