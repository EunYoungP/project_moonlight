using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    private ItemObject equipItem;
    public Image itemIcon;
    public Button slotBtn;
    public int slotIndex;

    public bool isEmpty;

    public void Init()
    {
        isEmpty = true;
    }

    public void EquipItem(ItemObject equipItem)
    {
        this.equipItem = equipItem;
        itemIcon.gameObject.SetActive(true);
        foreach (Sprite itemIcon in ResourceManager.Instance.ITEMICON)
        {
            if (itemIcon.name == equipItem.IconName)
                this.itemIcon.sprite = itemIcon;
        }

        isEmpty = false;
    }

    public void UnEquipItem()
    {
        equipItem = null;
        itemIcon.sprite = null;
        itemIcon.gameObject.SetActive(false);
        isEmpty = true;
    }
}
