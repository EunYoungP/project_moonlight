using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : BaseGameUI
{
    Category category;
    InventorySlot[] inventorySlots;

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
    public bool SetEquipState(ItemObject item, bool SetEquip)
    {
        if (SetEquip)
        {
            foreach (InventorySlot inventorySlot in inventorySlots)
            {
                // item의 identify key 가 필요
                if (inventorySlot.itemExist
                    && inventorySlot.item.IdentifyID == item.IdentifyID
                    && inventorySlot.isEquipState == false)
                {
                    inventorySlot.SetEquipState();
                    return true;
                }
            }
            Debug.Log("아이템을 장착할 수 없습니다.");
        }
        if (!SetEquip)
        {
            foreach (InventorySlot inventorySlot in inventorySlots)
            {
                if (inventorySlot.itemExist 
                    && inventorySlot.item.IdentifyID == item.IdentifyID 
                    && inventorySlot.isEquipState == true)
                {
                    inventorySlot.SetUnEquipState();
                    return true;
                }
            }
            Debug.Log("아이템을 해제할 수 없습니다.");
        }
        return false;
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

        inventoryItems.Add(item);
        Categorize();
        return true;
    }

    public void RemoveItem(ItemObject item)
    {
        inventoryItems.Remove(item);
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
            if(i < tabItems.Count)
            {
                inventorySlots[i].AddItem(tabItems[i]);
            }
            else
            {
                inventorySlots[i].ClearSlot();
            }
        }
    }
}
