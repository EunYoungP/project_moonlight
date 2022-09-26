using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UIType
{
    UILoading,
    Fade,
    UILevelUp,
    DungeonPage,
    UIPickUpItem,
}

public class UIMng : MonoBehaviour
{
    Dictionary<UIType, BaseUI> uiDic = new Dictionary<UIType, BaseUI>();

    private static UIMng instance = null;
    public static UIMng Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject("UIManager", typeof(UIMng));
                DontDestroyOnLoad(obj);

                instance = obj.GetComponent<UIMng>();
                instance.UIInit();
            }
            return instance;
        }
    }

    public void UIInit()
    {
        AddScript<UILoading>(UIType.UILoading);
        AddScript<Fade>(UIType.Fade);
        AddScript<UILevelUp>(UIType.UILevelUp);
        AddScript<DungeonPage>(UIType.DungeonPage);
        AddScript<UIPickUpItem>(UIType.UIPickUpItem);
    }
    
    // 게임이 시작할때 UIMng에 ui스크립트들을 가진 오브젝트들을 생성하고,
    // uiDictionary에 추가해주는 함수
    public void AddScript<T>(UIType uiType, bool isActive = false ) where T : BaseUI
    {
        if(!uiDic.ContainsKey(uiType))
        {
            ResourceManager.Instance.LoadUI<T>();
            GameObject prefab = GameObject.Instantiate(ResourceManager.Instance.UI, transform);
            if (prefab == null)
                return;
            DontDestroyOnLoad(prefab);
            prefab.SetActive(isActive);

            T t = prefab.GetComponent<T>();
            t.Init();
            uiDic.Add(uiType, t);
        }
    }

    public void UIUpdate(UIType uiType, float progress)
    {
        uiDic[uiType].UIUpdate(progress);
    }

    // 생성된 prefab에서 ui스크립트를 얻어와서 리턴해주는 함수
    public T OpenUI<T>(UIType uiType) where T : BaseUI
    {
        if(!uiDic.ContainsKey(uiType))
        {
            ResourceManager.Instance.LoadUI<T>();
            GameObject prefab = GameObject.Instantiate(ResourceManager.Instance.UI, transform);
            DontDestroyOnLoad(prefab);

            T t = GetComponent<T>();
            t.Init();
            t.Open();
            uiDic.Add(uiType, t);
            return t;
        }
        T tt =  uiDic[uiType].GetComponent<T>();
        tt.Open();
        return tt;
    }

    public void CloseUI(UIType uiType)
    {
        if (uiDic.ContainsKey(uiType))
        {
            uiDic[uiType].Close();
        }
    }
}
