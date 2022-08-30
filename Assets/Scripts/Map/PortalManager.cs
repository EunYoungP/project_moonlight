using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour
{
    public List<Portal> portalList = new List<Portal>();
    public List<SceneState> doneGetPortalScene = new List<SceneState>();

    private bool usingPortal = false;
    private SceneState portalCurrScene;

    private static PortalManager instance;
    public static PortalManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject("PortalManager", typeof(PortalManager));
                DontDestroyOnLoad(obj);

                instance = obj.GetComponent<PortalManager>();
            }
            return instance;
        }
    }

    // 포탈 정보 받아옴
    public void GetPortalInfo(Portal getPortal)
    {
        portalCurrScene = getPortal.currScene;
        usingPortal = true;
    }

    public void GetPortalList(SceneState getPortalScene)
    {
        if (doneGetPortalScene.Contains(getPortalScene))
            return;

        ResetPortalList();

        Portal[] portalArr = FindObjectsOfType<Portal>();
        for(int i = 0; i < portalArr.Length; i++)
        {
            portalList.Add(portalArr[i]);
        }

        doneGetPortalScene.Add(getPortalScene);
    }

    private void ResetPortalList()
    {
        for(int i = portalList.Count-1; i >= 0; i--)
        {
            portalList.Remove(portalList[i]);
        }
    }

    public void CheckUsePortal()
    {
        if (usingPortal == false)
            return;

        ChangeTargetPos(Player.Instance.gameObject, portalCurrScene);

        Player.Instance.playerController.ChangeState(PlayerState.Idle);
        Player.Instance.playerController.isUsingPortal = false;
        usingPortal = false;
    }

    public void ChangeTargetPos(GameObject target, SceneState currSceneState)
    {
        // 들어간 포탈의 연결된 씬과 같은 인덱스 번호를 가진 포탈
        // 그 포탈의 translatePos 로 타겟 위치 이동
        for(int i = 0; i < portalList.Count; i++)
        {
            if (currSceneState == portalList[i].connectScene)
            {
                //포탈의 게임오브젝트 위치 + traslayerPos의 위치 = Playter의 위치
                Vector3 tmp = new Vector3( portalList[i].translatePos.position.x,
                                          portalList[i].translatePos.position.y,
                                          portalList[i].translatePos.position.z);
                target.transform.position = tmp;
            }
        }
    }
}
