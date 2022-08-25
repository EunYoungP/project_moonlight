using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleLoading : MonoBehaviour
{
    public List<LayoutElement> elements = new List<LayoutElement>();

    private int index = 0;
    private float targetWidth;
    private float targetMaxWidth = 250;

    private void LayoutEffect()
    {
        StartZoomLayout(1.5f);
        KeepZoomState(3f);
        EndZoomLayout(1f);

        CheckIndex();
    }

    private void StartZoomLayout(float zoomInTime)
    {
        targetWidth = elements[index].minWidth;

        while(targetWidth != targetMaxWidth)
        {
            float newWidth = Mathf.Lerp(targetWidth, targetMaxWidth, zoomInTime);
            targetWidth = newWidth;
        }
    }

    private void KeepZoomState(float keepTime)
    {
        float startTime = Time.time;

        while(Time.time - startTime <= keepTime)
            return;
    }

    private void EndZoomLayout(float zoomOutTime)
    {
        targetWidth = elements[index].minWidth;

        while (targetWidth != 0)
        {
            float newWidth = Mathf.Lerp(targetWidth, 0, zoomOutTime);
            targetWidth = newWidth;
        }
    }

    private void CheckIndex()
    {
        index++;
        if (index / elements.Count > 0)
            index = 0;
    }

    private void Update()
    {
        LayoutEffect();
    }
}
