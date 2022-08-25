using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SkillType
{
    Area,
    Target,
}

[CreateAssetMenu(fileName ="New Skill", menuName ="Skill")]
public class Skill : ScriptableObject
{
    public new string name;
    public int key;

    [TextArea]
    public string description;

    public string skillIcon;
    public SkillType skillType;
    public int skillNum;
    public int manaCost;
    public float attack;
    public int health;
    public int coolTime;
    public int useLevel;
    public float skillRange;
    public WeaponState UseWeaponState;

    public bool isEquip = false;

    // 스킬 체크
    public void SetPlaySkill(Vector3 skillTargetPos)
    {
        foreach (Skill skill in ResourceManager.Instance.SkillLIST)
        {
            if (skill.name == name)
            {
                Player.Instance.playerController.isSkillState = true;

                if(skillType == SkillType.Area)
                    Player.Instance.playerController.SetAreaSkill(skillTargetPos);
                else if(skillType == SkillType.Target)
                    Player.Instance.playerController.SetTargetSkill(skillTargetPos);
                return;
            }
        }
        Debug.Log("해당하는 스킬이 없습니다.");
    }

    public void SetStopSkill()
    {
        Player.Instance.playerController.isSkillState = false;
    }
}
