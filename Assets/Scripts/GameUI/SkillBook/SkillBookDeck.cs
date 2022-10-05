using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillBookDeck : MonoBehaviour
{
    public SkillBookSlot[] skillBookSlots;

    public Button PageBtn;
    public Transform SlotParent;
    public Transform firstPagePos;
    public Transform secondPagePos;

    private bool isStartPage = true;
    private float slideTime;

    private Skill selectedSkill;
    public PanelParent panelParent;
    private SkillDeck skillDeck;
    private bool isWaitEquip;

    public void  Init()
    {
        skillBookSlots = GetComponentsInChildren<SkillBookSlot>(true);
        skillDeck = GameObject.Find("SkillDeck").GetComponent<SkillDeck>();
        SetSlotIndex();
        InitSlot();
        InitDeck();
    }

    public void SetSlotIndex()
    {
        for(int i = 0; i<skillBookSlots.Length; ++i)
        {
            skillBookSlots[i].slotIndex = i;
        }
    }

    public void InitSlot()
    {
        foreach(SkillBookSlot skillSlot in skillBookSlots)
        {
            skillSlot.Init();
            skillSlot.GetComponent<Button>().onClick.AddListener(() => { ClickSlot(skillSlot); });
        }
    }

    public void InitDeck()
    {
        SlotParent.transform.position = firstPagePos.transform.position;
        SetPageText();
    }

    // 스킬패널을 누르면 스킬북덱을 스킬선택대기 상태로 변환
    public void WaitForSelectSkill(Skill skill)
    {
        isWaitEquip = true;
        selectedSkill = skill;

        foreach (SkillBookSlot skillSlot in skillBookSlots)
        {
            skillSlot.WaitForSelectSkill(isWaitEquip);
        }
    }

    // 클릭된 슬롯이있을때 실행되는 함수
    public void ClickSlot(SkillBookSlot selectedSlot)
    {
        // 스킬장착 대기 상태
        if(selectedSlot.ClickSlot(isWaitEquip, selectedSkill))
        {
            // 스킬덱에 장착된 스킬정보 전달
            skillDeck.ReceiveSlotInfo(selectedSkill, selectedSlot.slotIndex);

            isWaitEquip = false;
        }
        // 스킬장착해제 대기상태
        else if(!selectedSlot.ClickSlot(isWaitEquip, selectedSkill))
        {
            // 스킬덱에 장착된 스킬정보 전달
            skillDeck.ReceiveSlotInfo(selectedSkill, selectedSlot.slotIndex);
            isWaitEquip = false;
        }

        // 스킬패널에 장착된 스킬이 있는지 검사
        panelParent.UpdateSkillPanel();

        // 스킬북덱 슬롯의 대기상태 끄기
        foreach (SkillBookSlot skillSlot in skillBookSlots)
        {
            skillSlot.WaitForSelectSkill(false);
        }
    }

    public void ChangePage()
    {
        slideTime = 0.3f;
        StartCoroutine("CoChangePage");
    }

    // 버튼을 누르면 다른페이지로 바꿔주는 기능
    IEnumerator CoChangePage()
    {
        float timer = 0f;
        isStartPage = SlotParent.transform.position == firstPagePos.transform.position ? true : false;
        if (isStartPage)
        {
            while(true)
            {
                timer += Time.deltaTime / slideTime;
                timer = Mathf.Clamp01(timer);
                SlotParent.transform.position =  Vector3.Lerp(firstPagePos.transform.position, secondPagePos.transform.position, timer);
                if (timer >= 1.0f)
                    break;
                yield return null;
            }
            isStartPage = false;
            SetPageText();
            yield return null;
        }
        else if(!isStartPage)
        {
            while(true)
            {
                timer += Time.deltaTime / slideTime;
                timer = Mathf.Clamp01(timer);
                SlotParent.transform.position = Vector3.Lerp(secondPagePos.transform.position, firstPagePos.transform.position, timer);
                if (timer >= 1.0f)
                    break;
                yield return null;
            }
            isStartPage = true;
            SetPageText();
            yield return null;
        }
    }

    public void SetPageText()
    {
        Text pageText = PageBtn.gameObject.GetComponentInChildren<Text>();
        if (isStartPage)
            pageText.text = "1";
        else
            pageText.text = "2";
    }
}
