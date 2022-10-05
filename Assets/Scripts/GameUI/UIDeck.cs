using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DeckType
{
    PotionDeck,
    ItemDeck,
    SkillDeck,
}

public class UIDeck : BaseGameUI
{
    public static Dictionary<DeckType, BaseDeck> deckDic = new Dictionary<DeckType, BaseDeck>();

    public override void Init()
    {
        AddScript<PotionDeck>(DeckType.PotionDeck);
        AddScript<ItemDeck>(DeckType.ItemDeck);
        AddScript<SkillDeck>(DeckType.SkillDeck);
    }

    public void AddScript<T>(DeckType deckType)where T : BaseDeck
    {
        if(!deckDic.ContainsKey(deckType))
        {
            T t = GetComponentInChildren<T>();
            deckDic.Add(deckType, t);
            deckDic[deckType].Init();
        }
        return;
    }

    public T GetScript<T>(DeckType deckType)where T:BaseDeck
    {
        if (!deckDic.ContainsKey(deckType))
            AddScript<T>(deckType);

        T t = deckDic[deckType].GetComponent<T>();
        return t;
    }

    public override void Open()
    {
        gameObject.SetActive(true);
        DownLoadAssetBundle.Instance.SetSkillSlotSprite();
    }

    public override void Close()
    {
        gameObject.SetActive(false);
    }
}
