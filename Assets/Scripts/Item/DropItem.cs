using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    private int keyNum = 1;

    public Dictionary<int,ItemObject> itemObjects = new Dictionary<int, ItemObject>();
    public List<GameObject> dropItems = new List<GameObject>();

    private static DropItem instance;
    public static DropItem Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject("DropItem", typeof(DropItem));
                DontDestroyOnLoad(obj);

                instance = obj.GetComponent<DropItem>();
            }
            return instance;
        }
    }

    // 3. ItemObject를 PickUpItem 에 넣어주고, 사용한다.
    public void ItemDrop(Item itemData, Vector3 dropPos, int itemCount = 1)
    {
        for (int i = 0; i < itemCount; i++)
        {
            // ItemPrefab 생성
            foreach(GameObject obj in ResourceManager.Instance.ITEM)
            {
                // 1. Client가 원하는 Item을 ItemDB에서 찾고, 
                if (obj.name == itemData.modelName)
                {
                    // 2. 해당 Item의 정보들을 ItemObject 에 넣어서 식별코드와 묶는다.
                    ItemObject newItemObject = NewItemObect(itemData);

                    GameObject ItemPrefab = Instantiate(obj, dropPos, Quaternion.identity);
                    SetDropRotate(ItemPrefab);
                    ItemPrefab.name = ItemPrefab.name.Replace("(Clone)", "");

                    SetPickUpItem(ItemPrefab, newItemObject);

                    // SetKeyID(ItemPrefab);

                    OnDropItemVFX(ItemPrefab);

                    dropItems.Add(ItemPrefab);
                }
            }
        }
    }

    // dropsitems.remove 창
    public void InventoryItemDrop(ItemObject itemObj, Vector3 dropPos, int itemCount = 1)
    {
        for (int i = 0; i < itemCount; i++)
        {
            // ItemPrefab 생성
            foreach (GameObject obj in ResourceManager.Instance.ITEM)
            {
                // 1. Client가 원하는 Item을 ItemDB에서 찾기
                if (obj.name == itemObj.ModelName)
                {
                    GameObject ItemPrefab = Instantiate(obj, dropPos, Quaternion.identity);
                    SetDropRotate(ItemPrefab);
                    ItemPrefab.name = ItemPrefab.name.Replace("(Clone)", "");

                    SetPickUpItem(ItemPrefab, itemObj);

                    // SetKeyID(ItemPrefab);

                    OnDropItemVFX(ItemPrefab);

                    dropItems.Add(ItemPrefab);
                }
            }
        }
    }

    public ItemObject NewItemObect(Item itemData)
    {
        ItemObject itemObject = new ItemObject(itemData, keyNum);
        itemObjects[keyNum] = itemObject;
        keyNum += 1;

        return itemObject;
    }

    public void SetPickUpItem(GameObject dropItem, ItemObject item)
    {
        if (dropItem.GetComponent<PickUpItem>() != null)
        {
            PickUpItem pickUpItem = dropItem.GetComponent<PickUpItem>();
            pickUpItem.modelName = dropItem.name;
            pickUpItem.PPikcUpItem = item;
        }
    }

    // Item의 keyID 부여
    //public void SetKeyID(GameObject dropItem)
    //{
    //    if (dropItem.GetComponent<PickUpItem>() != null)
    //    {
    //        PickUpItem pickUpItem = dropItem.GetComponent<PickUpItem>();
    //        // PickupItem 이 아직 채워지지 않은 상태
    //        pickUpItem.PPikcUpItem.IdentifyID = keyNum;
    //        keyNum += 1;
    //    }
    //}

    private void OnDropItemVFX(GameObject dropItem)
    {
        GameObject itemDropVFX = Instantiate(ResourceManager.Instance.ITEMDROP_VFX, dropItem.transform);
        itemDropVFX.transform.position = dropItem.transform.position;
        SetVFXRotation(itemDropVFX);
    }

    // 임시 Rotate Setting
    private void SetVFXRotation(GameObject itemdropVFX)
    {
        Quaternion dropRotation = Quaternion.Euler(-90, 0, 0);
        itemdropVFX.transform.rotation = dropRotation;
    }

    private void SetDropRotate(GameObject dropItem)
    {
        dropItem.transform.rotation = Quaternion.Euler(90, 0, 0);
    }

    // 드롭할 아이템 리턴
    // 이름 설정시 지정드롭
    // 이름 미설정시 랜덤드롭
    public Item rndItem(ItemType dropType, string dropName = null)
    {
        List<Item> equalTypeList = new List<Item>();

        foreach (Item item in ItemDB.Instance.itemDB)
        {
            // 일단 무기만 떨어지도록 설정
            if (item.itemType == dropType )
            {
                //if (item.detailType == DetailType.Bow
                //|| item.detailType == DetailType.DoubleSword
                //|| item.detailType == DetailType.MagicWand
                //|| item.detailType == DetailType.TwoHandSword
                //|| item.detailType == DetailType.Sword
                //|| item.detailType == DetailType.Shield)
                 equalTypeList.Add(item);
            }
        }

        if (dropName != null)
        {
            foreach(Item item in equalTypeList)
            {
                if (item.name == dropName)
                    return item;
            }
        }

        if (equalTypeList.Count == 0)
            return null;

        int rndNum = Random.Range(0, equalTypeList.Count-1);
        return equalTypeList[rndNum];
    }

    // 드랍할 위치 리턴
    public Vector3 DropPos(GameObject deadObj, float dropRadius =1f)
    {
        float rndRadius = Random.Range(-dropRadius, dropRadius);

        Vector3 tmp = deadObj.transform.position;
        tmp.x += rndRadius;
        tmp.z += rndRadius;
        Vector3 dropPos = tmp;

        return dropPos;
    }
}