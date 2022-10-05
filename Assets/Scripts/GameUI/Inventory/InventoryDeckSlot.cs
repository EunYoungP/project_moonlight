using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDeckSlot : MonoBehaviour
{
    public ItemObject EquipItem { get { return equipItem; } }
    private ItemObject equipItem;

    public Image itemIcon;
    public GameObject plusImg;
    public GameObject minusImg;

    public int slotIndex { get; set; }
    private bool isEmpty;

    public float scalePulseAmount = 0.04f;
    public float scalePulseSpeed = 10f;
    public float InitScale = 1.0f;
    private float currScale;
    private bool isCanPulse;

    public void Init()
    {
        isEmpty = true;
    }

    public void SetDeckSlotPulse(bool isWaitEquip)
    {
        SetImg(isWaitEquip);
        ScalePulse(isWaitEquip);
    }

    private void SetImg(bool isWaitEquip)
    {
        // 장착 대기 상태
        if (isWaitEquip)
        {
            if (isEmpty)
            {
                minusImg.SetActive(false);
                plusImg.SetActive(true);
            }
            else if (!isEmpty)
            {
                minusImg.SetActive(false);
                plusImg.SetActive(false);
            }
        }
        if (!isWaitEquip)
        {
            if (isEmpty)
            {
                plusImg.SetActive(false);
                minusImg.SetActive(false);
            }
            else if (!isEmpty)
            {
                minusImg.SetActive(true);
                plusImg.SetActive(false);
            }
        }
    }

    private void ScalePulse(bool isWaitEquip)
    {
        isCanPulse = isWaitEquip;
        currScale = InitScale;
        StartCoroutine(CoScalePulse());
    }

    private IEnumerator CoScalePulse()
    {
        while(isCanPulse)
        {
            currScale = Mathf.Sin(Time.time * scalePulseSpeed) * (scalePulseAmount) + InitScale;
            gameObject.transform.localScale = Vector3.one * currScale;
            yield return null;
        }
    }

    public bool ClickDeckSlot(bool isWaitEquip, ItemObject selectedItem)
    {
        // 스킬장착 대기상태
        if (isWaitEquip)
        {
            EquipInvenDeck(selectedItem);
            return true;
        }
        // 스킬해제 대기상태
        else if (!isWaitEquip)
        {
            if (!isEmpty)
            {
                UnEquipInvenDeck();
            }
        }
        return false;
    }

    public void EquipInvenDeck(ItemObject item)
    {
        isEmpty = false;
        equipItem = item;
        itemIcon.gameObject.SetActive(true);
        
        foreach(Sprite itemIcon in ResourceManager.Instance.ITEMICON)
        {
            if (itemIcon.name == item.IconName)
                this.itemIcon.sprite = itemIcon;
        }

        UIGameMng.Instance.CloseUI(UIGameType.DetailPage);

        // 해당하는 인벤토리 아이템의 장착상태 변경 가능 시점
    }

    public void UnEquipInvenDeck()
    {
        isEmpty = true;
        equipItem = null;
        itemIcon.sprite = null;
        itemIcon.gameObject.SetActive(false);

        // 해당하는 인벤토리 아이템의 장착상태 변경 가능 시점
    }
}
