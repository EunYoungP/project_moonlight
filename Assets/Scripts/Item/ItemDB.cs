using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.Linq;

// Item리스트를 가지고있는 클래스
// csv클래스의 Read함수를 이용하여 가져온 값을 Item에 대입하여 itemDB리스트에 담는다.
// Item클래스의 ReadItem으로 string으로 담아놓은 값들을 매핑하여 넣어준다.

public class ItemDB : csvReader
{
    private string csvFileURL;

    public List<Item> itemDB = new List<Item>();
    List<string> subjectStr = new List<string>();
        
    private static ItemDB instance;
    public static ItemDB Instance
    {
        get
        {
            if(instance == null)
            {
                GameObject obj = new GameObject("Item DB", typeof(ItemDB));
                DontDestroyOnLoad(obj);
                instance = obj.GetComponent<ItemDB>();
                
                if (instance != null)
                    instance.InitItem();

                return instance;
            }
            else
                return instance;
        }
    }

    private void InitItem()
    {
        csvFileURL = "https://drive.google.com/file/d/1d8Fg93kMcUvWasAZYfRERX3jce6KoS5J/view?usp=sharing";
        csvReaderInit(csvFileURL);

        StartCoroutine(ReadItem());
        //Debug.Log(itemDB[1]);
    }

    IEnumerator ReadItem()
    {
        while(!IsDownLoading)
        {
            yield return null;
        }

        List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();

        subjectStr = ReadSubject(csvFileURL);
        list = Read(csvFileURL);

        // 아이템의 개수
        // csv파일에서 받아온 정보들을 Item에 담아서
        // ItemList에 담는다.
        for(int i = 0; i<list.Count; ++i)
        {
            Item item = new Item();

            List<string> valueList = new List<string>();

            for(int j = 0; j<subjectStr.Count; ++j)
            {
                valueList.Add(list[i][subjectStr[j]]);
            }
            item.ReadItem(valueList);
            AddItem(item);
        }
        ResourceManager.Instance.LoadItemIcon();
    }

    public void AddItem(Item item)
    {
        itemDB.Add(item);
    }

    public void RemoveItem(Item item)
    {
        itemDB.Remove(item);
    }

    public Item GetItemByName(string itemName)
    {
        foreach (Item item in itemDB)
        {
            if (item.name == itemName)
                return item;
        }
        return null;
    }
}
