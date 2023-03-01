using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;
using UnityEngine.EventSystems;
using DG.Tweening;

public class UIQuestPopup : BaseGameUI
{
    public TextMeshProUGUI detailText;
    public TextMeshProUGUI titleText;
    public List<Image> rewardImages;

    // DoTween Animation
    [SerializeField] GameObject questPanel;
    [SerializeField] private AnimationCurve animationCurve;
    Sequence showSequence;
    Sequence hideSequence;
    Tweener floatTweener;

    public Sprite rewardMoneyImg;
    public Sprite rewardExpImg;

    private bool isActive = false;
    public static bool IsActive { get; }

    private StringBuilder sb = new StringBuilder(100);

    public override void Init()
    {
        DOAnimationQuestPopup();
    }

    private void DOAnimationQuestPopup()
    {
        floatTweener = questPanel.transform.DOMoveY(10, 1).SetRelative().SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad);

        showSequence = DOTween.Sequence().SetAutoKill(false).Pause()
            .AppendCallback(() => floatTweener.Play())
            .Join(questPanel.transform.DOScale(1,0.5f).SetEase(animationCurve));

        hideSequence = DOTween.Sequence().SetAutoKill(false).Pause()
            .Join(questPanel.transform.DOScale(0, 0.5f).SetEase(Ease.InBack))
            .AppendCallback(() => floatTweener.Pause())
            .OnComplete(() => gameObject.SetActive(isActive));
    }

    // 22.02.02 다시 활성화될 때, 스케일이 1로 변하지 않고 0으로 유지되는 오류 발생
    public override void Open(QuestData currQuestData)
    {
        isActive = true;
        SoundManager.Instance.Play("Quest/Jingle_Achievement_00");
        gameObject.SetActive(isActive);
        SetQuestRewardUI();
        sb.Append("'").Append(currQuestData.questName).Append("'").Append("완료");
        detailText.text = sb.ToString();
        sb.Clear();
        
        hideSequence.Pause();
        showSequence.Restart();
    }

    private void SetQuestRewardUI()
    {
        int rewardIndex = 0;
        QuestData currQuestData = QuestManager.Instance.GetCurrQuestData();

        if (currQuestData.exp > 0)
        {
            rewardImages[rewardIndex].gameObject.SetActive(true);
            rewardImages[rewardIndex].sprite = rewardExpImg;
            rewardIndex++;
        }
        if (currQuestData.money > 0)
        {
            rewardImages[rewardIndex].gameObject.SetActive(true);
            rewardImages[rewardIndex].sprite = rewardMoneyImg;
            rewardIndex++;
        }
        if (currQuestData.itemName!= null)
        {
            int rewardItemCount = 0;
            for(int i = 0; i< currQuestData.itemName.Length; i++)
            {
                rewardImages[rewardIndex].gameObject.SetActive(true);
                foreach(Sprite itemIcon in ResourceManager.Instance.ITEMICON)
                {
                    if(itemIcon.name == currQuestData.itemName[rewardItemCount])
                    {
                        rewardImages[rewardIndex].sprite = itemIcon;
                    }
                }
                rewardIndex++;
            }
        }
    }

    public override bool GetUIActiveState()
    {
        return isActive;
    }

    public override void Close()
    {
        base.Close();
        isActive = false;

        if (Player.Instance.CloseQuestUIEvent != null)
        {
            Player.Instance.CloseQuestUIEvent(NPCManager.Instance.selectedNpc.npcID, NPCManager.Instance.selectedNpc);
            Player.Instance.CloseQuestUIEvent -= Player.Instance.TalkNpc;
        }
        showSequence.Pause();
        hideSequence.Restart();
    }

    private void Update()
    {
        if (!isActive)
            return;
        
        // 입력된 터치가 UI오브젝트 위에 있는 상태
        if (Input.GetMouseButtonDown(0)
            && EventSystem.current.IsPointerOverGameObject(Player.Instance.playerController.pointerID))
            Close();
    }
}
