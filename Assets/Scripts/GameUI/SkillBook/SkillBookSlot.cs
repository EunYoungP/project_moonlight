using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillBookSlot : MonoBehaviour
{
    public Skill Skill { get { return skill; } }
    private Skill skill;
    public Image skillIcon;

    private float scalePulseAmount = 0.04f;          
    private float scalePulseSpeed = 10f;
    private float InitScale = 1.0f;
    private float currScale;
    private bool canPulse;
    public bool isEmpty;

    public GameObject PlusImg;
    public GameObject MinusImg;

    public int slotIndex;

    public void Init()
    {
        isEmpty = true;
    }

    // 슬롯버튼 클릭시 실행될 함수
    public bool ClickSlot(bool isWaitEquip, Skill skill)
    {
        // 스킬장착 대기상태
        if (isWaitEquip)
        {
            EquipSkill(skill);
            return true;
        }
        // 스킬해제 대기상태
        else if(!isWaitEquip)
        {
            if(!isEmpty)
            {
                UnEquipSkill();
            }
        }
        return false;
    }

    public void EquipSkill(Skill skill)
    {
        isEmpty = false;
        this.skill = skill;
        skillIcon.gameObject.SetActive(true);
        skill.isEquip = true;
        DownLoadAssetBundle.Instance.SetSkillBookSlotSprite();
    }

    public void UnEquipSkill()
    {
        isEmpty = true;
        skill.isEquip = false;
        skill = null;
        skillIcon.sprite = null;
        skillIcon.gameObject.SetActive(false);
    }

    public void WaitForSelectSkill(bool isWaitEquip)
    {
        SetImg(isWaitEquip);
        ScalePulse(isWaitEquip);
    }

    public void SetImg(bool isWaitEquip)
    {
        if(isWaitEquip)
        {
            if (isEmpty)
            {
                MinusImg.SetActive(false);
                PlusImg.SetActive(true);
            }
            else if(!isEmpty)
            {
                MinusImg.SetActive(false);
                PlusImg.SetActive(false);
            }
        }
        if(!isWaitEquip)
        {
            if(isEmpty)
            {
                PlusImg.SetActive(false);
                MinusImg.SetActive(false);
            }
            else if (!isEmpty)
            {
                MinusImg.SetActive(true);
                PlusImg.SetActive(false);
            }
        }
    }

    public void ScalePulse(bool isWaitEquip)
    {
        canPulse = isWaitEquip;
        currScale = InitScale;
        StartCoroutine("CoScalePulse");
    }

    // 슬롯 Pulse 효과
    IEnumerator CoScalePulse()
    {
        while(canPulse)
        {
            // initialScale 기준으로 scalePulseSpeed속도로 -scalePulseAmount~scaleAmount 반복
            currScale = Mathf.Sin(Time.time * scalePulseSpeed) * (scalePulseAmount) + InitScale;
            gameObject.transform.localScale = Vector3.one * currScale;
            yield return null;
        }
    }
}
