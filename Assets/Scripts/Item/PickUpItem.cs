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
        bool isAdd = UIGameMng.Instance.uiGameDic[UIGameType.Inventory].AddItem(PPikcUpItem);

        if (isAdd)
            Destroy(gameObject);
    }

    private void PlayAddItemAnim()
    {
        if(AddItemUIObj == null)
        {
            AddItemUIObj = Instantiate(ResourceManager.Instance.PICKUPITEM_UI, transform.parent);
            AddItemAnim = AddItemUIObj.GetComponentInChildren<Animator>();
            AddItemImg = AddItemUIObj.GetComponentInChildren<Image>();
        }

        SetActiveAnim(true);

        foreach(Sprite sprite in ResourceManager.Instance.ITEMICON)
        {
            if (sprite.name == PPikcUpItem.IconName)
                AddItemImg.sprite = sprite;
        }

        StartCoroutine(CheckAnimDone());
    }

    IEnumerator CheckAnimDone()
    {
        while(AddItemAnim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        {
            yield return null;
        }

        SetActiveAnim(false);
        yield return null;
    }

    private void SetActiveAnim(bool value)
    {
        AddItemUIObj.SetActive(value);
    }
}
