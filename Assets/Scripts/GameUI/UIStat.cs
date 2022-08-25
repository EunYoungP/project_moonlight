using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStat : BaseGameUI
{
    public Text levelText;
    public Slider hpSlider;
    public Slider mpSlider;
    public Slider expSlider;

    public override void Init()
    {
        levelText.text = Player.Instance.data.level.ToString();
        hpSlider.value = (float)Player.Instance.data.curHp / Player.Instance.data.maxHp;
        mpSlider.value = Player.Instance.data.curMp / Player.Instance.data.maxMp;
    }

    public override void Open()
    {
        gameObject.SetActive(true);
    }

    public override void Close()
    {
        gameObject.SetActive(false);
    }

    // hp, mp는 여기서 처리 안하도록 수정해야함
    public override void LevelUpUpdate()
    {
        levelText.text = Player.Instance.data.level.ToString();
    }

    private void Update()
    {
        expSlider.value = (float)Player.Instance.data.exp / Player.Instance.data.maxExp;
        hpSlider.value = (float)Player.Instance.data.curHp / Player.Instance.data.maxHp;
        mpSlider.value = (float)Player.Instance.data.curMp / Player.Instance.data.maxMp;
    }
}
