using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkillBook : BaseGameUI
{
    public List<Skill> skillList = new List<Skill>();
    private List<Skill> mySkills = new List<Skill>();
    
    private SkillDetail skillDetail;
    public PanelParent panelParent;
    public SkillBookDeck skillBookDeck;

    private Transform panelTransform;

    public override void Init()
    {
        foreach (Skill skill in ResourceManager.Instance.SkillLIST)
        {
            skillList.Add(skill);
            skill.isEquip = false;
        }

        panelTransform = UIGameMng.Instance.gameObject.GetComponentInChildren<PanelParent>(true).transform;

        if (skillDetail == null)
        {
            skillDetail = GetComponentInChildren<SkillDetail>();
        }
        if (skillBookDeck == null)
        {
            skillBookDeck = GetComponentInChildren<SkillBookDeck>();
            skillBookDeck.Init();
        }
        if(panelParent == null)
        {
            panelParent = GetComponentInChildren<PanelParent>();
            panelParent.InitPanel();
        }
        LevelUpUpdate();
    }

    // 스킬리스트를 모두 검사하여
    // 스킬 사용레벨이 달성되었으며 내스킬에 추가되어있지 않은 스킬이라면 추가,
    // 스킬에맞는 스킬 패널 생성
    public override void LevelUpUpdate()
    {
        int playerLevel = Player.Instance.data.level;
        foreach (Skill skill in skillList)
        {
            if (skill.useLevel <= playerLevel && !mySkills.Contains(skill))
            {
                mySkills.Add(skill);
                AddSkillPanel(skill);
            }
        }
    }

    // PanelParent에 새로운 skillPanel을 추가하는 함수
    public void AddSkillPanel(Skill skill)
    {
        GameObject newPanel = Instantiate(ResourceManager.Instance.SKILLPANEL, panelTransform);
        SkillPanel skillPanel = newPanel.GetComponent<SkillPanel>();
        skillPanel.skill = skill;
        skillPanel.DisplaySkill();
        panelParent.skillPanels.Add(skillPanel);
        panelParent.AddOnClickListener(skillPanel);
    }

    public override void Open()
    {
        base.Open();
        gameObject.SetActive(true);
        LevelUpUpdate();
        DownLoadAssetBundle.Instance.SetSkillBookSprite();
        DownLoadAssetBundle.Instance.SetSkillBookSlotSprite();
        panelParent.OpenSkillBook();
        skillDetail.OpenSkillDetail();
    }

    public override void Close()
    {
        base.Close();
        gameObject.SetActive(false);
    }
}
