﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject
{
    public int IdentifyID { get; set; }
    public int ItemID { get; set; }
    public string ModelName { get; set; }
    public string IconName { get; set; }
    public ItemType ItemType { get; set; }
    public DetailType DetailType { get; set; }
    public string Name { get; set; }
    public int Weight { get; set; }
    public string Grade { get; set; }
    public string Tier { get; set; }
    public bool Bind { get; set; }

    public ItemObject(Item item, int identifyID)
    {
        IdentifyID = identifyID;
        ItemID = item.itemID;
        ModelName = item.modelName;
        IconName = item.iconName;
        ItemType = item.itemType;
        DetailType = item.detailType;
        Name = item.name;
        Weight = item.weight;
        Grade = item.grade;
        Tier = item.tier;
        Bind = item.bind;
    }

    public ItemObject() { }
}
