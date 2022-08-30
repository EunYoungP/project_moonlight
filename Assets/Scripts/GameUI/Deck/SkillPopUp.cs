using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// SkillMarker는 SkillPopup에 포함시켜두고 키고끔
public class SkillPopUp : MonoBehaviour
{
    public GameObject TargetSkillPopup;
    public GameObject AreaSkillPopup;

    public GameObject TargetMarker;
    public GameObject AreaMarker;
    public SkillCollider areaMarkerCollider;

    public GameObject currSkillPopup;
    public GameObject target;

    // 스킬덱슬롯을 누르면 스킬발동 위치 지정을 돕는 팝업이 뜨게함
    public void ShowPopUp(Skill skill)
    {
        if (skill.skillType.Equals(SkillType.Target))
        {
            TargetSkillPopup.gameObject.SetActive(true);
            currSkillPopup = TargetSkillPopup;
        }
        else if (skill.skillType.Equals(SkillType.Area))
        {
            AreaSkillPopup.gameObject.SetActive(true);
            currSkillPopup = AreaSkillPopup;
        }
    }

    public void UnShowPopUp()
    {
        TargetSkillPopup.SetActive(false);
        AreaSkillPopup.SetActive(false);
        currSkillPopup = null;
    }

    public bool CheckUseSkill(Skill currSelectedSkill)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (currSelectedSkill.skillType.Equals(SkillType.Area))
        {
            if (Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.collider.CompareTag("Terrain"))
                {
                    //target = hitInfo.collider.gameObject;
                    return true;
                }
            }
        }
        else if (currSelectedSkill.skillType.Equals(SkillType.Target))
        {
            if (Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.collider.CompareTag("Monster"))
                {
                    // 여기서 타겟을 설정
                    target = hitInfo.collider.gameObject;
                    return true;
                }
            }
        }
        return false;
    }

    //// 마우스포인터있는곳에 target 검사후
    ////  hitinfo 위치 넘겨주는 함수
    public Vector3 MouseBtn(Skill currSelectedSkill)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (currSelectedSkill.skillType.Equals(SkillType.Area))
        {
            if (Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.collider.CompareTag("Terrain"))
                {
                    return hitInfo.point;
                }
            }
        }
        else if (currSelectedSkill.skillType.Equals(SkillType.Target))
        {
            if (Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.collider.CompareTag("Monster"))
                {
                    target = hitInfo.collider.gameObject;
                    Player.Instance.playerController.targetPos = target.transform;
                    return hitInfo.point;
                }
            }
        }
        return Vector3.zero;
    }

    public void ShowSkillMarker(Skill skill, Vector3 mousePos)
    {
        // 지정한 지역안에 몬스터가 있는지,
        // 있다면 그 몬스터를 타겟으로 설정하는 방법 구현해야함.
        if (skill.skillType.Equals(SkillType.Area))
        {
            AreaMarker.SetActive(true);
            AreaMarker.transform.position = new Vector3(mousePos.x, 10, mousePos.z);

            // mouse 위치로 collider 위치 지정
        }
        // 타겟에 마우스포인터가 닿으면
        // 타겟마커가 타겟아래쪽으로 붙어야한다.
        else if (skill.skillType.Equals(SkillType.Target))
        {
            TargetMarker.SetActive(true);
            TargetMarker.transform.position = new Vector3(target.transform.position.x, 0.1f, target.transform.position.z);
        }
    }

    public void UnShowSkillMarker()
    {
        TargetMarker.SetActive(false);
        AreaMarker.SetActive(false);
    }
}
