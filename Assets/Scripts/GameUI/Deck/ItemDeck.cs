using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDeck : BaseDeck
{
    private ItemSlot[] itemSlots;

    public override void Init()
    {
        itemSlots = GetComponentsInChildren<ItemSlot>();
        SetSlotIndex();
    }

    private void SetSlotIndex()
    {
        for (int i = 0; i < itemSlots.Length; ++i)
        {
            itemSlots[i].slotIndex = i;
            itemSlots[i].Init();
        }
    }

    public void EquipItemDeck(ItemObject equipItem,int slotIndex)
    {
        foreach(ItemSlot itemSlot in itemSlots)
        {
            if (itemSlot.slotIndex == slotIndex)
                itemSlot.EquipItem(equipItem);
        }
    }

    public void UnEquipItemDeck(int slotIndex)
    {
        foreach (ItemSlot itemSlot in itemSlots)
        {
            if (itemSlot.slotIndex == slotIndex)
            {
                itemSlot.UnEquipItem();
            }
        }
    }

}
