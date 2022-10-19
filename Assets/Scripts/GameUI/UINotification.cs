using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UINotification : BaseGameUI
{
    public TextMeshProUGUI textMshPro;
    private float floatingTime = 3f;

    public Animator ani;
    private bool isActive = false;

    public override bool GetUIActiveState()
    {
        return isActive;
    }

    public override void Open(string noticeText)
    {
        isActive = true;
        textMshPro.text = noticeText;
        gameObject.SetActive(true);
        ani.SetBool("isActive", isActive);
        StartCoroutine(WaitFloating());
    }

    IEnumerator WaitFloating()
    {
        yield return new WaitForSeconds(floatingTime);
        Close();
    }

    public override void Close()
    {
        isActive = false;
        ani.SetBool("isActive", isActive);
        StartCoroutine(CheckClosing());
    }

    IEnumerator CheckClosing()
    {
        while(isActive)
        {
            if (ani.GetCurrentAnimatorStateInfo(0).IsName("FadeOut")&&
                ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                gameObject.SetActive(false);
            }
            yield return null;
        }
        yield return null;
    }
}
