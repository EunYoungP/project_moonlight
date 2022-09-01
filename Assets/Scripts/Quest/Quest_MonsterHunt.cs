using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest_MonsterHunt : Quest
{
    private int currHuntCount;
    private int questCount;

    public override void AddQuest()
    {
        Player.Instance.MonsterHuntEvent += ProgressQuest;
        Player.Instance.AddQuest(QuestManager.Instance.GetCurrQuestData());
        QuestManager.Instance.questProgressType = QuestManager.QuestProgressType.PROGRESS;

        questCount = QuestManager.Instance.questHuntCount;
    }

    public override void RemoveQuest()
    {
        QuestManager.Instance.questProgressType = QuestManager.QuestProgressType.BEFORE;
        Player.Instance.MonsterHuntEvent -= ProgressQuest;
        Player.Instance.RemoveQuest(QuestManager.Instance.GetCurrQuestData());
        currHuntCount = 0;
    }

    // MonsterDie 떄마다 실행
    private void ProgressQuest(Monster monster)
    {
        if (QuestManager.Instance.questProgressType != QuestManager.QuestProgressType.PROGRESS)
            return;

        // 퀘스트 성공 조건
        if (currHuntCount != questCount)
        {
            currHuntCount++;
        }
        if (currHuntCount == questCount)
        {
            QuestManager.Instance.questProgressType = QuestManager.QuestProgressType.SUCCESS;
        }
    }

    public override void CompleteQuest()
    {
        QuestManager.Instance.questProgressType = QuestManager.QuestProgressType.COMPLETE;

        Player.Instance.CompleteQuest(QuestManager.Instance.GetCurrQuestData());
        RemoveQuest();
        QuestManager.Instance.CheckQuestComplete(NPCManager.Instance.selectedNpc.npcID);
    }
}
