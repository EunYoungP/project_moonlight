using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    // 각씬을 씬매니저 씬배열에 등록
    public void Awake()
    {
        RegisterScene();
    }

    public void RegisterScene()
    {
        LogoScene logo =  SceneMng.Instance.AddScript<LogoScene>(SceneState.LogoScene);
        SceneMng.Instance.AddScript<TitleScene>(SceneState.TitleScene);
        SceneMng.Instance.AddScript<SelectScene>(SceneState.SelectScene);
        SceneMng.Instance.AddScript<TownScene>(SceneState.TownScene);
        SceneMng.Instance.AddScript<GameScene>(SceneState.GameScene);
        SceneMng.Instance.AddScript<TrainingScene>(SceneState.TrainingScene);

        SceneMng.Instance.SetActive(SceneState.LogoScene);
        logo.LogoSceneStart();
    }
}
