using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest_Talk : Quest
{
    public override void AddQuest()
    {
        Player.Instance.AddQuest(QuestManager.Instance.GetCurrQuestData());
        Player.Instance.TalkNpcEvent += ProgressQuest;
        QuestManager.Instance.questProgressType = QuestManager.QuestProgressType.PROGRESS;
    }

    public override void RemoveQuest()
    {
        QuestManager.Instance.questProgressType = QuestManager.QuestProgressType.BEFORE;
        Player.Instance.TalkNpcEvent -= ProgressQuest;
        Player.Instance.RemoveQuest(QuestManager.Instance.GetCurrQuestData());
    }

    // TalkNPC 와 같이 실행
    private void ProgressQuest(NPC npc)
    {
        if (QuestManager.Instance.questProgressType != QuestManager.QuestProgressType.PROGRESS)
            return;

        // 퀘스트 성공 판정
        int questNpcId = QuestManager.Instance.questNpcID;
        if(npc.npcID == questNpcId)
        {
            QuestManager.Instance.questProgressType = QuestManager.QuestProgressType.SUCCESS;
        }
    }

    public override void CompleteQuest()
    {
        QuestManager.Instance.questProgressType = QuestManager.QuestProgressType.COMPLETE;
        NPCManager.Instance.selectedNpc.CompleteQuestJump();
        Player.Instance.CompleteQuest(QuestManager.Instance.GetCurrQuestData());
        RemoveQuest();
        QuestManager.Instance.CheckQuest(NPCManager.Instance.selectedNpc.npcID);
    }
}
