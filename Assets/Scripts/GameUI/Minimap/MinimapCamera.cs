using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    private Transform player;
    private float distanceY = 20;
    private Vector3 prevCameraPos;

    private void MinimapCameraInit()
    {
        if (Player.Instance != null)
            player = Player.Instance.transform;

        transform.position = player.position + new Vector3(0,distanceY,0);
    }

    private void TracePlayer()
    {
        if (Player.Instance == null)
            return;

        Transform playerCurPos = Player.Instance.transform;

        Vector3 newPosition = playerCurPos.position;
        newPosition += new Vector3(0, distanceY, 0);
        transform.position = newPosition;

        //transform.rotation = Quaternion.Euler(90f, playerCurPos.eulerAngles.y, 0);
    }

    private void LateUpdate()
    {
        TracePlayer();
    }
}
