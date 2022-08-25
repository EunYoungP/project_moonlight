using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlideDeck : MonoBehaviour
{
    public Button PageBtn;
    public Transform firstPagePos;
    public Transform secondPagePos;

    private Transform currPos;

    private bool isStartPage = true;
    private float slideTime;

    private void Start()
    {
        gameObject.transform.position = firstPagePos.transform.position;
        currPos = gameObject.transform;
        isStartPage = true;
    }

    public void ChangePage()
    {
        slideTime = 0.3f;
        StartCoroutine("CoChangePage");
    }

    public void SetPageText()
    {
        Text pageText = PageBtn.gameObject.GetComponentInChildren<Text>();
        if (isStartPage)
            pageText.text = "1";
        else
            pageText.text = "2";
    }

    // 버튼을 누르면 다른페이지로 바꿔주는 기능
    IEnumerator CoChangePage()
    {
        float timer = 0f;
        if (isStartPage)
        {
            while (true)
            {
                timer += Time.deltaTime / slideTime;
                timer = Mathf.Clamp01(timer);
                currPos.transform.position = Vector3.Lerp(firstPagePos.transform.position, secondPagePos.transform.position, timer);
                if (timer >= 1.0f)
                    break;
                yield return null;
            }
            isStartPage = false;
            SetPageText();
            yield return null;
        }
        else if (!isStartPage)
        {
            while (true)
            {
                timer += Time.deltaTime / slideTime;
                timer = Mathf.Clamp01(timer);
                currPos.transform.position = Vector3.Lerp(secondPagePos.transform.position, firstPagePos.transform.position, timer);
                if (timer >= 1.0f)
                    break;
                yield return null;
            }
            isStartPage = true;
            SetPageText();
            yield return null;
        }
    }
}
