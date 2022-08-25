using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class TitleSceneDialog : MonoBehaviour
{
    public TextMeshProUGUI tmp;

    public void EnterGameState()
    {
        tmp.text = "게임에 연결되는 중입니다.";
    }
}
