using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

public class DungeonPage : BaseUI
{
    private Dictionary<int, DungeonData> dungeonDataDic = new Dictionary<int, DungeonData>();
    private Dictionary<int, Button> dungeonBtnDic = new Dictionary<int, Button>();

    // 오브젝트 
    public TextMeshProUGUI dungeonMenuName;
    public TextMeshProUGUI dungeonName;
    public TextMeshProUGUI enterLevel;
    public TextMeshProUGUI dungeonDesc;
    public Image dungeonImg;

    public Button closeBtn;
    public GameObject dungeonTypesParent;
    private StringBuilder sb;

    public override void UIInit()
    {
        sb = new StringBuilder(100);
        GetDungeonMenuBtns();
        GenerateDungeonData();
        AddListener();
    }

    public override void Open()
    {
        gameObject.SetActive(true);
    }

    public override void Close()
    {
        gameObject.SetActive(false);
    }

    // 던전 데이터를 키와 함께 저장
    private void GenerateDungeonData()
    {
        DungeonData dungeonData = new DungeonData("초급 수련관",
                                                    2,
                                                    "세라보그성 외곽에 위치한 하수도에서는 알 수 없는 소리가 나고 있습니다.",
                                                    "초급 수련관",
                                                    "혼돈의 입구 입장권");
        dungeonDataDic[dungeonDataDic.Count] = dungeonData;
    }

    // 던전메뉴 버튼을 키와 함께 저장
    private void GetDungeonMenuBtns()
    {
        Button[] btnArg;
        btnArg = dungeonTypesParent.GetComponentsInChildren<Button>(true);

        for(int i = 0; i < btnArg.Length; i++)
        {
            dungeonBtnDic[i] = btnArg[i];
        }
    }

    private void AddListener()
    {
        for(int i = 0; i < dungeonBtnDic.Count; i++)
        {
            dungeonBtnDic[i].onClick.AddListener(() => SetDungeonData(i));
        }

        closeBtn.onClick.AddListener(() => Close());
        closeBtn.onClick.AddListener(() => UIGameMng.Instance.SetBasicScreenUI(true));

    }

    private void SetDungeonData(int dungeonKey)
    {
        this.dungeonMenuName.text = dungeonDataDic[dungeonKey].DungeonName;
        this.dungeonName.text = dungeonDataDic[dungeonKey].DungeonName;
        sb.Append("입장 가능 레벨 : ").Append(dungeonDataDic[dungeonKey].DungeonEnterLevel.ToString());
        this.enterLevel.text = sb.ToString();
        sb.Clear();
        this.dungeonDesc.text = dungeonDataDic[dungeonKey].DungeonDesc;
        foreach(Image img in ResourceManager.Instance.DUNGEON_IMG)
        {
            if (img.name == dungeonDataDic[dungeonKey].DungeonImg)
                this.dungeonImg = img;
        }
    }
}
