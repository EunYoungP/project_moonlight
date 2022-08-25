using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// 몬스터의 위치를 로컬위치로 바꿔서 
// 빈게임오브젝트의 rectTransform 을 따라가게 만든다.
public class FloatingTextController : MonoBehaviour
{
    public GameObject target;

    private Canvas canvas;
    private Camera uiCamera;
    private RectTransform rectParent;
    private RectTransform rectFollowTarget;

    private TextMeshProUGUI damageText;

    public void Init()
    {
        if(target == null)
            target = GetComponentInParent<Monster>().gameObject;

        canvas = GetComponentInParent<Canvas>();
        uiCamera = canvas.worldCamera;
        rectParent = canvas.GetComponent<RectTransform>();
        rectFollowTarget = GetComponent<RectTransform>();
        damageText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetTextToCharacter()
    {
        Vector3 targetScreenPos = Camera.main.WorldToScreenPoint(target.transform.position);

        if (targetScreenPos.z < 0.0f)
            targetScreenPos *= -1.0f;

        Vector2 localPosition = new Vector2(0, 0);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, targetScreenPos, uiCamera, out localPosition);

        rectFollowTarget.localPosition = localPosition;

        DestroyCheck();
    }

    public void SetText(int damage)
    {
        damageText.text = damage.ToString();
    }

    public void DestroyCheck()
    {
        if (damageText.color.a == 0)
            Destroy(this.gameObject);
    }

    public void LateUpdate()
    {
        SetTextToCharacter();
    }
}
