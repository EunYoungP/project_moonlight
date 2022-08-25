//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Events;

//public class CharacterManager : MonoBehaviour
//{
//    private CharacterManager instance;
//    public CharacterManager Instance
//    {
//        get
//        {
//            if(instance == null)
//            {
//                GameObject obj = new GameObject("CharacterManager", typeof(CharacterManager));
//                DontDestroyOnLoad(obj);

//                instance = obj.GetComponent<CharacterManager>();
//            }
//            return instance;
//        }
//    }

//    public string name { get; set; }
//    public int level { get; set; }
//    public int maxHp { get; set; }
//    public int curHp { get; set; }
//    public int minAttack { get; set; }
//    public int maxAttack { get; set; }
//    public int defense { get; set; }
//    public int ciriticalAttackPercent { get; set; }
//    public int criticalAttackPower { get; set; }
//    public int skipDamagedMove { get; set; }
//    public bool isDead { get; set; }
//    public float attackDelay { get; set; }
//    public float attackTimer { get; set; }

//    public int ATTACKPOWER
//    { get { return Random.Range(minAttack, maxAttack + 1); } }

//    public virtual void InitData()
//    {
//    }

//    public void DecreseHp(int enemyAttack)
//    {
//        curHp -= enemyAttack;

//        if (curHp < 0)
//            isDead = true;
//    }
//}
