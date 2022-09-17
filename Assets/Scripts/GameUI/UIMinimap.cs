using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIMinimap : BaseGameUI
{
    public TextMeshProUGUI curMapName;

    public override void Open()
    {
        base.Open();
        gameObject.SetActive(true);
    }

    public override void Close()
    {
        base.Close();
        gameObject.SetActive(false);
    }

    public void ChangeMapName(Map currmap)
    {
        curMapName.text = currmap.mapName;
    }
}
