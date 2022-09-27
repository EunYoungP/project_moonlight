using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMng : MonoBehaviour
{
    private static GameMng instance;
    public static GameMng Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = GameObject.Find("GameMng");
                DontDestroyOnLoad(obj);

                instance = obj.GetComponent<GameMng>();
            }
            return instance;
        }
    }

    void Awake()
    {
        Instance.Init();
        //if(instance == null)
        //{
        //    GameObject obj = GameObject.Find("GameMng");
        //    DontDestroyOnLoad(obj);

        //    instance = obj.GetComponent<GameMng>();
        //    Init();
        //}
    }

    //SLManager.Instance.Init();
    //HpBarParent.Instance.Init();
    //MonsterManager.Instance.Init();
    //Player.Instance.Init();
    //SkillManager.Instance.Init();
    //UIGameMng.Instance.Init();
    //UIMng.Instance.UIInit();
    //WeaponManager.Instance.Init();
    //QuestManager.Instance.Init();
    private void Init()
    {
        ItemDB.Instance.InitItem();
        DownLoadAssetBundle.Instance.Init();
        HpBarParent.Instance.Init();
        SLManager.Instance.Init();
        ItemManager.Instance.Init();
        Player.Instance.Init();
        MonsterManager.Instance.Init();
        SkillManager.Instance.Init();
        UIGameMng.Instance.Init();
        UIMng.Instance.UIInit();
        WeaponManager.Instance.Init();
        QuestManager.Instance.Init();
    }
}
