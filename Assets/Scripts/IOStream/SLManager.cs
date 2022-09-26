using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using UnityEngine.Networking;

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

    private UnityWebRequest request;
    private string inventoryIniturl;
    private string fileData;

    private string downloadInventoryDBurl;
    private string uploadInventoryDBurl;

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
            }
            return instance;
        }
    }

    public void Init()
    {
        SetURL();

        InitCharcater();
        InitInventoryItem();
    }

    private void SetURL()
    {
        inventoryIniturl = "https://drive.google.com/file/d/1VfgOEwluuSyoS4o291PCc34vl8IoW9PD/view?usp=sharing";
        downloadInventoryDBurl = "https://drive.google.com/file/d/1_bz1p68jXbOzl-te8k6MtEhfcG4NcXNw/view?usp=sharing";
        uploadInventoryDBurl = "https://drive.google.com/file/d/1_bz1p68jXbOzl-te8k6MtEhfcG4NcXNw/view?usp=sharing";
    }

    #region character

    private void InitCharcater()
    {
        if (characterDataDic.Count > 0)
            return;

        characterDataDic["Player1"] = new Character("Player1", 2, 1000, 0, 1000, 1000, 1000, 1000, 100, 300, 0, 0, 0, 0, false);

        for (int i = 0; i < MonsterManager.Instance.monsterInRange; i++)
        {
            characterDataDic["Golem" + (i + 1).ToString()] = new Character("Golem" + (i + 1).ToString(), 1, 0, 555, 1000, 1000, 0, 0, 300, 400, 0, 0, 0, 0, false);
        }
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

    public void SaveCharacterData()
    {
        string jdata = JsonConvert.SerializeObject(characterDataDic);

        // 암호화
        //byte[] bytes = System.Text.Encoding.UTF8.GetBytes(jdata);
        //string format = System.Convert.ToBase64String(bytes);

        File.WriteAllText(Application.persistentDataPath + "/Resources/DBfile/CharacterData.json", jdata);
    }

    public void LoadCharacterData()
    {
        string jdata = string.Empty;
        string path = "DBfile/CharacterData.json";
        TextAsset data = (TextAsset)Resources.Load(path, typeof(TextAsset));
        if(data != null)
        {
            jdata = data.text;
        }
        //string jdata = File.ReadAllText(Application.dataPath + "/Resources/DBfile/CharacterData.json");

        // 암호해독
        // 오류발생부분
        //byte[] bytes = System.Convert.FromBase64String(jdata);
        //string reformat = System.Text.Encoding.UTF8.GetString(bytes);

        characterDataDic = JsonConvert.DeserializeObject<Dictionary<string, Character>>(jdata);

        print(characterDataDic["Player01"].curHp);
    }

    #endregion

    private void InitInventoryItem()
    {
        // GoogleDrvie file Load/Save
        #region using googledrive 
        //if (request == null)
        //{
        //    StartCoroutine(DownLoadJson(inventoryIniturl));
        //    return;
        //}

        //다운로드가 처음이 아닐경우
        //if (curInventoryItem.Count > 0)
        //{
        //    //SaveInventoryData();
        //    LoadDataInLocal();
        //    return;
        //}
        #endregion

        string filePath = Application.persistentDataPath + "/InventoryDB.json";
        // 새게임
        if (!File.Exists(filePath))
        {
            StartCoroutine(DownLoadJson(inventoryIniturl, filePath));
            return;
        }
        else
        {
            LoadDataInLocal();
        }
    }

    private string ExtractFileURL(string url)
    {
        url = url.Replace("https://drive.google.com/file/d/", "");
        url = url.Replace("/view?usp=sharing", "");

        string prefix = "http://drive.google.com/uc?export=view&id=";
        url = prefix + url;

        return url;
    }

    public void SaveDataInLocal()
    {
        string path = Application.persistentDataPath + "/InventoryDB.json";
        curInventoryItem = UIGameMng.Instance.GetUI<UIInventory>(UIGameType.Inventory).inventoryItems;
        string jdata = JsonConvert.SerializeObject(curInventoryItem);

        File.WriteAllText(path, jdata);
    }

    // Android dataPath : C:/Users/Park E.Y/AppData/LocalLow/dobby/moonlight
    public void LoadDataInLocal()
    {
        string filePath = Application.persistentDataPath + "/InventoryDB.json";
        string jdata;

        if (!File.Exists(filePath))
        {
            StartCoroutine(DownLoadJson(inventoryIniturl, filePath));
            return;
        }
        else
        {
            jdata = File.ReadAllText(filePath);
            curInventoryItem = JsonConvert.DeserializeObject<List<ItemObject>>(jdata);
            //UIGameMng.Instance.GetUI<UIInventory>(UIGameType.Inventory).InitItem();
        }
    }

    public void UpLoadInventoryData()
    {
        string jdata = JsonConvert.SerializeObject(curInventoryItem);
        StartCoroutine(UpLoadJson(uploadInventoryDBurl, jdata));
    }

    public void DownLoadInventoryInitData()
    {
    }

    private IEnumerator DownLoadJson(string fileURL, string filePath)
    {
        var gdfileURL = ExtractFileURL(fileURL);

        request = UnityWebRequest.Get(gdfileURL);
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
        }
        else
        {
            fileData = request.downloadHandler.text;
            curInventoryItem = JsonConvert.DeserializeObject<List<ItemObject>>(fileData);
            //UIGameMng.Instance.GetUI<UIInventory>(UIGameType.Inventory).InitItem();
        }
    }

    private IEnumerator UpLoadJson(string fileURL, string jdata)
    {
        var filePath = ExtractFileURL(fileURL);

        using (UnityWebRequest request = UnityWebRequest.Post(filePath, jdata))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jdata);
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();
            if (request.isHttpError || request.isNetworkError)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log(request.downloadHandler.text);
            }
        }
    }

    private void CreateInitJson()
    {
        foreach(Item item in ItemDB.Instance.itemDB)
        {
            if(item.name == "심장석"
                || item.name == "두툼한 고기 덩어리"
                || item.name == "HP 회복 물약"
                || item.name == "MP 회복 물약"
                || item.name == "빵")
            {
                curInventoryItem.Add(DropItem.Instance.NewItemObect(item));
            }
        }
        string jdata = JsonConvert.SerializeObject(curInventoryItem);
        Debug.Log("현재 인벤토리 정보 : " + jdata);
        File.WriteAllText(Application.dataPath + "/Resources/DBfile/InitInventoryItem.json", jdata);
    }

    private void ParsingData(string jdata)
    {
        StringReader sr = new StringReader(jdata);
        string[] lines = new string[5];

        int index = 0;
        while (true)
        {
            string line = sr.ReadLine();
            if (line == null) break;

            lines[index] = line;
            index++;
        }

        for (int i = 0; i < lines.Length; i++)
        {
            string[] row = lines[i].Split('\t');
            List<string> rowList = row.ToList();

            Item item = new Item();
            item.ReadItem(rowList);

            ItemObject newItemObject = DropItem.Instance.NewItemObect(item);
            //inventoryInitItem.Add(newItemObject);
        }
    }

    private void OnApplicationQuit()
    {
       SaveDataInLocal();
    }
}
