using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum QuestType
{
    GET_ITEM,
    MONSTER_HUNT,
    TALK,
}

public class QuestManager : MonoBehaviour
{
    public enum QuestProgressType
    {
        BEFORE,
        PROGRESS,
        SUCCESS,
        COMPLETE,
    }

    public Action<QuestProgressType> ChangeQuestProgressTypeEvent;

    private QuestProgressType _questProgressType = QuestProgressType.BEFORE;
    public QuestProgressType questProgressType
    {
        get
        {
            return _questProgressType;
        }
        set
        {
            _questProgressType = value;
            if (ChangeQuestProgressTypeEvent != null)
                ChangeQuestProgressTypeEvent(_questProgressType);
        }
    }

    private int prevQuestId = 0;
    private int currQuestId = 10;
    private int questActionIndex = 0;
    private float questProgress;

    private bool startQuest = true;
    private string startNotification;

    // 퀘스트 목표설정
    public int questNpcID;
    public string questItemName;
    public int questHuntCount;
    public bool activeQuestUI;

    // <questId, QuestData>
    private Dictionary<int, QuestData> questDatas;
    public Dictionary<QuestType, Quest> questTypes = new Dictionary<QuestType, Quest>();

    private static QuestManager instance;
    public static QuestManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject("QuestManager", typeof(QuestManager));
                DontDestroyOnLoad(obj);
                instance = obj.GetComponent<QuestManager>();

