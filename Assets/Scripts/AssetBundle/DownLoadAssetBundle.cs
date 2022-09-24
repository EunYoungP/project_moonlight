using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.Networking;

public class DownLoadAssetBundle : MonoBehaviour
{
    [SerializeField] private string BundleURL;
    [SerializeField] private int version;
    [SerializeField] private string atlasName;

    private UnityWebRequest www;
    private SpriteAtlas skillIconSpriteAtlas;

    private static DownLoadAssetBundle instance;
    public static DownLoadAssetBundle Instance
    {
        get
        {
            if(instance == null)
            {
                GameObject obj = new GameObject("BundleDownLoad", typeof(DownLoadAssetBundle));
                obj.transform.parent = GameMng.Instance.transform;
                DontDestroyOnLoad(obj);

                instance = obj.GetComponent<DownLoadAssetBundle>();
            }
            return instance;
        }
    }

    public void Init()
    {
        atlasName = "SkillIconSpriteAtlas";
        BundleURL = "https://drive.google.com/file/d/1Lges1NYpL__EpV3Si3mhyPTPB8Eq8SLU/view?usp=sharing";
        StartCoroutine(DownLoadBundle());
    }

    private string ExtractFileID(string url)
    {
        url = url.Replace("https://drive.google.com/file/d/", "");
        url = url.Replace("/view?usp=sharing", "");
        return url;
    }

    IEnumerator DownLoadBundle()
    {
        var gdID = ExtractFileID(BundleURL);
        var prefix = "http://drive.google.com/uc?export=view&id=";
        BundleURL = prefix + gdID;

        www = UnityWebRequestAssetBundle.GetAssetBundle(BundleURL);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("오류 : " + www.error);
        }
        else
        {
            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);
            skillIconSpriteAtlas = bundle.LoadAsset<SpriteAtlas>(atlasName);
        }
    }

    public void SetSkillBookSprite()
    {
        if (skillIconSpriteAtlas != null)
        {
            foreach (SkillPanel skillPanel in UIGameMng.Instance.GetUI<UISkillBook>(UIGameType.SkillBook).panelParent.skillPanels)
            {
                skillPanel.skillIcon.sprite = skillIconSpriteAtlas.GetSprite(skillPanel.skill.skillIcon);
            }
        }
        else
        {
            Debug.Log("아틀라스에 값이 할당되지 않았습니다.");
        }
    }

    public void SetSkillBookSlotSprite()
    {
        foreach (SkillBookSlot skillBookSlot in UIGameMng.Instance.GetUI<UISkillBook>(UIGameType.SkillBook).skillBookDeck.skillBookSlots)
        {
            if (!skillBookSlot.isEmpty)
                skillBookSlot.skillIcon.sprite = skillIconSpriteAtlas.GetSprite(skillBookSlot.Skill.skillIcon);
        }
    }

    public void SetSkillSlotSprite()
    {
        foreach (SkillSlot skillSlot in UIGameMng.Instance.GetUI<UIDeck>(UIGameType.Deck).GetScript<SkillDeck>(DeckType.SkillDeck).SkillSlots)
        {
            if (!skillSlot.isEmptySlot)
                skillSlot.skillIcon.sprite = skillIconSpriteAtlas.GetSprite(skillSlot.CurrSkill.skillIcon);
        }
    }
}
