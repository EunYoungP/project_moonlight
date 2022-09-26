using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPickUpItem : BaseUI
{
    private Animator AddItemAnim;
    private Image AddItemImg;

    public override void UIInit()
    {
        AddItemAnim = GetComponentInChildren<Animator>();
        AddItemImg = GetComponentInChildren<Image>();
    }

    public void PlayAddItemAnim(ItemObject addItem)
    {
        foreach (Sprite sprite in ResourceManager.Instance.ITEMICON)
        {
            if (sprite.name == addItem.IconName)
                AddItemImg.sprite = sprite;
        }
        StartCoroutine(CheckAnimDone());
    }

    IEnumerator CheckAnimDone()
    {
        while (AddItemAnim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        {
            yield return null;
        }
        Close();
    }

    public override void Open()
    {
        gameObject.SetActive(true);
    }

    public override void Close()
    {
        gameObject.SetActive(false);
    }
}
