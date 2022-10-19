using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseScene : MonoBehaviour
{
    public virtual void UpdateScene()
    {
    }

    public virtual void LoadScene(SceneState enterSceneState)
    {
    }

    public virtual void Enter(SceneState enterSceneState)
    {
        MapManager.Instance.AddMapDic(enterSceneState);
        MonsterManager.Instance.CreateMonster();
        UIMng.Instance.OpenUI<Fade>(UIType.Fade).FadeIn(2.0f);
    }

    public virtual void Exit()
    {
    }
}
