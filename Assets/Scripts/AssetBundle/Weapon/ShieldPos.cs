using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPos : MonoBehaviour
{
    GameObject[] shields;

    private static ShieldPos instance;
    public static ShieldPos Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ShieldPos>();
            }

            instance.Init();
            return instance;
        }
    }

    public void Init()
    {
        shields = GetComponentsInChildren<GameObject>(true);
    }
}
