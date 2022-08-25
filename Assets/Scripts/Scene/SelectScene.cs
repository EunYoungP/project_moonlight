using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectScene : BaseScene
{
    private void Start()
    {
        UIMng.Instance.OpenUI<Fade>(UIType.Fade).Close();
    }

    public override void Exit()
    {
    }
}
