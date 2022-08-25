using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UILevelUp : BaseUI
{
    public TextMeshProUGUI nextLevelTMPro;
    public TextMeshProUGUI currMaxHPTMPro;
    public TextMeshProUGUI nextMaxHPTMPro;
    public TextMeshProUGUI currMaxMPTMPro;
    public TextMeshProUGUI nextMaxMPTMPro;

    public Animator anim;

    public override void UIInit()
    {
    }

    private void SetUILevelUpText()
    {
        Character playerData = Player.Instance.data;

        nextLevelTMPro.text = playerData.level.ToString() + " 레벨을 달성했습니다!";
        currMaxHPTMPro.text = playerData.prevMaxHP.ToString();
        nextMaxHPTMPro.text = playerData.maxHp.ToString();
        currMaxMPTMPro.text = playerData.prevMaxMP.ToString();
        nextMaxMPTMPro.text = playerData.maxMp.ToString();
    }

    public override void Open()
    {
        SetUILevelUpText();

        gameObject.SetActive(true);
    }

    // 애니메이션 이벤트로 추가
    public override void Close()
    {
        if(anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        Close();
    }
}
