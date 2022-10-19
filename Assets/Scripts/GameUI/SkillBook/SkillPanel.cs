using System.Collections;
using System.Collections.Generic;
using System.Text;
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
    private StringBuilder sb =  new StringBuilder(100);

    // 스킬패널에 해당스킬의 정보 표현
    public void DisplaySkill()
    {
        skillIcon.gameObject.SetActive(true);
        skillName.text = skill.name;
        sb.Append("스킬 사용가능 레벨 : ").Append(skill.useLevel.ToString());
        skillLevel.text = sb.ToString();
        sb.Clear();
        key = skill.key;
    }

    public void SetEquipState(bool isEquip)
    {
        EquipCheck.SetActive(isEquip);
    }
}
