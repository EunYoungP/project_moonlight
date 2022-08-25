using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipSlotType
{
    Weapon,
    Shield,
    Head,
    Armor,
    Glove,
    Ring,
    Earring,
    Necklace,
    Cloak,
    Arrow,
    FishingRod,
}

public class UIEquipment : BaseGameUI
{
    EquipSlot[] equipSlots;
    List<ItemObject> equipItems = new List<ItemObject>();

    ItemObject equipItem;

    public override void Init()
    {
        equipSlots = GetComponentsInChildren<EquipSlot>();
        
        foreach(EquipSlot equipSlot in equipSlots)
        {
            equipSlot.SlotAddListener();
        }
    }

    public override void Open()
    {
        base.Open();
        gameObject.SetActive(true);
    }

    public override void Close()
    {
        base.Close();
        gameObject.SetActive(false);
    }

    // equipItem 에 맞는 equipSlot을 찾아주는 함수
    public EquipSlot FindSlot(EquipSlotType equipSlotType)
    {
        EquipSlot equipSlot = null;

        for(int i = 0; i<equipSlots.Length; i++)
        {
            if(equipSlots[i].equipType.Equals(equipSlotType))
            {
                equipSlot =  equipSlots[i];
            }
        }
        return equipSlot;
    }

    public void EquipItem(ItemObject item)
    {
        equipItem = item;
        equipItems.Add(item);

        if (equipItem.ItemType == ItemType.Weapon)
        {
            FindSlot(EquipSlotType.Weapon).AddEquipItem(equipItem);
        }
    }

    public void unEquipItem(ItemObject unequipItem)
    {
        equipItems.Remove(unequipItem);
        if (unequipItem.ItemType == ItemType.Weapon)
        {
            FindSlot(EquipSlotType.Weapon).RemoveEquipItem(unequipItem);
        }
        equipItem = null;
    }
}
