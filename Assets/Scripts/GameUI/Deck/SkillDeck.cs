using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillDeck : BaseDeck
{
    public SkillSlot[] SkillSlots { get { return skillSlots; } }
    private SkillSlot[] skillSlots;
    public SkillPopUp skillPopup;

    public Skill currSelectedSkill;
    private bool isWaitSkillAct;
    private bool isActive;
    
    public override void Init()
    {
        skillSlots = GetComponentsInChildren<SkillSlot>();
        SetSlotIndex();
    }

    public void SetSlotIndex()
    {
        for(int i = 0; i <skillSlots.Length; ++i)
        {
            skillSlots[i].slotIndex = i;
            skillSlots[i].Init();
        }
    }

    // SkillSlot 에서 실행
    public override void OnClickSlot(Skill selectedSkill)
    {
        SkillManager.Instance.currSelectedSkill = selectedSkill;
        currSelectedSkill = selectedSkill;
        isWaitSkillAct = true;
        Player.Instance.playerController.isWaitActSkill = isWaitSkillAct;
        skillPopup.ShowPopUp(selectedSkill);
    }

    // 스킬팝업창의 닫기버튼 에 OnClick이벤트추가된 함수
    public void OnClickCancelSkill()
    {
        skillPopup.currSkillPopup.SetActive(false);
        skillPopup.currSkillPopup = null;
        currSelectedSkill = null;
        isWaitSkillAct = false;
        Player.Instance.playerController.isWaitActSkill = isWaitSkillAct;
        SkillManager.Instance.currSelectedSkill = null;
    }

    public void ReceiveSlotInfo(Skill skill, int slotIndex)
    {
        // 스킬이 장착되었을 경우
        if(skill.isEquip)
        {
            foreach(SkillSlot skillSlot in skillSlots)
            {
                if (slotIndex == skillSlot.slotIndex)
                {
                    skillSlot.EquipSkill(skill);
                }
            }
        }
        // 스킬이 장착해제되었을 경우
        else if(!skill.isEquip)
        {
            foreach (SkillSlot skillSlot in skillSlots)
            {
                if (slotIndex == skillSlot.slotIndex)
                {
                    skillSlot.UnEquipSkill();
                }
            }
        }
    }

    private void MouseInput()
    {
        // 스킬사용 대기상태
        if (isWaitSkillAct)
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;

            if (Input.GetMouseButtonDown(0))
            {
                skillPopup.UnShowPopUp();

                // 마우스포인트에 타겟이 있다면 마커를 타겟에 붙여준다.
                if (skillPopup.CheckUseSkill(currSelectedSkill) == true)
                {
                    Vector3 mousePos;
                    mousePos = skillPopup.MouseBtn(currSelectedSkill);
                    skillPopup.ShowSkillMarker(currSelectedSkill, mousePos);
                }
                else if (skillPopup.CheckUseSkill(currSelectedSkill) == false)
                {
                    skillPopup.UnShowSkillMarker();
                }
            }

            // 따라다닐때 해당하는 타겟에 마우스포인터가 가면 타겟/위치 표시
            if (Input.GetMouseButton(0))
            {
                // SkillMarker Visuable 조건검사를 통과했다면
                if (skillPopup.CheckUseSkill(currSelectedSkill)==true)
                {
                    Vector3 mousePos;
                    mousePos = skillPopup.MouseBtn(currSelectedSkill);
                    skillPopup.ShowSkillMarker(currSelectedSkill, mousePos);
                }
                else if(skillPopup.CheckUseSkill(currSelectedSkill)==false)
                {
                    skillPopup.UnShowSkillMarker();
                }
            }

            // 마우스위치와 스킬범위에 타겟이있다면 스킬실행
            if (Input.GetMouseButtonUp(0))
            {
                // 타겟을 따라가는 스킬이면 타겟에 마커 계속 생성되게 수정
                if (skillPopup.CheckUseSkill(currSelectedSkill) == true)
                {
                    Vector3 mousePos;
                    mousePos = skillPopup.MouseBtn(currSelectedSkill);
                    skillPopup.ShowSkillMarker(currSelectedSkill, mousePos);

                    SetSkillPlay(mousePos);
                }
                else if (skillPopup.CheckUseSkill(currSelectedSkill) == false)
                {
                    skillPopup.UnShowSkillMarker();
                }

                skillPopup.UnShowSkillMarker();
                isWaitSkillAct = false;
                Player.Instance.playerController.isWaitActSkill = isWaitSkillAct;
                currSelectedSkill = null;
            }
        }
    }

    // 스킬 사용이 결정되면,
    // 해당스킬의 타입에 따라 타겟이나 스킬실행 방향, 등이 결정된다.
    // 타겟 스킬은, 타겟의 위치를 목표위치로 설정하고 플레이어의 상태를 Move로 변경한다.
    // 범위 스킬은,1. 해당스킬의 Trigger 안의 Monster 들을 파악하여 Attack 을 실행하거나,
    //             2. 해당스킬의 Trigger 안의 Monster 들에게 Damaged를 직접 실행시킨다.
    private void SetSkillPlay(Vector3 mousePos)
    {
        if (currSelectedSkill.skillType == SkillType.Area)
        {
            // 임시로 아무 transform 값 적용
            Player.Instance.playerController.targetPos = transform;
            currSelectedSkill.SetPlaySkill(mousePos);
        }
        else if (currSelectedSkill.skillType == SkillType.Target)
        {
            currSelectedSkill.SetPlaySkill(mousePos);
        }
    }

    void Update()
    {
        MouseInput();
    }
}
