using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestData 
{
    public string questName;
    public int[] npcId;
    public QuestType questType;
    public string[] details;
    public int exp;
    public int money;
    public string[] itemName;
    //public QuestRewardType[] rewards;
    //public int reward;

    // 퀘스트명, 퀘스트소유NPCID, 퀘스트타입, 보상금액, 알림글
    public QuestData(string questName, int[] npcId, QuestType questType,
                     int exp, int money, string[] itemName, string[] details) //QuestRewardType[] rewards
    {
        this.questName = questName;
        this.npcId = npcId;
        this.questType = questType;
        this.exp = exp;
        this.money = money;
        this.itemName = itemName;
        //this.rewards = rewards;
        this.details = details;
    }
}
