using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UIQuestPopup : BaseGameUI
{
    public TextMeshProUGUI detailText;
    public TextMeshProUGUI titleText;
    public List<Image> rewardImages;

    public Sprite rewardMoneyImg;
    public Sprite rewardExpImg;

    private bool isActive = false;

    public override void Open(QuestData currQuestData)
    {
        isActive = true;
        SoundManager.Instance.Play("Quest/Jingle_Achievement_00");
        gameObject.SetActive(isActive);
        SetQuestRewardUI();
        detailText.text = "'" + currQuestData.questName + "'" + "완료";
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
        gameObject.SetActive(isActive);
    }

    private void Update()
    {
        if (!isActive)
            return;
        
        // 마우스 입력을 시작한 상태 
        // 입력된 터치가 UI오브젝트 위에 있는 상태
        if (Input.GetMouseButtonDown(0)
            && EventSystem.current.IsPointerOverGameObject(Player.Instance.playerController.pointerID))
            Close();
    }
}
