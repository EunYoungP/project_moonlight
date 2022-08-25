using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.IO;
using System.Linq;

public class Character
{
    public string name;
    public int level;
    public int maxExp;
    public int exp;
    public int maxHp;
    public int curHp;
    public int maxMp;
    public int curMp;
    public int minAttack;
    public int maxAttack;
    public int defense;
    public int ciriticalAttackPercent;
    public int criticalAttackPower;
    public int skipDamagedMove;
    public bool isDead;

    public int prevMaxHP;
    public int prevMaxMP;

    public Character(string name, int level, int maxExp, int exp,
        int maxHp, int curHp, int maxMp, int curMp, int minAttack,
        int maxAttack, int defense, int ciriticalAttackPercent, 
        int criticalAttackPower, int skipDamagedMove, bool isDead)
    {
        this.name = name;
        this.level = level;
        this.maxExp = maxExp;
        this.exp = exp;
        this.maxHp = maxHp;
        this.curHp = curHp;
        this.maxMp = maxMp;
        this.curMp = curMp;
        this.minAttack = minAttack;
        this.maxAttack = maxAttack;
        this.defense = defense;
        this.ciriticalAttackPercent = ciriticalAttackPercent;
        this.criticalAttackPower = criticalAttackPower;
        this.skipDamagedMove = skipDamagedMove;
        this.isDead = isDead;
    }

    public int ATTACKPOWER
    { get { return Random.Range(minAttack, maxAttack + 1); } }

    public void DecreseHp(int enemyAttack)
    {
        curHp -= enemyAttack;

        if (curHp < 0)
            isDead = true;
    }

    private void IncreseMaxHP()
    {
        prevMaxHP = maxHp;
        maxHp = Mathf.RoundToInt(maxHp + 1.5f);
    }

    private void IncreseMaxMP()
    {
        prevMaxMP = maxMp;
        maxMp = Mathf.RoundToInt(maxMp + 1.5f);
    }

    private void IncreseMaxExp()
    {
        exp = exp - maxExp;
        maxExp = Mathf.RoundToInt(maxExp * 1.5f);
    }

    public void LevelUpdate(Character target)
    {
        exp += target.exp;

        // 레벨업
        if (exp > maxExp)
        {
            level++;
            IncreseMaxExp();
            IncreseMaxHP();
            IncreseMaxMP();

            UIGameMng.Instance.LevelUpUpdate();
            UIMng.Instance.OpenUI<UILevelUp>(UIType.UILevelUp).Open();
            EffectManager.Instance.GetScript<PlayerEffect>(EffectObject.Player).OnLevelUpVFX();
        }
        //UIStat uiStat = GameObject.FindObjectOfType<UIStat>();
        //uiStat.LevelUpUpdate();
    }
}

[System.Serializable]
public class SLManager : MonoBehaviour
{
    [SerializeField]
    public Dictionary<string, Character> characterDataDic = new Dictionary<string, Character>();
    public Dictionary<MAP, List<Character>> golemDataDic = new Dictionary<MAP, List<Character>>();
    public List<ItemObject> inventoryInitItem = new List<ItemObject>();
    public List<ItemObject> curInventoryItem = new List<ItemObject>();

    private static SLManager instance;
    public static SLManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject("SLManager", typeof(SLManager));
                DontDestroyOnLoad(obj);

                instance = obj.GetComponent<SLManager>();
                instance.Init();
            }
            return instance;
        }
    }

    public void Init()
    {
        if (characterDataDic.Count > 0)
            return;

        characterDataDic["Player1"] = new Character("Player1", 2,1000, 0, 1000, 1000, 1000, 1000, 100, 300, 0, 0, 0, 0, false);

        for(int i = 0; i<MonsterManager.Instance.monsterInRange; i++)
        {
            characterDataDic["Golem"+(i+1).ToString()] = new Character("Golem"+ (i + 1).ToString(), 1,0, 555, 1000, 1000, 0, 0, 300, 400, 0, 0, 0, 0, false);
        }

        InitInventoryItem();
    }

    public void CreateGolemData(MAP mapType, int golemNum)
    {
        List<Character> golemDataList = new List<Character>();
        for (int i = 0; i < golemNum; i++)
        {
            golemDataList.Add(new Character("Golem" + (i + 1).ToString(), 1, 0, 555, 1000, 1000, 0, 0, 300, 400, 0, 0, 0, 0, false));
        }
        golemDataDic[mapType]= golemDataList;
    }

    public Character LoadGolemData(MAP mapType, string keyCode )
    {
        if (!golemDataDic.ContainsKey(mapType))
            return null;

        foreach(Character golemData in golemDataDic[mapType])
        {
            if (golemData.name == keyCode)
                return golemData;
        }
        return null;
    }

    public void SaveData()
    {
        string jdata = JsonConvert.SerializeObject(characterDataDic);

        // 암호화
        //byte[] bytes = System.Text.Encoding.UTF8.GetBytes(jdata);
        //string format = System.Convert.ToBase64String(bytes);

        File.WriteAllText(Application.dataPath + "/CharacterData.json", jdata);
    }

    public void LoadData()
    {
        string jdata = File.ReadAllText(Application.dataPath + "/CharacterData.json");

        // 암호해독
        // 오류발생부분
        //byte[] bytes = System.Convert.FromBase64String(jdata);
        //string reformat = System.Text.Encoding.UTF8.GetString(bytes);

        characterDataDic = JsonConvert.DeserializeObject<Dictionary<string, Character>>(jdata);

        print(characterDataDic["Player01"].curHp);
    }

    private void InitInventoryItem()
    {
        if (curInventoryItem.Count > 0)
        {
            SaveInventory();
            LoadInventory();
            return;
        }

        string[] line = File.ReadAllLines(Application.dataPath + "/DBfile/InitInventoryData.txt");

        for(int i =0; i<line.Length; i++)
        {
            string[] row = line[i].Split('\t');
            List<string> rowList = row.ToList();

            Item item = new Item();
            item.ReadItem(rowList);

            ItemObject newItemObject = DropItem.Instance.NewItemObect(item);
            inventoryInitItem.Add(newItemObject);
        }
    }

    public void SaveInventory()
    {
        string jdata = JsonConvert.SerializeObject(curInventoryItem);
        File.WriteAllText(Application.dataPath + "/DBfile/InventoryDB.json", jdata);
    }

    public void LoadInventory()
    {
        string jdata = File.ReadAllText(Application.dataPath + "/DBfile/InventoryDB.json");
        curInventoryItem = JsonConvert.DeserializeObject<List<ItemObject>>(jdata);
    }

    private void OnApplicationQuit()
    {
        SaveInventory();
    }
}
