using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownScene : BaseScene
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
        UIGameMng.Instance.GetUI<UIMinimap>(UIGameType.Minimap).ChangeMapName(MapManager.Instance.currMap);
        SoundManager.Instance.Play("BGM/Enchanted garden", SOUND.Bgm);
        string currMapName = MapManager.Instance.currMap.mapName;
        UIGameMng.Instance.uiGameDic[UIGameType.SceneDesc].Open(currMapName);
    }
}
