﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NPCManager : MonoBehaviour
{
    public enum NPCProgressType
    {
        BEFORE,
        PROGRESS,
        SUCCESS,
        COMPLETE,
    }

    public Dictionary<int, NPC> NPCdic = new Dictionary<int, NPC>();    // 모든 NPC정보, <NPCid,NPC>
    public NPC selectedNpc;

    private static NPCManager instance;
    public static NPCManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject("NPCManager", typeof(NPCManager));
                DontDestroyOnLoad(obj);
                instance = obj.GetComponent<NPCManager>();
                instance.Init();

                return instance;
            }
            return instance;
        }
    }

    public Action<NPCProgressType> ChangeNPCProgressTypeEvent;      // NPC 진행상태 변화 이벤트
    private NPCProgressType _npcProgressType = NPCProgressType.BEFORE;
    public NPCProgressType npcProgressType
    {
        get
        {
            return _npcProgressType;
        }
        set
        {
            _npcProgressType = value;
            if (ChangeNPCProgressTypeEvent != null)
                ChangeNPCProgressTypeEvent(_npcProgressType);
        }
    }

    private void Init()
    {
        Player.Instance.TalkNpcEvent += SetClickNpcId;
    }

    private void SetClickNpcId(NPC clickNpc)
    {
        selectedNpc = clickNpc;
    }

    public void SetQuestWhenPlayerGet1stQuest()
    {
        QuestManager.Instance.ChangeQuestProgressTypeEvent += ChangeQuestProgressType;
    }

    public void SetQuestWhenPlayerGetQuest()
    {
        // 퀘스트 종류를 구분하여 각 퀘스트를 받았을때 NPC 세팅, 행동 결정
    }

    private void ChangeQuestProgressType(QuestManager.QuestProgressType questProgressType)
    {
        switch(questProgressType)
        {
            case QuestManager.QuestProgressType.BEFORE:
                npcProgressType = NPCProgressType.BEFORE;
                break;
            case QuestManager.QuestProgressType.PROGRESS:
                npcProgressType = NPCProgressType.PROGRESS;
                break;
            case QuestManager.QuestProgressType.SUCCESS:
                npcProgressType = NPCProgressType.SUCCESS;
                QuestManager.Instance.CompleteQuestByType();
                break;
        }
    }

    private void Update()
    {
        switch(npcProgressType)
        {
            case NPCProgressType.SUCCESS:
            break;
        }
    }

    public void AddScript(int npcId, NPC addNPC)
    {
        if (NPCdic.ContainsKey(npcId))
            return;

        NPCdic[npcId] = addNPC;
    }

    public NPC GetNPC(int npcId)
    {
        if (!NPCdic.ContainsKey(npcId))
            return null;

        return NPCdic[npcId];
    }
}
