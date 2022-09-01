using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest_GetItem : Quest
{
    public override void AddQuest()
    {
        Player.Instance.GetItemEvent += ProgressQuest;
        Player.Instance.AddQuest(QuestManager.Instance.GetCurrQuestData());
        QuestManager.Instance.questProgressType = QuestManager.QuestProgressType.PROGRESS;
    }

    public override void RemoveQuest()
    {
        QuestManager.Instance.questProgressType = QuestManager.QuestProgressType.BEFORE;
        Player.Instance.GetItemEvent -= ProgressQuest;
        Player.Instance.RemoveQuest(QuestManager.Instance.GetCurrQuestData());
    }

    // TalkNPC 와 같이 실행
    // 아이템수령 시점은?
    // 대화가 끝나면 아이템수령?
    private void ProgressQuest(ItemObject getItem)
    {
        if (QuestManager.Instance.questProgressType != QuestManager.QuestProgressType.PROGRESS)
            return;

        // 퀘스트 성공 판정
        string QuestItem = QuestManager.Instance.questItemName;
        if(getItem.Name == QuestItem)
            QuestManager.Instance.questProgressType = QuestManager.QuestProgressType.SUCCESS;
    }

    public override void CompleteQuest()
    {
        QuestManager.Instance.questProgressType = QuestManager.QuestProgressType.COMPLETE;

        Player.Instance.CompleteQuest(QuestManager.Instance.GetCurrQuestData());
        RemoveQuest();
        QuestManager.Instance.CheckQuestComplete(NPCManager.Instance.selectedNpc.npcID);
    }
}
