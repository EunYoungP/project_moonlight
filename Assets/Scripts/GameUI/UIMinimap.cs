using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMinimap : BaseGameUI
{
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

    public override void Init()
    {
    }

    public override void LevelUpUpdate()
    {
    }
}
