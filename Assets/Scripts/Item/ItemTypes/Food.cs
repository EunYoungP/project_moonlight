using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : ItemBase
{
    public override void OpenDetailPage(ItemObject item, InventorySlot invenSlot)
    {
        UIGameMng.Instance.GetUI<UIDetailPage>(UIGameType.DetailPage).OpenSelectedItemData();
    }
}
