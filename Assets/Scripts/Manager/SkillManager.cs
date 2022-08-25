using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SkillManager : MonoBehaviour
{
    public Dictionary<WeaponState, List<Skill>> skillDic = new Dictionary<WeaponState, List<Skill>>();
    public List<SkillCollider> skillColliders = new List<SkillCollider>();
    public Dictionary<Skill, SkillCollider> skillColliderDic = new Dictionary<Skill, SkillCollider>();
    public Skill currSelectedSkill;
    public static SkillColliderManager skillColliderManager;

    private static SkillManager instance;
    public static SkillManager Instance
    {
        get
        {
            if(instance == null)
            {
                GameObject obj = new GameObject("SkillManager", typeof(SkillManager));
                DontDestroyOnLoad(obj);
                instance = obj.GetComponent<SkillManager>();
                

                GameObject collObj = new GameObject("SkillColliderManager",  typeof(SkillColliderManager));
                collObj.transform.parent = obj.transform;
                skillColliderManager = collObj.GetComponent<SkillColliderManager>();
            }
            return instance;
        }
    }

    // skillDic 채우기
    public void Init()
    {
        AddSkillDic(WeaponState.Bow);
        AddSkillDic(WeaponState.DoubleSword);
        AddSkillDic(WeaponState.MagicWand);
        AddSkillDic(WeaponState.SwordShield);
        AddSkillDic(WeaponState.TwoHandSword);
    }

    // 해당하는 weaponState에 맞는 스킬들을
    // skillNum 의 오름차순으로 리스트에 담아서 
    // Dictionary의 값으로 넣어준다.
    private void AddSkillDic(WeaponState weaponState)
    {
        List<Skill> equalWeaponSkill = new List<Skill>();
        foreach (Skill skill in ResourceManager.Instance.SkillLIST)
        {
            if (weaponState == skill.UseWeaponState)
            {
                equalWeaponSkill.Add(skill);
                AddDicSkillColliders(skill);
            }
        }
        equalWeaponSkill = equalWeaponSkill.OrderBy(Skill => Skill.skillNum).ToList(); ;
        skillDic[weaponState] = (equalWeaponSkill);
    }

    private void AddDicSkillColliders(Skill skill)
    {
        if(!skillColliderDic.ContainsKey(skill))
        {
            foreach(SkillCollider skillCollider in skillColliders)
            {
                if (skillCollider.gameObject.name == skill.name)
                    skillColliderDic[skill] = skillCollider;
            }
        }
    }

    public SkillCollider GetSkillCollider(Skill skill)
    {
        if(skillColliderDic.ContainsKey(skill))
        {
            return skillColliderDic[skill];
        }
        Debug.Log("해당하는 키나 스킬콜라이더가 존재하지 않습니다.");
        return null;
    }
}
