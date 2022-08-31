using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum UIGameType
{
    Stat,
    Menu,
    MenuPage,
    Inventory,
    Equipment,
    DetailPage,
    Deck,
    SkillBook,

    Notification,
    QuestPopup,
    SceneDesc,
}

public class UIGameMng : MonoBehaviour
{
    public Dictionary<UIGameType, BaseGameUI> uiGameDic = new Dictionary<UIGameType, BaseGameUI>();

    private static UIGameMng instance;
    public static UIGameMng Instance
    {
        get
        {
            if(instance == null)
            {
                GameObject obj = Instantiate( ResourceManager.Instance.GAMEUI );
                DontDestroyOnLoad(obj);
                instance = obj.GetComponent<UIGameMng>();
                instance.Init();
            }
            return instance;
        }
    }

    public void Init()
    {
        AddScript<UIStat>(UIGameType.Stat, true);
        AddScript<UIMenu>(UIGameType.Menu, true);
        AddScript<UIMenuPage>(UIGameType.MenuPage);
        AddScript<UIInventory>(UIGameType.Inventory);
        AddScript<UIEquipment>(UIGameType.Equipment);
        AddScript<UIDetailPage>(UIGameType.DetailPage);
        AddScript<UIDeck>(UIGameType.Deck, true);
        AddScript<UISkillBook>(UIGameType.SkillBook);
        AddScript<UINotification>(UIGameType.Notification);
        AddScript<UIQuestPopup>(UIGameType.QuestPopup);
        AddScript<UISceneDesc>(UIGameType.SceneDesc);
    }

    // dictionary에 script들을 추가하는 함수
    public void AddScript<T>(UIGameType uiType, bool isActive = false) where T : BaseGameUI
    {
        if (!uiGameDic.ContainsKey(uiType))
        {
            T t = GetComponentInChildren<T>(true);

            if (t == null)
                return;

            t.Init();
            t.gameObject.SetActive(isActive);

            uiGameDic.Add(uiType, t);
        }
    }

    // 다른 클래스에서 해당UI를 열때 사용하는 함수
    public void OpenUI<T>(UIGameType uiType) where T : BaseGameUI
    {
        if (!uiGameDic.ContainsKey(uiType))
        {
            AddScript<T>(uiType);
        }
        SetActive(uiType);
    }

    public T GetUI<T>(UIGameType uiType) where T : BaseGameUI
    {
        if(!uiGameDic.ContainsKey(uiType))
        {
            AddScript<T>(uiType);
        }
        T t = uiGameDic[uiType].GetComponent<T>();
        return t;
    }

    public void SetActive(UIGameType uiType)
    {
        foreach (UIGameType type in uiGameDic.Keys)
        {
            if (type == uiType)
            {
                uiGameDic[type].Open();
            }
        }
    }

    public void CloseUI(UIGameType uiType)
    {
        if (uiGameDic.ContainsKey(uiType))
        {
            uiGameDic[uiType].Close();
        }
    }

    public void SetBasicScreenUI(bool value)
    {
        GetUI<UIMenu>(UIGameType.Menu).gameObject.SetActive(value);
        GetUI<UIStat>(UIGameType.Stat).gameObject.SetActive(value);
        GetUI<UIDeck>(UIGameType.Deck).gameObject.SetActive(value);
    }

    public void LevelUpUpdate()
    {
        uiGameDic[UIGameType.Stat].LevelUpUpdate();
        uiGameDic[UIGameType.SkillBook].LevelUpUpdate();
    }
}
