using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    // 각 맵에따른 몬스터리스트
    public Dictionary<MAP, List<Monster>> monsterDic = new Dictionary<MAP, List<Monster>>();
    public List<Monster> deadMonsterList = new List<Monster>();

    public int monsterInRange;
    public float colX;
    public float colY;
    float terrRadius;
    float playerRange = 10;
    
    private static MonsterManager instance = null;
    public static MonsterManager Instance
    {
        get
        {
            if(instance == null)
            {
                GameObject obj = new GameObject("Monster Manager", typeof(MonsterManager));
                DontDestroyOnLoad(obj);

                instance = obj.GetComponent<MonsterManager>();
            }
            return instance;
        }
    }

    public void Init()
    {
        //SLManager.Instance.Init();
        CreateMonster();
    }

    // 맵마다 생성되어야하는 몬스터 수를 받아와서 생성하고
    //InRanage 안에있는 몬스터들만 활성화해주는 코드로 변경해야함
    public void CreateMonster()
    {
        Map currMap = MapManager.Instance.currMap;
        if (currMap == null) return;
        if (currMap.isBattlePossible == false)
            return;

        for(int i = 0; i< currMap.monsterNumInMap; ++i)
        {
            SLManager.Instance.CreateGolemData(currMap.MapType, currMap.monsterNumInMap);

            GameObject newMon = Instantiate(ResourceManager.Instance.GOLEMR);
            newMon.name = "Golem" + (i + 1).ToString();
            SetRandomPos(newMon);
            Monster monScript = newMon.GetComponent<Monster>();

            AddMonsterDic(currMap.MapType, monScript);
            //monsterDic.Add(currMap.MapType,monScript);
        }
        //isCreate = true;
    }

    // 오류 확인
    // monsterDic[mapType].Add 해도 값이 추가가 안됨.
    private void AddMonsterDic(MAP mapType, Monster monsterScript)
    {
        // 키가 포함되어있지 않다면
        if(!monsterDic.ContainsKey(mapType))
        {
            List<Monster> monsterList = new List<Monster>();
            monsterList.Add(monsterScript);

            monsterDic.Add(mapType, monsterList);
        }
        else
        {
            monsterDic[mapType].Add(monsterScript);
        }
        monsterScript.MapType = mapType;
    }

    void SetRandomPos(GameObject obj)
    {
        // 임시로 모두 0번째 terrain을 받아옴
        Terrain currMapTerrain = MapManager.Instance.currMap.terrainList[0];
        terrRadius = currMapTerrain.terrainData.size.x;

        Vector3 tmp = obj.transform.position;
        tmp.x = Random.Range(currMapTerrain.GetPosition().x, terrRadius);
        tmp.z = Random.Range(currMapTerrain.GetPosition().z, terrRadius);
        obj.transform.position = tmp;
    }

    public void MonsterDead(Monster monster)
    {
        monsterDic[monster.MapType].Remove(monster);
        deadMonsterList.Add(monster);

        monster.AfterDead(monster);
    }
}
