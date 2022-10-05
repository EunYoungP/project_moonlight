using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionDeck : BaseDeck
{
    private PotionSlot[] potionSlots;

    public override void Init()
    {
        potionSlots = GetComponentsInChildren<PotionSlot>(true);
    }
}
