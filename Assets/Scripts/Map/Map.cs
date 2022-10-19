using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 각 terrain/map 오브젝트에 부착됨
// 하나의 씬에 두개의 terrain 이 존재할 경우
public class Map : MonoBehaviour
{
    public MAP MapType;
    public string mapName;
    public SceneState mapScene;
    public bool isBattlePossible;
    public int monsterNumInMap;

    public List<Terrain> terrainList = new List<Terrain>();

    public void Init()
    {
        AddTerrainList();
    }

    // 맵오브젝트 하위 terrain들을 리스트에 담는기능
    private void AddTerrainList()
    {
        Terrain[] arr = gameObject.GetComponentsInChildren<Terrain>();

        foreach (Terrain t in arr)
        {
            terrainList.Add(t);
        }
    }
}
