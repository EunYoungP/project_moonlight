using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestSign : MonoBehaviour
{
    public bool isActive = false;
    private float questsignOffset = 60f;
    private RectTransform parentRect;
    private RectTransform myRect;
    private Vector3 targetScreenPos;
    private Vector2 localPosition;

    public void ActiveQuestSign(GameObject target ,Canvas canvas)
    {
        isActive = true;
        gameObject.SetActive(isActive);
        StartCoroutine(SetSignPos(target.transform, canvas));
    }

    public void OffQuestSign()
    {
        isActive = false;
        gameObject.SetActive(isActive);
    }

    IEnumerator SetSignPos(Transform targetPos, Canvas parentCanvas)
    {
        while(isActive)
        {
            parentRect = parentCanvas.GetComponent<RectTransform>();
            myRect = GetComponent<RectTransform>();

            targetScreenPos = Camera.main.WorldToScreenPoint(targetPos.position);

            localPosition = new Vector2(0, 0);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, targetScreenPos, null, out localPosition);

            myRect.localPosition = new Vector2( localPosition.x, localPosition.y + questsignOffset);
            yield return null;
        }
        yield return null;
    }
}
