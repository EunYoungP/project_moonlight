using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.IO;
using System;

public enum ItemType
{
    Equipment,
    Use,
    Food,
    Ingredient,
    Etc,
    Quest,
    Accessories,
    Weapon,
}

public enum DetailType
{
    //Weapon
    Bow,
    DoubleSword,
    TwoHandSword,
    Sword,
    Shield,
    MagicWand,
    Arrow,

    //Equipment
    Helmet,
    Armor,
    Gloves,
    Cloak,

    //Accessories
    Ring,
    Necklace,
    Earring,

    Food,
    Loot,
    Statue,
    Potion,
    Scroll,
}

// itemInfo 를 가지고있는 클래스
public class Item 
{
    public int itemID;
    public string modelName;
    public string iconName;
    public ItemType itemType;
    public DetailType detailType;
    public string name;
    public int weight;
    public string grade;
    public string tier;
    public bool bind;

    public Sprite icon;
    public bool isLock;
    //public int identifyID;

    // subject늘어나면 일일이 추가해 주어야하는 부분
    public void ReadItem(List<string> list)
    {
        //List<string> sortList = new List<string>();
        //SortString(list);

        this.itemID = int.Parse(list[0]);
        this.modelName = list[1];
        this.iconName = list[2];
        this.itemType = (ItemType)Enum.Parse(typeof(ItemType), list[3]);
        this.detailType = (DetailType)Enum.Parse(typeof(DetailType), list[4]);
        this.name = list[5];
        this.weight = int.Parse(list[6]);
        this.grade = list[7];
        this.tier = list[8];
        this.bind = bool.Parse(list[9]);

        // 2020.07.16 
        // 아래의 코드가 동작하지 않는 이유
        // 리소스 매니저의 아이콘을 읽어들이는 코드보다 먼저 호출되었기 때문에
        // 아이콘이 null값으로 설정되었음.
        //foreach (Sprite image in ResourceManager.Instance.ITEMICON)
        //{
        //    if (image == null)
        //        continue;

        //    if (iconName == image.name)
        //    {
        //        icon = image;
        //    }
        //}
    }

    //public List<string> SortString(List<string> list)
    //{
    //    for(int i = 0; i<list.Count; ++i)
    //    {
    //        string value = string.Format("{0},{1},{2}", list[0], list[1], list[2]);
    //        var val = value.Split(',');
    //    }
    //}
}
