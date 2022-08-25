using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestSign : MonoBehaviour
{
    public bool isActive = false;
    private float questsignOffset = 60f;

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
            RectTransform parentRect = parentCanvas.GetComponent<RectTransform>();
            RectTransform myRect = GetComponent<RectTransform>();

            Vector3 targetScreenPos = Camera.main.WorldToScreenPoint(targetPos.position);

            Vector2 localPosition = new Vector2(0, 0);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, targetScreenPos, null, out localPosition);

            myRect.localPosition = new Vector2( localPosition.x, localPosition.y + questsignOffset);
            yield return null;
        }
        yield return null;
    }
}
