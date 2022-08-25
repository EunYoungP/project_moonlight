using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingScene : BaseScene
{
    public override void LoadScene(SceneState thisSceneState)
    {
        //thisSceneState = SceneState.TrainingScene;
        base.LoadScene(thisSceneState);
        PortalManager.Instance.CheckUsePortal();
    }

    public override void Enter(SceneState currScene)
    {
        base.Enter(currScene);
        SoundManager.Instance.Play("BGM/In a good place", SOUND.Bgm);
        string currMapName = MapManager.Instance.currMap.mapName;
        UIGameMng.Instance.uiGameDic[UIGameType.SceneDesc].Open(currMapName);
    }
}
