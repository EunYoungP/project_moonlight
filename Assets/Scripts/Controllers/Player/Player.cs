using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class Player : MonoBehaviour
{
    // [PlayerData]
    public string keyCode;
    public Character data;
    public PlayerController playerController;
    public PlayerInput playerInput { get; set; }
    public Transform startPos;
    public GameObject levelUpVFX;
    public Rigidbody rigid;
    
    // [HpBar]
    private HpBar hpBar;
    private Vector3 hpBarOffset = new Vector3(0, 2.2f, 0);
    private Canvas uiCanvas;
    private Slider hpBarSlider;

    // [Talk]
    public TalkManager talkMng;
    public bool isTalking { get; set; }
    private int talkIndex = 0;
    public GameObject inputControlPanel;
    public PlayerCamera playerCamera;

    // [Quest]
    public QuestManager questMng;
    public List<QuestData> currQuestList = new List<QuestData>();

    public Action<QuestData> AddQuestEvent;
    public Action<QuestData> RemoveQuestEvent;
    public Action<QuestData> CompleteQuestEvent;

    public Action<Monster> MonsterHuntEvent;
    public Action<NPC> TalkNpcEvent;
    public Action<ItemObject> GetItemEvent;

    public Action<int, NPC> CloseQuestUIEvent;
    public Action<QuestData> QuestNotificationEvent;
    public Action<string> GetItemWhenEndTalk;

    private static Player instance;
    public static Player Instance
    {
        get
        { 
            if (instance == null)
            {
                GameObject obj = Instantiate(ResourceManager.Instance.PLAYER);
                DontDestroyOnLoad(obj);
                instance = obj.GetComponent<Player>();
                //instance.Init();
            }
            return instance;
        }
    }

    public void Init()
    {
        if (data != null)
            return;

        SetPlayerStartPos(startPos);

        keyCode = "Player1";
        data = SLManager.Instance.characterDataDic[keyCode];
        playerController = gameObject.GetComponent<PlayerController>();
        playerInput = gameObject.GetComponent<PlayerInput>();
        playerController.Init();
        playerInput.Init();
        SetHpBar();
        talkMng = TalkManager.Instance;
        playerCamera = Camera.main.gameObject.GetComponent<PlayerCamera>();
        questMng = QuestManager.Instance;
        AddCompleteQuestEvent();
        AddSkillColliders();
    }

    // 각 quest script 에서 실행
    public void AddQuest(QuestData currQuest)
    {
        currQuestList.Add(currQuest);

        if (AddQuestEvent != null)
            AddQuestEvent(currQuest);

        // npc quest 추가 지점
        NPCManager.Instance.SetQuestWhenPlayerGetQuest();
    }

    public void RemoveQuest(QuestData currQuest)
    {
        currQuestList.Remove(currQuest);

        if (RemoveQuestEvent != null)
            RemoveQuestEvent(currQuest);
    }

    public void CompleteQuest(QuestData currQuest)
    {
        if (CompleteQuestEvent != null)
            CompleteQuestEvent(currQuest);
    }

    public void AddCompleteQuestEvent()
    {
        CompleteQuestEvent += GetExp;
        CompleteQuestEvent += QuestManager.Instance.CompleteQuestUI;
    }

    private void GetExp(QuestData currQuest)
    {
    }

    public void GetPlayerCamera()
    {
        playerCamera = Camera.main.gameObject.GetComponent<PlayerCamera>();
    }

    public void SetPlayerStartPos(Transform playerPos)
    {
        gameObject.transform.position = playerPos.position;
    }
    
    // Animation 파일에 event 추가
    public void PlayEffect(int effectNum)
    {
        EffectManager.Instance.PlayerSkillEffect(effectNum, EffectObject.Player);
    }

    public void SetTarget(Monster monster)
    {
        if (monster == null)
            playerController.targetPos = null;
        else
            playerController.targetPos = monster.transform;
    }

    // Animation 파일에 event 추가
    public void SetAttack()
    {
        if (!playerController.isSkillState)
        {
            OnAttack();
            return;
        }
        
        // 콜라이더 트리거 체크
        Skill currSkill = SkillManager.Instance.currSelectedSkill;
        SkillManager.skillColliderManager.CheckSkillAttack(currSkill);
    }

    public void OnAttack()
    {
        // Area 스킬일때는 소리가 안나게 수정해야합니다.
        SoundManager.Instance.Play("SkillSFX/NoWeapon/AWP_Impact_Smack_11");

        if (playerController.targetPos != null)
        {
            Monster _target = playerController.targetPos.GetComponent<Monster>();

            if (_target == null)
                return;

            _target.baseController.SetTarget(gameObject.transform);
            _target.SetDamage();
            _target.baseController.ChangeState(MonsterState.GetDamage);
            Debug.Log(_target.name + "'s HP : " + _target.data.curHp);
        }
    }

    public void SetDamage()
    {
        ShakeCamera.Instance.OnShakeCamera(0.1f, 0.1f);

        if (data.isDead == false)
        {
            // 내가 맞기전까지 누구한테 맞는지 모르기때문에 SetDamgae는 SetAttack 에서 불러줘야함
            Monster target = playerController.targetPos.GetComponent<Monster>();
            bool isSkip = UnityEngine.Random.Range(0, 100) >= target.data.skipDamagedMove ? false : true;

            if(!isSkip)
            {
                int monAttackPower = target.data.ATTACKPOWER;
                // 크리티컬 공격 확률 설정
                bool isCritical = UnityEngine.Random.Range(0, 100) >= target.data.ciriticalAttackPercent ? false : true;
                if (isCritical == true)
                {
                    monAttackPower *= target.data.criticalAttackPower;
                }
                data.curHp -= monAttackPower;
            }
        }
        return;
    }

    public void ChangeAnimator()
    {
        playerController.ChangeState(PlayerState.ChangeWeapon);
    }

    public void SetHpBar()
    {
        hpBar = HpBarParent.Instance.CreateHpBar(this.gameObject, hpBarOffset);

        //GameObject canvas = GameObject.Find("UICanvas");
        //if(canvas != null )
        //{
        //    uiCanvas = canvas.GetComponent<Canvas>();
        //    GameObject hpBarPrefab = Instantiate<GameObject>(ResourceManager.Instance.HPBAR, uiCanvas.transform);
        //    hpBarSlider = hpBarPrefab.GetComponentInChildren<Slider>();
        //    hpBar = hpBarPrefab.GetComponent<HpBar>();
        //    hpBar.targetTr = this.gameObject.transform;
        //    hpBar.offset = hpBarOffset;
        //}
    }

    private void AddSkillColliders()
    {
        SkillCollider[] skillColliderArr = GetComponentsInChildren<SkillCollider>();

        for (int i = 0; i < skillColliderArr.Length; i++)
        {
            SkillManager.Instance.skillColliders.Add(skillColliderArr[i]);
            skillColliderArr[i].gameObject.SetActive(false);
        }
    }

    #region 대화진행
    public void TalkNpc(int npcId, NPC npc)
    {
        if (QuestDialogActive())
            return;

        // 카메라 줌인
        if (isTalking == false)
        {
            StartTalk();
            playerCamera.ZoomIn(npc.gameObject);
        }

        // 퀘스트 데이터 받기
        int questTalkIndex = questMng.GetQuestTalkIndex();
        // 대화 데이터 받기
        string talkData = talkMng.GetTalk(npcId + questTalkIndex, talkIndex);

        // 대화 종료 검사
        if (talkData == null)
        {
            // QuestPopUp창이 Close 될때 
            // 실행된 TalkNPC 에서 받은 Quest가 없어서
            // talkData 가 없고, Quest 상태가 아닌데도 실행됨
            EndTalk(npc);
            return;
        }

        // 대화 진행
        npc.OnChatBox(talkData);
        npc.Rotate(gameObject.transform.position);

        talkIndex++;
    }

    private bool QuestDialogActive()
    {
        bool isQuestDialogActive = UIGameMng.Instance.uiGameDic[UIGameType.QuestDialog].GetUIActiveState();

        if (isQuestDialogActive)
        {
           //CloseQuestUIEvent += TalkNpc;
        }
        else if (!isQuestDialogActive)
        {
        }
        return isQuestDialogActive;
    }
    
    private void StartTalk()
    {
        isTalking = true;
        inputControlPanel.SetActive(isTalking);
        //playerController.ChangeState(PlayerState.Talk);
    }

    private void EndTalk(NPC npc)
    {
        isTalking = false;
        inputControlPanel.SetActive(isTalking);
        talkIndex = 0;
        npc.OffChatBox();
        playerCamera.ZoomOut();
        playerController.ChangeState(PlayerState.Idle);

        if (GetItemWhenEndTalk != null)
        {
            QuestData _prevQuestData = QuestManager.Instance.GetPrevQuestData();
            QuestNotificationEvent(_prevQuestData);

            // 아이템지급
            GetItemWhenEndTalk(QuestManager.Instance.questItemName);
            QuestManager.Instance.RemoveGetItemQuest();
            return;
        }
        QuestData prevQuestData = QuestManager.Instance.GetPrevQuestData();
        QuestNotificationEvent(prevQuestData);
    }
    #endregion

    private void LateUpdate()
    {
        if( hpBar != null )
            hpBar.UpdateHpBar(data.maxHp, data.curHp);
    }
}
