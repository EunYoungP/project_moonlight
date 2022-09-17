using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public PlayerEffectType effectType;
    public Skill effectSkill;
    public int effectNum;

    ParticleSystem skillEffect;

    public void Init()
    {
        skillEffect = GetComponent<ParticleSystem>();
    }

    private void StopSkillVFX()
    {
       gameObject.SetActive(false);
    }

    private void Update()
    {
        if(!skillEffect.IsAlive())
        {
            StopSkillVFX();
        }
    }
}
