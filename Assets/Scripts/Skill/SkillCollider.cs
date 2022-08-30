using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCollider : MonoBehaviour
{
    public bool activeTrigger = false;
    private List<GameObject> areaSkillOverlapMonster = new List<GameObject>();

    public void ActiveTrigger()
    {
        activeTrigger = true;
        gameObject.SetActive(true);
    }

    private void NotActiveTrigger()
    {
        activeTrigger = false;
        ResetOverlapList();
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!activeTrigger)
        {
            NotActiveTrigger();
            return;
        }
        
        if(SkillManager.Instance.currSelectedSkill.skillType == SkillType.Target)
        {
            if(other.gameObject.CompareTag("Monster"))
            {
                Player.Instance.playerController.targetPos = other.gameObject.transform;
                Player.Instance.OnAttack();
            }
            NotActiveTrigger();
        }
        else if(SkillManager.Instance.currSelectedSkill.skillType == SkillType.Area)
        {
            if(other.gameObject.CompareTag("Monster"))
            {
                if (areaSkillOverlapMonster.Contains(other.gameObject))
                    return;

                areaSkillOverlapMonster.Add(other.gameObject);
                Player.Instance.playerController.targetPos = other.gameObject.transform;
                Player.Instance.OnAttack();
            }
        }
    }

    private void ResetOverlapList()
    {
        for (int i = 0; i < areaSkillOverlapMonster.Count; i++)
        {
            areaSkillOverlapMonster.Remove(areaSkillOverlapMonster[i]);
        }
    }
}
