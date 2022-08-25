using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 생성된 패널에 정보 넣어주기
public class SkillPanel : MonoBehaviour
{
    public Skill skill;
    public int key;

    public Image skillIcon;
    public Text skillName;
    public Text skillLevel;
    public Button panelBtn;
    public GameObject EquipCheck;

    // 스킬패널에 해당스킬의 정보 표현
    public void DisplaySkill()
    {
        //skillIcon.sprite = skill.skillIcon;
        //DownLoadAssetBundle.Instance.LoadSkillSprite(this.skillIcon.sprite, skill.skillIcon);
        skillIcon.gameObject.SetActive(true);
        skillName.text = skill.name;
        skillLevel.text = " 스킬 레벨 미구현";
        key = skill.key;
    }

    public void SetEquipState(bool isEquip)
    {
        EquipCheck.SetActive(isEquip);
    }
}
