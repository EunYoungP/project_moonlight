using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    public override void LoadScene(SceneState thisSceneState)
    {
        //thisSceneState = SceneState.GameScene;
        base.LoadScene(thisSceneState);
        PortalManager.Instance.CheckUsePortal();
    }

    public override void Enter(SceneState currScene)
    {
        base.Enter(currScene);
        SoundManager.Instance.Play("BGM/Aspiration Woods (Area Theme)", SOUND.Bgm);
        string currMapName = MapManager.Instance.currMap.mapName;
        UIGameMng.Instance.uiGameDic[UIGameType.SceneDesc].Open(currMapName);
    }
}
