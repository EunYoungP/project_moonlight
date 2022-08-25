using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PLAYER_EFFECT_TYPE
{
    SKILL,
    LEVELUP,
}

public class PlayerEffect : EffectBase
{
    List<Effect> playerEffects = new List<Effect>();

    public override void Init()
    {
        AddEffects();
    }

    private void AddEffects()
    {
        Effect[] effects = gameObject.GetComponentsInChildren<Effect>(true);
        foreach(Effect effect in effects)
        {
            playerEffects.Add(effect);
            effect.Init();
        }
    }

    public override void PlaySkillEffect(int effectNum)
    {
        // 현재 선택된 스킬에 null 이 들어가서 오류가 생긴다.
        Skill selectedSkill = SkillManager.Instance.currSelectedSkill;
        foreach(Effect effect in playerEffects)
        {
            if(effect.effectType == PLAYER_EFFECT_TYPE.SKILL &&
                selectedSkill == effect.effectSkill)
            {
                if(effect.effectNum == effectNum)
                {
                    effect.gameObject.SetActive(true);
                }
            }
        }
    }

    public void OnLevelUpVFX()
    {
        foreach(Effect effect in playerEffects)
        {
            if (effect.effectType == PLAYER_EFFECT_TYPE.LEVELUP)
            {
                effect.gameObject.SetActive(true);
                ParticleSystem vfx = effect.gameObject.GetComponent<ParticleSystem>();
                float holdingTime = vfx.duration;
                StartCoroutine(HoldingLevelUpVFX(holdingTime));
            }
        }
    }

    IEnumerator HoldingLevelUpVFX(float holdingTime)
    {
        yield return new WaitForSeconds(holdingTime);
    }
}
