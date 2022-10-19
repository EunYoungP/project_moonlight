using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InventoryDeck : MonoBehaviour
{
    private InventoryDeckSlot[] inventoryDeckSlots;

    public Button PageBtn;
    public Transform SlotParent;
    public Transform firstPagePos;
    public Transform secondPagePos;
    public ItemDeck itemDeck;

    private ItemObject selectedItem;
    private InventorySlot selectedSlot;

    private bool isStartPage = true;
    private bool isWaitEquip;
    private float slideTime;
    private Text pageText;

    public void Init()
    {
        inventoryDeckSlots = GetComponentsInChildren<InventoryDeckSlot>();
        SetSlotIndex();
        InitSlot();
        InitDeckPos();
    }

    private void SetSlotIndex()
    {
        for (int i = 0; i < inventoryDeckSlots.Length; ++i)
        {
            inventoryDeckSlots[i].slotIndex = i;
        }
    }

    private void InitDeckPos()
    {
        SlotParent.transform.position = firstPagePos.transform.position;
        SetPageText();
    }

    private void InitSlot()
    {
        foreach(InventoryDeckSlot invenDeckSlot in inventoryDeckSlots)
        {
            invenDeckSlot.Init();
            invenDeckSlot.GetComponent<Button>().onClick.AddListener(() => { ClickDeckSlot(invenDeckSlot); });
        }
    }

    // 인벤토리안의 아이템 클릭시 실행
    public void WaitForEquipItem(ItemObject selectedItem, InventorySlot selectedSlot)
    {
        isWaitEquip = true;
        this.selectedItem = selectedItem;
        this.selectedSlot = selectedSlot;

        foreach (InventoryDeckSlot invenDeckSlot in inventoryDeckSlots)
        {
            invenDeckSlot.SetDeckSlotPulse(isWaitEquip);
        }
    }

    public void SetNotEquipState()
    {
        foreach(InventoryDeckSlot invenDeckSlot in inventoryDeckSlots)
        {
            invenDeckSlot.SetDeckSlotPulse(false);
        }
    }

    // 인벤토리 덱슬롯 클릭시 실행
    private void ClickDeckSlot(InventoryDeckSlot selectedDeckSlot)
    {
        // 아이템장착 대기상태
        if (isWaitEquip)
        {
            selectedDeckSlot.EquipInvenDeck(selectedItem);
            itemDeck.EquipItemDeck(selectedItem, selectedDeckSlot.slotIndex);
            selectedSlot.SetSelectedUI(false);
            isWaitEquip = false;
        }
        // 아이템 장착해제 대기상태
        else if(!isWaitEquip)
        {
            selectedDeckSlot.UnEquipInvenDeck();
            itemDeck.UnEquipItemDeck(selectedDeckSlot.slotIndex);
        }

        foreach (InventoryDeckSlot invenDeckSlot in inventoryDeckSlots)
        {
            invenDeckSlot.SetDeckSlotPulse(false);
        }
    }

    // 페이지버튼에 연결된 함수
    public void ChangePage()
    {
        slideTime = 0.3f;
        StartCoroutine("CoChangePage");
    }

    // 버튼을 누르면 다른페이지로 바꿔주는 기능
    IEnumerator CoChangePage()
    {
        float elapsedTime = 0f;
        isStartPage = SlotParent.transform.position == firstPagePos.transform.position ? true : false;
        if (isStartPage)
        {
            while (true)
            {
                elapsedTime += Time.deltaTime / slideTime;
                elapsedTime = Mathf.Clamp01(elapsedTime);
                SlotParent.transform.position = Vector3.Lerp(firstPagePos.transform.position, secondPagePos.transform.position, elapsedTime);
                if (elapsedTime >= 1.0f)
                    break;
                yield return null;
            }
            isStartPage = false;
            SetPageText();
            yield return null;
        }
        else if (!isStartPage)
        {
            while (true)
            {
                elapsedTime += Time.deltaTime / slideTime;
                elapsedTime = Mathf.Clamp01(elapsedTime);
                SlotParent.transform.position = Vector3.Lerp(secondPagePos.transform.position, firstPagePos.transform.position, elapsedTime);
                if (elapsedTime >= 1.0f)
                    break;
                yield return null;
            }
            isStartPage = true;
            SetPageText();
            yield return null;
        }
    }

    private void SetPageText()
    {
        pageText = PageBtn.gameObject.GetComponentInChildren<Text>();
        if (isStartPage)
        {
            pageText.text = "1";
        }
        else
        {
            pageText.text = "2";
        }
    }
}
