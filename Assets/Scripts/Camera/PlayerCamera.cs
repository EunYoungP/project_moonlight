using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private Vector3 playerBeforePos;

    private float distanceX = 0f;
    private float distanceY = 11f;
    private float distanceZ = -6f;

    // 대화 시스템
    private GameObject target;

    // 줌 효과
    private bool isMoving;
    private Transform beforeZoomTr;
    private float elapsedTime;
    private float zoomTime = 1f;
    private float distance;

    public void CameraInit()
    {
        if (Player.Instance != null)
            playerBeforePos = Player.Instance.transform.position;

        // 처음 카메라 위치 지정
        transform.position = playerBeforePos + new Vector3(distanceX, distanceY, distanceZ);
    }

    private void FollowPlayer()
    {
        if (Player.Instance == null)
            return;

        Vector3 playerCurPos = Player.Instance.transform.position;

        Vector3 tmp = transform.position;
        tmp += (playerCurPos - playerBeforePos);
        transform.position = tmp;

        playerBeforePos = playerCurPos;
    }

    public void ZoomIn( GameObject getTarget)
    {
        beforeZoomTr = transform;
        target = getTarget;
        isMoving = true;

        StartCoroutine(ZoomInAction(target));
    }

    private IEnumerator ZoomInAction(GameObject target)
    {
        if (target == null)
            yield return null;

        elapsedTime = 0;
        while( Camera.main.fieldOfView != 40)
        {
            elapsedTime += Time.deltaTime;
            float FOVvalue = Mathf.Lerp(Camera.main.fieldOfView,40, zoomTime/elapsedTime);
            Camera.main.fieldOfView = FOVvalue;
            Camera.main.fieldOfView = Mathf.Clamp(FOVvalue, 40, 80);
            yield return null;
        }
        isMoving = false;
        yield return null;
    }

    public void ZoomOut()
    {
        target = null;
        isMoving = true;

        StartCoroutine(ZoomOutAction());
    }

    private IEnumerator ZoomOutAction()
    {
        elapsedTime = 0;
        while( Camera.main.fieldOfView != 60)
        {
            elapsedTime += Time.deltaTime;
            float FOVvalue = Mathf.Lerp(Camera.main.fieldOfView, 60, zoomTime/elapsedTime);
            Camera.main.fieldOfView = FOVvalue;
            Camera.main.fieldOfView = Mathf.Clamp(FOVvalue, 40, 80);
            yield return null;
        }
        isMoving = false;
        yield return null;
    }

    private void LateUpdate()
    {
        if(isMoving)
            return;

        FollowPlayer();
    }
}
