using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelParent : MonoBehaviour
{
    private SkillBookDeck skillBookDeck;
    public List<SkillPanel> skillPanels = new List<SkillPanel>();
    private int selectedPanel;
    private SkillDetail skillDetail;
    private Skill selectedSkill;

    public void InitPanel()
    {
        skillDetail = UIGameMng.Instance.GetComponentInChildren<SkillDetail>(true);
        selectedPanel = 0;
    }

    // 스킬 패널이 추가될때 버튼리스너 연결시키는 부분
    public void AddOnClickListener(SkillPanel skillPanel)
    {
        skillPanel.panelBtn.onClick.AddListener(() => { SetWaitState(skillPanel.key); });
    }

    // 스킬디테일 UI에 데이터 넣는 함수
    public void OpenSkillBook()
    {
        SetWaitState(selectedPanel);
    }

    private bool OverlapCheck(int selectedkey)
    {
        foreach (SkillPanel skillPanel in skillPanels)
        {
            if (skillPanel.key == selectedkey)
            {
                if (!skillPanel.skill.isEquip)
                    return true;
                else if (skillPanel.skill.isEquip)
                    Debug.Log("이미 장착중인 스킬입니다.");
            }
        }
        return false;
    }

    // 패널 클릭시 덱선택 대기상태로 변환하는 함수
    public void SetWaitState(int selectedkey)
    {
        if(!OverlapCheck(selectedkey))
            return;

        switch (selectedkey)
        {
            case 0:
                foreach (SkillPanel skillPanel in skillPanels)
                {
                    if (skillPanel.key == 0)
                    {
                        skillDetail.DisplayDetail(skillPanel.skill);
                        selectedPanel = 0;
                        ChangeColor(0);
                        selectedSkill = skillPanel.skill;
                    }
                }
                break;
            case 1:
                foreach (SkillPanel skillPanel in skillPanels)
                {
                    if (skillPanel.key == 1)
                    {
                        skillDetail.DisplayDetail(skillPanel.skill);
                        selectedPanel = 1;
                        ChangeColor(1);
                        selectedSkill = skillPanel.skill;
                    }
                }
                break;
            case 2:
                foreach (SkillPanel skillPanel in skillPanels)
                {
                    if (skillPanel.key == 2)
                    {
                        skillDetail.DisplayDetail(skillPanel.skill);
                        selectedPanel = 2;
                        ChangeColor(2);
                        selectedSkill = skillPanel.skill;
                    }
                }
                break;
        }
        //패널 클릭시 덱 선택대기상태 실행
        if(skillBookDeck == null)
            skillBookDeck = FindObjectOfType<SkillBookDeck>();
        skillBookDeck.WaitForSelectSkill(selectedSkill);
    }

    public void ChangeColor(int selectedKey)
    {
        for (int i = 0; i < skillPanels.Count; ++i)
        {
            ColorBlock cb = skillPanels[i].panelBtn.colors;

            if(skillPanels[i].key == selectedKey)
            {

                cb.normalColor = cb.selectedColor;
                skillPanels[i].panelBtn.colors = cb;
            }
            else
            {
                cb.normalColor = Color.white;
                skillPanels[i].panelBtn.colors = cb;
            }
        }
    }

    public void UpdateSkillPanel()
    {
        foreach(SkillPanel skillPanel in skillPanels)
        {
            if (skillPanel.skill.isEquip)
                skillPanel.SetEquipState(true);
            else if (!skillPanel.skill.isEquip)
                skillPanel.SetEquipState(false);
        }
    }
}
