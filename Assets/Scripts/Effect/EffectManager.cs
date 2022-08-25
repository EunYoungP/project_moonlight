using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectObject
{
    Player,
}

public class EffectManager : MonoBehaviour
{
    public Dictionary<EffectObject, EffectBase> effectControllerDic = new Dictionary<EffectObject, EffectBase>();

    private static EffectManager instance;
    public static EffectManager Instance
    {
        get
        {
            if(instance == null)
            {
                GameObject obj = new GameObject("EffectManager", typeof(EffectManager));
                DontDestroyOnLoad(obj);
                instance = obj.GetComponent<EffectManager>();
                instance.Init();
                return instance;
            }
            return instance;
        }
    }

    private void Init()
    {
        AddScript<PlayerEffect>(EffectObject.Player);
    }
    
    public void AddScript<T>(EffectObject effectObj) where T:EffectBase
    {
        if(!effectControllerDic.ContainsKey(effectObj))
        {
            T t = FindObjectOfType<T>();
            if (t == null)
                return;

            t.Init();
            effectControllerDic.Add(effectObj, t);
        }
    }

    public T GetScript<T>(EffectObject effectObject) where T : EffectBase
    {
        if (!effectControllerDic.ContainsKey(effectObject))
        {
            T t = GetComponent<T>();
            t.Init();
            effectControllerDic.Add(effectObject, t);
            return t;
        }
        T tt = effectControllerDic[effectObject].GetComponent<T>();
        return tt;
    }

    public void PlayerSkillEffect(int effectNum, EffectObject effectObj)
    {
        effectControllerDic[effectObj].PlaySkillEffect(effectNum);
    }
}
 