using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MAP
{
    LOGO,
    TITLE,
    TRAING,
    GAME,
    TOWN,
}

public class MapManager : MonoBehaviour
{
    public Map currMap;
    Dictionary<SceneState, List<Terrain>> mapDic = new Dictionary<SceneState, List<Terrain>>();

    private static MapManager instance;
    public static MapManager Instance
    {
        get
        {
            if(instance == null)
            {
                GameObject obj = new GameObject("MapManager", typeof(MapManager));
                DontDestroyOnLoad(obj);

                instance = obj.GetComponent<MapManager>();
            }
            return instance;
        }
    }

    // 각씬 바뀔때마다 실행
    public void AddMapDic(SceneState nextSceneState)
    {
        Map currSceneMap = FindObjectOfType<Map>();
        if (currSceneMap == null) return;

        if(mapDic.ContainsKey(nextSceneState)) return;

        currSceneMap.Init();
        mapDic.Add(nextSceneState, currSceneMap.terrainList);
        currMap = currSceneMap;
    }
}
