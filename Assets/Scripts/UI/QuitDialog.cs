using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitDialog : MonoBehaviour
{
    public Button YBtn;
    public Button NBtn;

    public void Init()
    {
        AddBtnListener();
    }

    private void AddBtnListener()
    {
        YBtn.onClick.AddListener(() => QuitGame());
        NBtn.onClick.AddListener(() =>gameObject.SetActive(false));
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
