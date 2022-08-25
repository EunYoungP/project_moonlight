using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillSlot : MonoBehaviour, IPointerUpHandler
{
    public Skill CurrSkill { get { return currSkill; } }
    private Skill currSkill;
    public Image skillIcon;
    public Button slotBtn;

    public int slotIndex;
    public bool isEmptySlot;
    public bool isWaitSkilAct;

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isEmptySlot)
            return;

        UIDeck.deckDic[DeckType.SkillDeck].OnClickSlot(currSkill);
    }

    public void Init()
    {
        isEmptySlot = true;
        slotBtn = GetComponent<Button>();
    }

    public void EquipSkill(Skill skill)
    {
        currSkill = skill;
        skillIcon.gameObject.SetActive(true);
        //DownLoadAssetBundle.Instance.LoadSkillSprite(this.skillIcon.sprite, skill.skillIcon);
        //this.skillIcon.sprite = skill.skillIcon;
        isEmptySlot = false;
    }

    public void UnEquipSkill()
    {
        currSkill = null;
        this.skillIcon.sprite = null;
        skillIcon.gameObject.SetActive(false);
        isEmptySlot = true;
    }

}
