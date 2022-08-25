using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMenu : BaseGameUI
{
    private List<BaseGameUI> menuBtns = new List<BaseGameUI>();

    public override void Open()
    {
        gameObject.SetActive(true);
    }

    public override void Close()
    {
        gameObject.SetActive(false);
    }

    public override void Init()
    {

    }
}
