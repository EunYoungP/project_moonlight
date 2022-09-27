using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : ItemBase
{
    public override void OpenDetailPage(ItemObject item, InventorySlot invenSlot)
    {
        UIDetailPage detailPage = UIGameMng.Instance.GetUI<UIDetailPage>(UIGameType.DetailPage);
        bool isEquip = detailPage.CheckEquipEqualItem(invenSlot);
        if(isEquip)
        {
            detailPage.OpenEquipItemData();
        }
        else if(!isEquip)
        {
            ItemObject equipItem = detailPage.CheckEquipEqualType();
            if (equipItem != null)
            {
                detailPage.OpenCompareItemData(equipItem);
            }
            else if(equipItem == null)
            {
                detailPage.OpenSelectedItemData();
            }
        }
    }
}
