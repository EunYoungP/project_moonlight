using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMenuPage : BaseGameUI
{
    public Button dungeonMenuBtn;
    public Button closeBtn;

    public override void Open()
    {
        gameObject.SetActive(true);
    }

    public override void Close()
    {
        gameObject.SetActive(false);
    }

    public override void Init()
    {
        AddMenuPageBtnListener();
    }

    private void AddMenuPageBtnListener()
    {
        // 던전페이지를 open 하는 함수 연결
        dungeonMenuBtn.onClick.AddListener(() => UIMng.Instance.OpenUI<DungeonPage>(UIType.DungeonPage));
        dungeonMenuBtn.onClick.AddListener(() => Close());
        //dungeonMenuBtn.onClick.AddListener(() => UIGameMng.Instance.CloseUI(UIGameType.Menu));
        //dungeonMenuBtn.onClick.AddListener(() => UIGameMng.Instance.CloseUI(UIGameType.Stat));
        //dungeonMenuBtn.onClick.AddListener(() => UIGameMng.Instance.CloseUI(UIGameType.Deck));
        dungeonMenuBtn.onClick.AddListener(() => UIGameMng.Instance.SetBasicScreenUI(false));

        closeBtn.onClick.AddListener(() => Close());
    }
}
