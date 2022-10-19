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
    FishingRod,
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

    // subject늘어나면 추가해 주어야하는 부분
    public void ReadItem(List<string> list)
    {
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
    }
}
