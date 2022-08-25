using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 타이머 구현하기
public class LogoScene : BaseScene
{
    private float fadeInTime = 3.0f;
    private float fadeOutTime = 3.5f;
    private float waitTime = 3.0f;

    public void LogoSceneStart()
    {
        UIMng.Instance.OpenUI<Fade>(UIType.Fade).FadeIn(fadeInTime);

        Invoke("FadeOut", fadeInTime + waitTime);
    }

    public void FadeOut()
    {
        UIMng.Instance.OpenUI<Fade>(UIType.Fade).FadeOut(fadeOutTime);

        Invoke("ChangeScene", fadeOutTime);
    }

    public void ChangeScene()
    {
        SceneMng.Instance.ChangeScene(SceneState.TitleScene, false);
    }
}
