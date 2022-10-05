using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Use : ItemBase
{
    private Character player;
    private int HpPotionValue = 400;
    private int MpPotionValue = 250;

    public override void OpenDetailPage(ItemObject item, InventorySlot invenSlot)
    {
        UIGameMng.Instance.GetUI<UIDetailPage>(UIGameType.DetailPage).OpenSelectedItemData();
    }

    public override void UseItem(ItemObject item)
    {
        switch(item.DetailType)
        {
            case DetailType.Potion:
                UsePotion(item);
                break;
            case DetailType.Scroll:
                break;
        }
    }

    private void UsePotion(ItemObject potion)
    {
        // HP물약
        if(potion.ItemID == 17)
        {
            UseHpPotion(potion);
        }
        // MP물약
        else if(potion.ItemID == 18)
        {
            UseMpPotion(potion);
        }
    }

    private void UseHpPotion(ItemObject potion)
    {
        player = Player.Instance.data;
        if(player.curHp == player.maxHp)
        {
            Debug.Log("현재 Hp가 꽉 차있습니다.");
        }
        else if(player.curHp < player.maxHp)
        {
            Player.Instance.data.IncreaseHp(HpPotionValue);
            UIGameMng.Instance.GetUI<UIInventory>(UIGameType.Inventory).RemoveItem(potion);
            UIGameMng.Instance.CloseUI(UIGameType.DetailPage);
        }
    }

    private void UseMpPotion(ItemObject potion)
    {
        player = Player.Instance.data;
        if (player.curMp == player.maxMp)
        {
            Debug.Log("현재 Mp가 꽉 차있습니다.");
        }
        else if (player.curMp < player.maxMp)
        {
            Player.Instance.data.IncreaseMp(MpPotionValue);
            UIGameMng.Instance.GetUI<UIInventory>(UIGameType.Inventory).RemoveItem(potion);
            UIGameMng.Instance.CloseUI(UIGameType.DetailPage);
        }
    }
}
