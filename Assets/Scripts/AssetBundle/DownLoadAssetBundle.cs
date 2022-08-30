using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class DownLoadAssetBundle : MonoBehaviour
{
    [SerializeField]
    private string BundleURL;
    [SerializeField]
    private int version;

    private string atalsName;
    private WWW loader;

    [SerializeField]
    private Sprite loadSprite;

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
        atalsName = "SkillIconSpriteAtlas";
        BundleURL = "file://" + Application.dataPath + "/AssetBundles/bundle/skilliconassetbundle";
        StartCoroutine(DownloadAndCache());
        //LoadSkillSprite("DoubleSword_Skill0");
    }

    IEnumerator DownloadAndCache()
    {
        // 캐싱 시스템이 준비 될 때까지 대기
        while (!Caching.ready)
            yield return null;

        // 캐시에 에셋번들이 있다면 로드,
        // 없다면 다운로드하여 캐시폴더에 다운로드
        using (WWW loader = WWW.LoadFromCacheOrDownload(BundleURL, version))
        {
            yield return loader;
            if (loader.error != null)
                throw new Exception("WWW 다운로드에 에러가 발생했습니다." + loader.error);
        }
    }

    public void SetSkillBookSprite()
    {
        StartCoroutine(LoadSkillBookSprites());
    }

    public void SetSkillBookSlotSprite()
    {
        StartCoroutine(LoadSkillBookSlotSprites());
    }

    public void SetSkillSlotSprite()
    {
        StartCoroutine(LoadSkillSlotSprites());
    }

    IEnumerator LoadSkillBookSprites()
    {
        if (loader == null)
            loader = new WWW(BundleURL);

        yield return loader;

        SpriteAtlas atlas = loader.assetBundle.LoadAsset<SpriteAtlas>(atalsName);

        foreach(SkillPanel skillPanel in UIGameMng.Instance.GetUI<UISkillBook>(UIGameType.SkillBook).panelParent.skillPanels)
        {
            skillPanel.skillIcon.sprite = atlas.GetSprite(skillPanel.skill.skillIcon);
        }
        //loadSprite = atlas.GetSprite(spriteName);
    }
    
    IEnumerator LoadSkillBookSlotSprites()
    {
        if (loader == null)
            loader = new WWW(BundleURL);

        yield return loader;

        SpriteAtlas atlas = loader.assetBundle.LoadAsset<SpriteAtlas>(atalsName);

        foreach (SkillBookSlot skillBookSlot in UIGameMng.Instance.GetUI<UISkillBook>(UIGameType.SkillBook).skillBookDeck.skillBookSlots)
        {
            if(!skillBookSlot.isEmpty)
                skillBookSlot.skillIcon.sprite = atlas.GetSprite(skillBookSlot.Skill.skillIcon);
        }
        //loadSprite = atlas.GetSprite(spriteName);
    }

    IEnumerator LoadSkillSlotSprites()
    {
        if (loader == null)
            loader = new WWW(BundleURL);

        yield return loader;

        SpriteAtlas atlas = loader.assetBundle.LoadAsset<SpriteAtlas>(atalsName);

        foreach (SkillSlot skillSlot in UIGameMng.Instance.GetUI<UIDeck>(UIGameType.Deck).GetScript<SkillDeck>(DeckType.SkillDeck).SkillSlots)
        {
            if(!skillSlot.isEmptySlot)
                skillSlot.skillIcon.sprite = atlas.GetSprite(skillSlot.CurrSkill.skillIcon);
        }
        //loadSprite = atlas.GetSprite(spriteName);
    }
}
