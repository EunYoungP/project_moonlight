using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : BaseGameUI
{
    private Category category;
    private InventorySlot[] inventorySlots;

    public List<ItemObject> equipItems = new List<ItemObject>();
    public List<ItemObject> inventoryItems = new List<ItemObject>();
    public List<ItemObject> tabItems = new List<ItemObject>();

    public List<Button> tabBtns;
    public int selectedTab;
    public int space = 20;
    private bool isInitalize = true;

    public override void Init()
    {
        category = GetComponentInChildren<Category>();
        inventorySlots = GetComponentsInChildren<InventorySlot>();

        category.Init();
        tabBtns = category.tabBtns;
        TabAddListener();
        InitTab();
    }

    // 카테고리 버튼 리스너 연결
    public void TabAddListener()
    {
        foreach (Button btn in tabBtns)
        {
            btn.onClick.AddListener(() => { OnClickTab(btn); });
        }
    }

    public void InitTab()
    {
        selectedTab = 0;
        ChangeColor(tabBtns[0]);
        Categorize();
    }

    public void InitItem()
    {
        if (!isInitalize)
            return;

        List<ItemObject> LoadItems = new List<ItemObject>();
        LoadItems = SLManager.Instance.curInventoryItem;

        foreach(ItemObject itemobj in LoadItems)
        {
            if (inventoryItems.Count > space)
            {
                Debug.Log("가방이 꽉 찼습니다.");
                return;
            }
            inventoryItems.Add(itemobj);
        }
        Categorize();
        isInitalize = false;
    }

    // 장착중 표시제어
    public bool SetEquipState(ItemObject item,InventorySlot selectedSlot, bool SetEquip)
    {
        if (SetEquip)
        {
            if (selectedSlot.isItemExist
                && !selectedSlot.isEquipState
                && selectedSlot.item.IdentifyID == item.IdentifyID)
            {
                selectedSlot.SetEquipState();
                equipItems.Add(item);
                return true;
            }
            else
            {
                Debug.Log("아이템을 장착할 수 없습니다.");
            }
        }
        if (!SetEquip)
        {
            if (selectedSlot.isItemExist
                && selectedSlot.item.IdentifyID == item.IdentifyID
                && selectedSlot.isEquipState == true)
            {
                selectedSlot.SetUnEquipState();
                equipItems.Remove(item);
                return true;
            }
            else
            {
                Debug.Log("아이템을 해제할 수 없습니다.");
            }
        }
        return false;
    }

    private void CheckEquipItem(ItemObject item, InventorySlot invenSlot)
    {
        // 장착중인 아이템에 받아온 아이템이 들어있다면,
        if(equipItems.Contains(item))
        {
            invenSlot.SetEquipState();
        }
    }

    // UIMenu 에 연결되어있는 함수
    public override void Open()
    {
        base.Open();
        if (category == null)
            Init();

        gameObject.SetActive(true);
        InitItem();
        Categorize();
    }

    public override void Close()
    {
        base.Close();
        gameObject.SetActive(false);
    }

    public override bool AddItem(ItemObject item)
    {
        // GetItem 퀘스트 판정
        if (Player.Instance.GetItemEvent != null)
            Player.Instance.GetItemEvent(item);

        if (inventoryItems.Count > space)
        {
            Debug.Log("가방이 꽉 찼습니다.");
            return false;
        }
        else
        {
            inventoryItems.Add(item);
            Categorize();
            return true;
        }
    }

    public void RemoveItem(ItemObject item)
    {
        inventoryItems.Remove(item);
        Categorize();

        InventoryItemDrop(item);
        //DropItem.Instance
    }

    public void InventoryItemDrop(ItemObject dropItem)
    {
        Vector3 dropPos = DropItem.Instance.DropPos(Player.Instance.gameObject);
        DropItem.Instance.InventoryItemDrop(dropItem, dropPos);
    }

    public void OnClickTab(Button btn)
    {
        for (int i = 0; i < tabBtns.Count; i++)
        {
            if (tabBtns[i] == btn)
            {
                selectedTab = i;
                ChangeColor(btn);
                break;
            }
        }
        Categorize();
    }

    public void ChangeColor(Button btn)
    {
        for(int i =0; i<tabBtns.Count; ++i)
        {
            ColorBlock cb = tabBtns[i].colors;
            
            if(tabBtns[i] == btn)
            {
                cb.normalColor = cb.selectedColor;
                tabBtns[i].colors = cb;
            }
            else
            {
                cb.normalColor = Color.white;
                tabBtns[i].colors = cb;
            }
        }
    }

    // 탭 고르고 타입에맞는 아이템을 텝아이템에 담음
    public void Categorize()
    {
        tabItems.Clear();
        switch (selectedTab)
        {
            case 0:
                for (int i = 0; i < inventoryItems.Count; ++i)
                {
                    tabItems.Add(inventoryItems[i]);
                }
                break;
            case 1:
                for (int i = 0; i < inventoryItems.Count; ++i)
                {
                    if (inventoryItems[i].ItemType == ItemType.Equipment
                    || inventoryItems[i].ItemType == ItemType.Weapon
                    || inventoryItems[i].ItemType == ItemType.Accessories)
                        tabItems.Add(inventoryItems[i]);
                }
                break;
            case 2:
                for (int i = 0; i < inventoryItems.Count; ++i)
                {
                    if (inventoryItems[i].ItemType == ItemType.Use)
                        tabItems.Add(inventoryItems[i]);
                }
                break;
            case 3:
                for (int i = 0; i < inventoryItems.Count; ++i)
                {
                    if (inventoryItems[i].ItemType == ItemType.Food)
                        tabItems.Add(inventoryItems[i]);
                }
                break;
            case 4:
                for (int i = 0; i < inventoryItems.Count; ++i)
                {
                    if (inventoryItems[i].ItemType == ItemType.Ingredient)
                        tabItems.Add(inventoryItems[i]);
                }
                break;
            case 5:
                for (int i = 0; i < inventoryItems.Count; ++i)
                {
                    if (inventoryItems[i].ItemType == ItemType.Etc
                        || inventoryItems[i].ItemType == ItemType.Quest)
                        tabItems.Add(inventoryItems[i]);
                }
                break;
        }
        UpdateInventory();
    }

    // 탭에맞는 아이템목록을 인벤토리에 채워넣는 함수 
    // Categorize 후 실행
    public void UpdateInventory()
    {
        for(int i = 0; i < inventorySlots.Length; ++i)
        {
            inventorySlots[i].ClearSlot();

            if(i < tabItems.Count)
            {
                inventorySlots[i].AddItem(tabItems[i]);
                // 장착중이 아닌 아이템들이 모두 장착중상태로 변환
                //SetEquipState(tabItems[i], inventorySlots[i], true);
                CheckEquipItem(tabItems[i], inventorySlots[i]);
            }
            //else
            //{
            //    inventorySlots[i].ClearSlot();
            //    SetEquipState(tabItems[i], inventorySlots[i], false);
            //}
        }
    }
}
