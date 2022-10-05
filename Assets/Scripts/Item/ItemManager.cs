using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private Dictionary<ItemType, ItemBase> itemDic = new Dictionary<ItemType, ItemBase>();
    private static ItemManager instance;
    public static ItemManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject("ItemManager", typeof(ItemManager));
                obj.gameObject.transform.parent = GameMng.Instance.transform.parent;
                DontDestroyOnLoad(obj);

                instance = obj.GetComponent<ItemManager>();
            }
            return instance;
        }
    }

    public void Init()
    {
        AddItemDic<Use>(ItemType.Use);
        AddItemDic<QuestItem>(ItemType.Quest);
        AddItemDic<Ingredient>(ItemType.Ingredient);
        AddItemDic<Etc>(ItemType.Etc);
        AddItemDic<Equipment>(ItemType.Equipment);
        AddItemDic<Weapon>(ItemType.Weapon);
        AddItemDic<Accessories>(ItemType.Accessories);
        AddItemDic<Food>(ItemType.Food);
    }

    private void AddItemDic<T>(ItemType itemType)where T :ItemBase
    {
        if(!itemDic.ContainsKey(itemType))
        {
            GameObject obj = new GameObject(itemType.ToString(), typeof(T));
            T t = obj.GetComponent<T>();
            itemDic.Add(itemType, t);
        }
    }

    public void OpenDetailPage(ItemObject item, InventorySlot invenSlot)
    {
        if(!itemDic.ContainsKey(item.ItemType))
        {
            Debug.Log("딕셔너리에 존재하지 않는 키값입니다.");
        }
        itemDic[item.ItemType].OpenDetailPage(item, invenSlot);
    }

    public void UseItem<T>(ItemType itemType,ItemObject useitem) where T : ItemBase
    {
        if(!itemDic.ContainsKey(itemType))
        {
            AddItemDic<T>(itemType);
        }

        itemDic[itemType].UseItem(useitem);
    }
}
