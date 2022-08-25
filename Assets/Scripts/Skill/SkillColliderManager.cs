using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillColliderManager : MonoBehaviour
{
    private Skill currPlayedSkill;

    private float runTime;
    private float skill2_skillTime = 1f;
    private Vector3 prevColliderPos;
    public Vector3 end;

    // 스킬의 머택 시점에서 트리거 체크
    // 몬스터에 트리거될 경우에만 Attack 처리
    public void CheckSkillAttack(Skill currPlaySkill)
    {
        SkillCollider currSkillColliderScript = SkillManager.Instance.GetSkillCollider(currPlaySkill);
        currPlayedSkill = currPlaySkill;

        switch (currPlaySkill.key)
        {
            case 0:
                // 콜라이더 trigger 체크만 실행
                currSkillColliderScript.ActiveTrigger();
                break;
            case 1:
                currSkillColliderScript.ActiveTrigger();
                break;
            case 2:
                //콜라이더 trigger + move 실행
                StartCoroutine(Skill2_ColliderCheck(currSkillColliderScript));
                break;
        }
    }

    // 같은 몬스터 중복공격 체크 추후 추가
    IEnumerator Skill2_ColliderCheck(SkillCollider skill2_ColliderScript)
    {
        skill2_ColliderScript.gameObject.SetActive(true);

        SoundManager.Instance.Play("SkillSFX/DoubleSword/fireimpact04");

        while (skill2_skillTime > runTime)
        {
            skill2_ColliderScript.ActiveTrigger();

            runTime += Time.deltaTime;
            skill2_ColliderScript.gameObject.transform.Translate(transform.forward * 3);
            yield return null;
        }

        ResetSkillState(skill2_ColliderScript);
        yield return null;
    }

    private void ResetSkillState(SkillCollider skillCollScript)
    {
        skillCollScript.transform.localPosition = Vector3.zero;
        //skill2_ColliderScript.gameObject.SetActive(false);
        skillCollScript.activeTrigger = false;
        currPlayedSkill.SetStopSkill();
        runTime = 0;
        currPlayedSkill = null;
    }

}
