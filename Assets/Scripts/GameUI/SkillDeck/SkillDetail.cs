using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// SkillBook에서 받은 Skill정보 가져와 보여주는 기능
public class SkillDetail : MonoBehaviour
{
    private Skill skill;
    private bool isActive;

    public Text skillName;
    //public Text skillLevel;
    public Text manaCost;
    public Text coolTime;
    public Text skillDesc;
    //public Text nextLevel;

    public void DisplayDetail(Skill skill)
    {
        if (isActive == false)
            SetActive(true);

        skillName.text = skill.name;
        manaCost.text = "필요한 마나 : " + skill.manaCost.ToString();
        coolTime.text = "스킬 쿨타임 : " + skill.coolTime.ToString() + "초";
        skillDesc.text = skill.description;
    }

    public void SetActive(bool active)
    {
        if(active == true)
        {
            gameObject.SetActive(active);
            isActive = true;
        }
        else if(active == false)
        {
            gameObject.SetActive(active);
            isActive = active;
        }
    }

    public void OpenSkillDetail()
    {
        gameObject.SetActive(true);
        isActive = true;
    }

    public void CloseSkillDetail()
    {
        gameObject.SetActive(false);
        isActive = false;
    }
}
