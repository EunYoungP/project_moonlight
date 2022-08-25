using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : BaseUI
{
    private Image image;
    private bool isActive = false;
    
    private float delayTime;

    public override void UIInit()
    {
        image = GetComponentInChildren<Image>();
    }

    public void FadeIn(float delayTime = 2.0f)
    {
        this.delayTime = delayTime;
        StartCoroutine("CoFadeIn");
    }

    IEnumerator CoFadeIn()
    {
        float timer = 0;
        while (timer <= delayTime)
        {
            timer += Time.deltaTime / delayTime;
            image.color = Color.Lerp(Color.black, new Color(0, 0, 0, 0), timer);
            yield return null;
        }
        Close();
        yield return null;
    }

    public void FadeOut(float delayTime = 2.0f)
    {
        this.delayTime = delayTime;
        StartCoroutine("CoFadeOut");
    }

    IEnumerator CoFadeOut()
    {
        float timer = 0;
        while (timer <= delayTime)
        {
            timer += Time.deltaTime/ delayTime;
            image.color = Color.Lerp(new Color(0, 0, 0, 0), Color.black, timer);
            yield return null;
        }
        yield return null;
    }

    public override void Open()
    {
        if(!isActive)
        {
            gameObject.SetActive(true);
            isActive = true;
        }
    }

    public override void Close()
    {
        if (isActive)
        {
            gameObject.SetActive(false);
            isActive = false;
        }
    }
}
