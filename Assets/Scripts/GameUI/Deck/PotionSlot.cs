using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PotionType
{
    HP,
    MP,
}

public class PotionSlot : MonoBehaviour
{
    public PotionType potionType;
    public Image potionIcon;
    private Item potion;
    private Button slotBtn;

    private void Init()
    {
        slotBtn = GetComponent<Button>();
    }

    private void AddBtnListener()
    {
        slotBtn.onClick.AddListener(()=>OpenPotionList());
    }

    private void OpenPotionList()
    {

    }
}
