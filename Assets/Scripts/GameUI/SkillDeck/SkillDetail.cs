using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;

// SkillBook에서 받은 Skill정보 가져와 보여주는 기능
public class SkillDetail : MonoBehaviour
{
    private Skill skill;
    private bool isActive;

    public Text skillName;
    public Text manaCost;
    public Text coolTime;
    public Text skillDesc;
    private StringBuilder sb = new StringBuilder(100);

    public void DisplayDetail(Skill skill)
    {
        if (isActive == false)
            SetActive(true);

        skillName.text = skill.name;
        sb.Append("필요한 마나 : ").Append(skill.manaCost.ToString());
        manaCost.text = sb.ToString();
        sb.Clear();
        sb.Append("스킬 쿨타임 : ").Append(skill.coolTime.ToString()).Append("초");
        coolTime.text = sb.ToString();
        sb.Clear();
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
