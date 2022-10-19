using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    private Transform player;
    private float distanceY = 20;
    private Vector3 newPosition;
    private Transform playerCurPos;

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

        playerCurPos = Player.Instance.transform;

        newPosition = playerCurPos.position;
        newPosition += new Vector3(0, distanceY, 0);
        transform.position = newPosition;
    }

    private void LateUpdate()
    {
        TracePlayer();
    }
}
