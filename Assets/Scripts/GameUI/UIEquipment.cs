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

    public EquipSlot FindEqualTypeEquipSlot(ItemObject item)
    {
        if (item.DetailType == DetailType.Bow
            || item.DetailType == DetailType.DoubleSword
            || item.DetailType == DetailType.TwoHandSword
            || item.DetailType == DetailType.Sword
            || item.DetailType == DetailType.MagicWand)
        {
            return FindSlot(EquipSlotType.Weapon);
        }
        else if(item.DetailType == DetailType.Shield)
        {
            return FindSlot(EquipSlotType.Shield);
        }
        else if (item.DetailType == DetailType.Helmet)
        {
            return FindSlot(EquipSlotType.Head);
        }
        else if (item.DetailType == DetailType.Armor)
        {
            return FindSlot(EquipSlotType.Armor);
        }
        else if (item.DetailType == DetailType.Gloves)
        {
            return FindSlot(EquipSlotType.Glove);
        }
        else if (item.DetailType == DetailType.Ring)
        {
            return FindSlot(EquipSlotType.Ring);
        }
        else if (item.DetailType == DetailType.Earring)
        {
            return FindSlot(EquipSlotType.Earring);
        }
        else if (item.DetailType == DetailType.Necklace)
        {
            return FindSlot(EquipSlotType.Necklace);
        }
        else if (item.DetailType == DetailType.Cloak)
        {
            return FindSlot(EquipSlotType.Cloak);
        }
        else if (item.DetailType == DetailType.Arrow)
        {
            return FindSlot(EquipSlotType.Arrow);
        }
        else if (item.DetailType == DetailType.FishingRod)
        {
            return FindSlot(EquipSlotType.FishingRod);
        }

        Debug.Log("착용아이템이 아닙니다.");
        return null;
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
