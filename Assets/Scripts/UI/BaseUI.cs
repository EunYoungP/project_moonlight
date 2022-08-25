using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// UI화면들의 관리담당
// BaseUI로 묶어서 관리가능
public class BaseUI : MonoBehaviour
{
    public void Init()
    {
        UIInit();
    }

    public virtual void UIInit()
    {
    }

    public virtual void UIUpdate(float progress)
    {
    }

    public virtual void Open()
    {
    }

    public virtual void Close()
    {
    }
}
