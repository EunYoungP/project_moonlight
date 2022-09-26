using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{
    private GameObject playerR;
    private GameObject golemR;

    private List<GameObject> ItemR = new List<GameObject>();
    private List<Sprite> ItemIconR = new List<Sprite>();

    private GameObject skillPanelR;
    private GameObject skillPopUpR;
    private List<Skill> skillList = new List<Skill>();

    private GameObject floatingText;
    private GameObject chatBox;
    private GameObject notification;
    private GameObject hpBarR;
    private GameObject loadUIR;
    private GameObject gameUIR;
    private GameObject ItemDropVFXR;
    private Image[] dungeonImgs;
    private GameObject pitcupItemUI;

    // 프로퍼티
    public GameObject PLAYER { get { return playerR; } }
    public GameObject GOLEMR { get { return golemR; } }
    public List<GameObject> ITEM { get { return ItemR; } }
    public List<Skill> SkillLIST { get { return skillList; } }

    public GameObject HPBAR { get { return hpBarR; } }
    public GameObject UI { get { return loadUIR; } }
    public List<Sprite> ITEMICON { get { return ItemIconR; } }
    public GameObject SKILLPANEL { get { return skillPanelR; } }
    public GameObject SKILLPOPUP { get { return skillPopUpR; } }
    public GameObject FLOATINGTEXT { get { return floatingText; } }
    public GameObject CHATBOX { get { return chatBox; } }
    public GameObject GAMEUI { get { return gameUIR; } }
    public GameObject ITEMDROP_VFX { get { return ItemDropVFXR; } }
    public Image[] DUNGEON_IMG { get { return dungeonImgs; } }
    public GameObject PICKUPITEM_UI { get { return pitcupItemUI; } }


    private static ResourceManager instance;
    public static ResourceManager Instance
    {
        get
        {
            if(instance == null)
            {
                GameObject obj = new GameObject("Resource Manager", typeof(ResourceManager));
                DontDestroyOnLoad(obj);

                instance = obj.GetComponent<ResourceManager>();
                // new GameObject를 하는 순간에 Awake()함수가 호출되지는 않습니다.
                // 따라서 매니저의 초기화 함수는 직접 호출해주는 것이 좋습니다.
                instance.Init();
            }
            return instance;
        }
    }

    private void Init()
    {
        LoadPlayer();
        LoadGolem();
        LoadHpBar();
        //LoadItem();
        //LoadItemIcon();
        LoadSkillPanel();
        LoadSkillPopUp();
        LoadSkillList();
        LoadFloatingText();
        LoadChatBox();
        LoadGameUI();
        LoadItemDropVFX();
        LoadDungeonImg();
        LoadPickUpItemUI();
    }

    public void LoadPickUpItemUI()
    {
        pitcupItemUI = Resources.Load<GameObject>("Prefabs/UI/PickUpItemUI");
    }

    public void LoadDungeonImg()
    {
        dungeonImgs = Resources.LoadAll<Image>("DungeonUI/");
    }

    public void LoadItemDropVFX()
    {
        ItemDropVFXR = Resources.Load<GameObject>("ItemEffect/ItemDropAura");
    }

    public void LoadGameUI()
    {
        gameUIR = Resources.Load<GameObject>("Prefabs/UI/UIGame");
    }

    public void LoadChatBox()
    {
        chatBox = Resources.Load<GameObject>("Prefabs/UI/ChatBox");
    }

    public void LoadFloatingText()
    {
        floatingText = Resources.Load<GameObject>("Prefabs/UI/FloatingDamageText");
    }

    public void LoadSkillList()
    {
        Skill[] skills = Resources.LoadAll<Skill>("Skills/");
        foreach(Skill skill in skills)
        {
            skillList.Add(skill);
        }
    }

    public void LoadSkillPopUp()
    {
        skillPopUpR = Resources.Load<GameObject>("Prefabs/Skill/SkillPopUp");
    }

    public void LoadSkillPanel()
    {
        skillPanelR = Resources.Load<GameObject>("Prefabs/Skill/SkillPanel");
    }

    public void LoadItemIcon()
    {
        foreach (Item item in ItemDB.Instance.itemDB)
        {
            string path = "Prefabs/ItemIcon/" + (item.iconName).ToString();
            Sprite sprite = Resources.Load<Sprite>(path);
            // 스프라이트를 로드할 때 아이템 db의 sprite를 초기화
            item.icon = sprite;
            ItemIconR.Add(sprite);
        }
    }

    // 아이템 프리팹에있는 아이템 전부 로드해놓음
    public void LoadItem()
    {
        foreach(Item item in ItemDB.Instance.itemDB)
        {
            ItemR.Add( Resources.Load<GameObject>("Prefabs/Item/" + (item.name).ToString()));
        }
    }

    public void LoadUI<T>() where T : BaseUI
    {
        //if(loadUIR == null)
            loadUIR = Resources.Load<GameObject>("Prefabs/UI/" + typeof(T).ToString());
    }

    public void LoadGolem()
    {
        if (golemR == null)
            golemR = Resources.Load<GameObject>("Prefabs/Character/Golem");
    }

    public void LoadHpBar()
    {
        if (hpBarR == null)
            hpBarR = Resources.Load<GameObject>("Prefabs/UI/HPBar");
    }

    public void LoadPlayer()
    {
        if(playerR==null)
        {
            playerR = Resources.Load<GameObject>("Prefabs/Character/Player");
        }
    }       
}
