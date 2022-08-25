using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UISceneDesc : BaseGameUI
{
    public TextMeshProUGUI SceneNameTxtMshPro;
    private float fadeOutTime = 2.0f;
    private float fadeInTime = 2.0f;
    private float stateTrueTime = 3.0f;

    public override void Open(string noticeText)
    {
        SceneNameTxtMshPro.text = noticeText;
        gameObject.SetActive(true);
        FadeIn();
    }

    public override void Close()
    {
        gameObject.SetActive(false);
    }

    private void FadeOut()
    {
        StartCoroutine("CoFadeOut");
    }

    IEnumerator CoFadeOut()
    {
        float timer = 0;
        while (timer <= fadeOutTime)
        {
            timer += Time.deltaTime / fadeOutTime;
            SceneNameTxtMshPro.color = Color.Lerp( Color.white, new Color(1, 1, 1, 0), timer);
            yield return null;
        }
        Close();
        yield return null;
    }

    private void FadeIn()
    {
        StartCoroutine("CoFadeIn");
    }

    IEnumerator CoFadeIn()
    {
        float timer = 0;
        while (timer <= fadeInTime)
        {
            //timer += Time.deltaTime / fadeInTime;
            timer += Time.deltaTime;
            float interpolateValue = timer / fadeInTime;
            SceneNameTxtMshPro.color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, interpolateValue);
            yield return null;
        }
        yield return new WaitForSeconds(stateTrueTime);
        FadeOut();
    }
}
