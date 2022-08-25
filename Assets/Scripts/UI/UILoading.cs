using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILoading : BaseUI
{
    public List<Sprite> loadingImgList = new List<Sprite>();
    public Image bgImg;
    private Dictionary<SceneState, Sprite> loadingImgDic = new Dictionary<SceneState, Sprite>();
    private Slider loadImgBar;
    private bool isActive = false;
    private float delayTime = 2.0f;

    public override void UIInit()
    {
        loadImgBar = GetComponentInChildren<Slider>(true);
    }

    public override void UIUpdate(float progress)
    {
        loadImgBar.value = progress;
    }

    private void AddLoadingImg(SceneState sceneState)
    {
        if (loadingImgDic.ContainsKey(sceneState))
            return;

        foreach(Sprite loadingSprite in loadingImgList)
        {
            if (loadingSprite.name == sceneState.ToString())
                loadingImgDic[sceneState] = loadingSprite;
        }
    }

    public void SetLoadingImg(SceneState loadingScene)
    {
        if (!loadingImgDic.ContainsKey(loadingScene))
            AddLoadingImg(loadingScene);

        bgImg.sprite = loadingImgDic[loadingScene];
    }

    public override void Open()
    {
        if(!isActive)
        {
            gameObject.SetActive(true);
            isActive = true;
        }
    }

    public override void Close()
    {
        if(isActive == true)
        {
            gameObject.SetActive(false);
            bgImg.sprite = null;
            isActive = false;
        }
    }
}