                return instance;
            }
            return instance;
        }
    }

    public void Init()
    {
        ReisterQuestScript();
        GenerateQuestData();
        StartQuest();
    }

    private void ReisterQuestScript()
    {
        AddScript<Quest_MonsterHunt>(QuestType.MONSTER_HUNT);
        AddScript<Quest_GetItem>(QuestType.GET_ITEM);
        AddScript<Quest_Talk>(QuestType.TALK);
    }

    private T AddScript<T>(QuestType questType) where T : Quest
    {
        if (!questTypes.ContainsKey(questType))
        {
            GameObject obj = new GameObject(typeof(T).ToString(), typeof(T));

            T t = obj.GetComponent<T>();
            t.transform.parent = gameObject.transform;

            questTypes.Add(questType, t);

            return t;
        }
        return questTypes[questType].GetComponent<T>();
    }

    #region 퀘스트 데이터
    private void GenerateQuestData()
    {
        startNotification = "루인에게 말을 걸어야합니다.";

        questDatas = new Dictionary<int, QuestData>();
        questDatas.Add(10, new QuestData("루인과 대화하기",
                                        new int[] { 1000 },
                                        QuestType.TALK,
                                        1000,
                                        10000,
                                        null,
                                        //new QuestData.QuestRewardType[] { QuestData.QuestRewardType.EXP, QuestData.QuestRewardType.MONEY },
                                        new string[] { "루인 에게 다시 말을 걸어야 합니다." }));
        questDatas.Add(20, new QuestData("첫 마을방문 기념선물",
                                         new int[] { 1000 },
                                         QuestType.GET_ITEM,
                                        1000,
                                        10000,
                                        new string[] { "발두르의 검" },
                                         //new QuestData.QuestRewardType[] { QuestData.QuestRewardType.EXP, QuestData.QuestRewardType.MONEY, QuestData.QuestRewardType.ITEM },
                                         new string[] { }));
        questDatas.Add(30, new QuestData("첫번째 의뢰", 
                                        new int[] { 1000 },
                                        QuestType.TALK,
                                        1000,
                                        10000,
                                        null,
                                        //new QuestData.QuestRewardType[] { QuestData.QuestRewardType.EXP, QuestData.QuestRewardType.MONEY },
                                        new string[] { "북부평원에 있는 루카에게 가야합니다." }));
        questDatas.Add(40, new QuestData("초원골렘 토벌",
                        new int[] { 2000},
                        QuestType.TALK,
                        1000,
                        10000,
                        null,
                        //new QuestData.QuestRewardType[] { QuestData.QuestRewardType.EXP, QuestData.QuestRewardType.MONEY },
                        new string[] { "초원골렘 10 마리를 잡아야합니다." }));
        questDatas.Add(50, new QuestData("초원골렘 토벌 성공",
                new int[] { 2000 },
                QuestType.MONSTER_HUNT,
                1000,
                10000,
                null,
                //new QuestData.QuestRewardType[] { QuestData.QuestRewardType.EXP, QuestData.QuestRewardType.MONEY },
                new string[] { "루카에게 돌아가야합니다." }));
        questDatas.Add(60, new QuestData("초원골렘 토벌 완료",
                new int[] { 2000 },
                QuestType.TALK,
                1000,
                10000,
                null,
                //new QuestData.QuestRewardType[] { QuestData.QuestRewardType.EXP, QuestData.QuestRewardType.MONEY },
                new string[] { "세라보그성 에 있는 마리아에게 가야합니다." }));
        questDatas.Add(70, new QuestData("물건 배달", 
                                        new int[] { 3000},
                                        QuestType.TALK,
                                        1000,
                                        10000,
                                        null,
                                        //new QuestData.QuestRewardType[] { QuestData.QuestRewardType.EXP, QuestData.QuestRewardType.MONEY },
                                        new string[] { "혼돈의 입구 입장권으로 던전을 탐험해보세요." }));
        questDatas.Add(80, new QuestData("마리아의 보답",
                                new int[] { 3000 },
                                QuestType.GET_ITEM,
                                1000,
                                10000,
                                new string[] { "혼돈의 입구 입장권" },
                                //new QuestData.QuestRewardType[] { QuestData.QuestRewardType.EXP, QuestData.QuestRewardType.MONEY, QuestData.QuestRewardType.ITEM },
                                new string[] { "혼돈의 입구 입장권을 받았습니다." }));
        questDatas.Add(90, new QuestData("던전 탐험",
                                        new int[] { 3000 },
                                        QuestType.TALK,
                                        1000,
                                        10000,
                                        null,
                                        //new QuestData.QuestRewardType[] { QuestData.QuestRewardType.EXP, QuestData.QuestRewardType.MONEY },
                                        new string[] { "혼돈의 입구 입장권으로 던전을 탐험해보세요." }));
    }
    //루인 에게 다시 말을 걸어야 합니다.
    // "여우평원에 있는 루카에게 가야합니다."
    //"초원골렘 10 마리를 잡아야합니다." 
    //"루카에게 돌아가야합니다." 
    //"세라보그성 에 있는 마리아에게 가야합니다." 
    // "답례품을 받았습니다."
    #endregion

    #region 퀘스트진행
    public void StartQuest()
    {
        if (!startQuest)
            return;

        // 느낌표 UI
        //NPC startNPC = NPCManager.Instance.GetNPC(questDatas[currQuestId].npcId[questActionIndex]);
        //if (startNPC != null)
        //    startNPC.OnQuestSign();

        NPCManager.Instance.SetQuestWhenPlayerGet1stQuest();
        
        UIGameMng.Instance.uiGameDic[UIGameType.Notification].Open(startNotification);
        AddQuestByType();
        startQuest = false;
    }

    // QuestComplete 시점에 실행
    public void CheckQuestComplete(int npcId)
    {
        // 진행중인 퀘스트 없음
        if (questProgressType == QuestProgressType.BEFORE)
        {
            // 퀘스트 종료 검사
            if (questActionIndex == questDatas[currQuestId].npcId.Length -1
                && questDatas[currQuestId+10]!= null)
                NextQuest();
            // 연결퀘스트 존재 검사
            else if (npcId == questDatas[currQuestId].npcId[questActionIndex])
            {
                questActionIndex++;
            }
        }
        // 진행중인 퀘스트 존재
    }

    public void NextQuest()
    {
        prevQuestId = currQuestId;
        currQuestId += 10;
        questActionIndex = 0;

        AddQuestByType();
    }

    // 현재퀘스트 타입에 맞는 퀘스트 추가
    private void AddQuestByType()
    {
        QuestType currQuestType = questDatas[currQuestId].questType;
        SetQuest();
        questTypes[currQuestType].AddQuest();
    }

    public void CompleteQuestByType()
    {
        QuestType currQuestType = questDatas[currQuestId].questType;
        questTypes[currQuestType].CompleteQuest();
    }
    #endregion

    #region 퀘스트 정보 설정/제공
    // Talk/Hunt/Item Quest의 구체적인 목표 설정
    private void SetQuest()
    {
        switch (GetQuestTalkIndex())
        {
            case 10:
                questNpcID = 1000;
                break;
            case 20:
                //"발두르의 검"
                questItemName = "발두르의 검";
                AddGetItemWhenEndTalk();
                break;
            case 30:
                questNpcID = 1000;
                break;
            case 40:
                questNpcID = 2000;
                break;
            case 50:
                questHuntCount = 1;
                break;
            case 60:
                questNpcID = 2000;
                break;
            case 70:
                questNpcID = 3000;
                break;
            case 80:
                // 던전입장권 생성으로 변경
                questItemName = "혼돈의 입구 입장권";
                AddGetItemWhenEndTalk();
                break;
        }
    }

    // 현재 대화 인덱스 받아오기
    public int GetQuestTalkIndex()
    {
        return currQuestId + questActionIndex;
    }

    // 현재 퀘스트 받아오기
    public QuestData GetCurrQuestData()
    {
        return questDatas[currQuestId];
    }

    public QuestData GetPrevQuestData()
    {
        return questDatas[prevQuestId];
    }

    // 아이템지급 퀘스트 지급시점지정
    private void AddGetItemWhenEndTalk()
    {
        Player.Instance.GetItemWhenEndTalk += SetQuestItem;
    }

    public void RemoveGetItemQuest()
    {
        Player.Instance.GetItemWhenEndTalk -= SetQuestItem;
    }

    // Quest_GetItem 에서 QuestItem 지급
    private void SetQuestItem(string itemName)
    {
        foreach(Item questItem in ItemDB.Instance.itemDB)
        {
            if(questItem.name == itemName)
            {
                ItemObject newItemObject = DropItem.Instance.NewItemObect(questItem);
                //Player.Instance.QuestNotificationEvent(GetCurrQuestData());
                UIGameMng.Instance.uiGameDic[UIGameType.Inventory].AddItem(newItemObject);
                return;
            }
        }
    }
    #endregion

    #region 퀘스트UI

    //퀘스트완료 UI처리
    public void CompleteQuestUI(QuestData currQuestData)
    {
        FloatQuestPopup(currQuestData);

        Player.Instance.QuestNotificationEvent += FloatNoticeQuestText;
        //FloatNoticeQuestText(currQuestData);
    }

    // 퀘스트 알림판
    private void FloatQuestPopup(QuestData currQuestData)
    {
        if (currQuestData.questType == QuestType.TALK)
            Player.Instance.CloseQuestUIEvent += Player.Instance.TalkNpc;

        UIGameMng.Instance.uiGameDic[UIGameType.QuestPopup].Open(currQuestData);
    }

    // 퀘스트 알림글
    private void FloatNoticeQuestText(QuestData currQuestData)
    {
        UIGameMng.Instance.uiGameDic[UIGameType.Notification].Open(currQuestData.details[questActionIndex]);
    }
    #endregion
}
