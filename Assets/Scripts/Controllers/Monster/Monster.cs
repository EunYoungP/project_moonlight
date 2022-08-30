using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MonsterType
{
    Golem,
}

public class Monster : MonoBehaviour
{
    public string keyCode;
    public Character data;
    
    public Animator m_animator;
    public float viewDist = 6f;

    // [HpBar]
    private HpBar hpBar;
    private Vector3 hpBarOffset = new Vector3(0, 2.2f, 0);
    private Canvas uiCanvas;
    private Slider hpBarSlider;

    public MonsterType monsterType;
    public BaseController baseController;

    // [FloatingDamageText]
    public Transform floatingTextPos;
    private Vector3 floatingTextOffset = new Vector3(0, 1.0f, 0);

    // [Map]
    private MAP mapType;

    void Start()
    {
        keyCode = gameObject.name;
        data = SLManager.Instance.LoadGolemData(mapType, keyCode);
        monsterType = MonsterType.Golem;
        m_animator = GetComponent<Animator>();
        baseController = GetComponent<BaseController>();

        CreateMonsterHpBar();
    }

    // Attack Animation 의 Event 로 추가된 함수
    // Monster의 공격이 Player에게 닿을 때 실행됨
    // 여기서 Player.Instance로 처리하지 않고 sendMessage로 타겟정보만 보내줌
    public void SetAttack()
    {
        // 오류나는지 확인 해야함 null체크
        if(baseController.GetTarget() !=null)
        {
            if (baseController.GetTarget() == null)
                return;

            Player.Instance.playerController.targetPos = gameObject.transform;
            Player.Instance.SetDamage();
            Player.Instance.playerController.ChangeState(PlayerState.GetDamage);
            Debug.Log(Player.Instance.data.name + "'s HP : " + Player.Instance.data.curHp);

            if (Player.Instance.data.curHp <= 0)
                baseController.SetTarget(null);
        }
    }

    public void SetDamage()
    {
        if (data.isDead == false)
        {
            bool isSkip = Random.Range(0, 100) >= Player.Instance.data.skipDamagedMove ? false : true;

            if (isSkip == false)
            {
                int attackPower = Player.Instance.data.ATTACKPOWER;
                // 크리티컬 공격 확률 설정
                bool isCritical = Random.Range(0, 100) >= Player.Instance.data.ciriticalAttackPercent ? false : true;
                if (isCritical == true)
                {
                    attackPower *= Player.Instance.data.criticalAttackPower;
                }
                data.curHp -= attackPower;
                CreateDamageText(attackPower);
            }
        }
        return;
    }

    public void CreateDamageText(int text)
    {
        GameObject FloatingTextObj = Instantiate(ResourceManager.Instance.FLOATINGTEXT,floatingTextPos );

        FloatingTextController floatingTextController = FloatingTextObj.GetComponent<FloatingTextController>();
        //FloatingText floatingText = FloatingTextObj.GetComponentInChildren<FloatingText>();

        floatingTextController.Init();
        floatingTextController.target = this.gameObject;
        floatingTextController.SetText(text);
    }

    public void CreateMonsterHpBar()
    {
        //// UI게임 오브젝트 받아오기
        //uiCanvas = GameObject.Find("UICanvas").GetComponent<Canvas>();
        //// 프리팹으로 만든hpbar 생성해서 담기
        //GameObject hpBarPrefab = Instantiate<GameObject>(ResourceManager.Instance.HPBAR, uiCanvas.transform);
        //// 받아온 프리팹 객체에서 슬라이더 받기
        ////hpBarSlider = hpBarPrefab.GetComponentInChildren<Slider>();
        //// 받아온 프리팹 객체에서 HpBar 스크립트 받기
        //hpBar = hpBarPrefab.GetComponent<HpBar>();
        //// 프리팹객체를 생성할 위치를 스크립트붙은 몬스터 위치로 설정
        //hpBar.targetTr = this.gameObject.transform;
        //// 몬스터객체로부터 얼마나 위에 배치될지 설정
        //hpBar.offset = hpBarOffset;

        hpBar =  HpBarParent.Instance.CreateHpBar(this.gameObject, hpBarOffset);
    }

    public void AfterDead(Monster monster)
    {
        hpBar.gameObject.SetActive(false);

        //몬스터 일정시간 반투명

        StartCoroutine(DestroyMonster(monster));
        ItemDrop();
    }

    IEnumerator DestroyMonster(Monster monster)
    {
        yield return new WaitForSeconds(2f);
        monster.gameObject.SetActive(false);
    }

    // 죽은 타겟 위치의 근방에 아이템 드랍
    public void ItemDrop()
    {
        Vector3 dropPos = DropItem.Instance.DropPos(this.gameObject);
        Item itemData = DropItem.Instance.rndItem(ItemType.Ingredient, "예리한 발톱");
        DropItem.Instance.ItemDrop(itemData, dropPos);
    }

    public MAP MapType
    {
        get { return this.mapType; }
        set { this.mapType = value; }
    }

    void Update()
    {
        hpBar.UpdateHpBar(data.maxHp, data.curHp);
    }
}
