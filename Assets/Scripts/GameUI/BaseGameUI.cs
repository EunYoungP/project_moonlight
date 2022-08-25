using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGameUI : MonoBehaviour
{
    public virtual void Open()
    {
        SoundManager.Instance.Play("UI/Coarse Click_Minimal UI Sounds");
    }

    public virtual void Open(QuestData currQuestData) { }

    public virtual void Open(string setStr) { }

    public virtual void Close()
    {
        SoundManager.Instance.Play("UI/Coarse Click_Minimal UI Sounds");
    }

    public virtual void Init()
    {
    }

    public virtual void LevelUpUpdate()
    {
    }

    public virtual bool AddItem(ItemObject item) { return false; }

    public virtual void SetNoticeText(string noticeText) { }

    public virtual bool GetUIActiveState() { return false; }
}
