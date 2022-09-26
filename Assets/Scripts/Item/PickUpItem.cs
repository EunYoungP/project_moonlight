using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 아이템교환 함수는 델리게이트로 생성?
// 아이템에 추가된 클래스
public class PickUpItem : MonoBehaviour
{
    //public Item pickupItem;
    //private ItemObject pickupItem;
    public ItemObject PPikcUpItem { get; set; }
    public string modelName;

    private GameObject AddItemUIObj;
    private Animator AddItemAnim;
    private Image AddItemImg;

    public void PickUp()
    {
        Debug.Log("PickUp : " + PPikcUpItem.Name);
        UIMng.Instance.OpenUI<UIPickUpItem>(UIType.UIPickUpItem).PlayAddItemAnim(PPikcUpItem);
        bool isAdd = UIGameMng.Instance.uiGameDic[UIGameType.Inventory].AddItem(PPikcUpItem);

        if (isAdd)
            Destroy(gameObject);
    }   
}
