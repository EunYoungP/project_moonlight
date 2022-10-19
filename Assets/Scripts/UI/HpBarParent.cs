using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBarParent : MonoBehaviour
{
    private Canvas UICanvas;

    private static HpBarParent instnace;
    public static HpBarParent Instance
    {
        get
        {
            if(instnace == null)
            {
                GameObject obj = GameObject.Find("HPBarParent");
                DontDestroyOnLoad(obj);

                instnace = obj.GetComponent<HpBarParent>();
            }
            return instnace;
        }
    }

    public void Init()
    {
        UICanvas = GetComponentInChildren<Canvas>();
    }

    public HpBar CreateHpBar(GameObject _target, Vector3 offset)
    {
        if (UICanvas != null)
        {
            GameObject hpBarPrefab = Instantiate(ResourceManager.Instance.HPBAR, UICanvas.transform);
            HpBar hpBar = hpBarPrefab.GetComponent<HpBar>();
            hpBar.target = _target;
            hpBar.offset = offset;
            return hpBar;
        }
        return null;
    }
}
