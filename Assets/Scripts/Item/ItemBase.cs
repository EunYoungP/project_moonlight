﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    public abstract void OpenDetailPage(ItemObject item, InventorySlot invenSlot);
}
