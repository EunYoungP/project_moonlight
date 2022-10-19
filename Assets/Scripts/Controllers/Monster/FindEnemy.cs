using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 카메라의 시야각을 이용하여 player를 감지하는 방식
public class FindEnemy : MonoBehaviour
{
    private Camera m_fieldCamera;
    private Transform m_currTarget;

    private Player player;

    private void Start()
    {
        Init();
    }
    
    public void Init()
    {
        m_fieldCamera = GetComponent<Camera>();
        player = GameObject.FindObjectOfType<Player>();
    }

    private void Update()
    {
        if (player == null)
            return;

        // 카메라의 평면값
        Plane[] planes =  GeometryUtility.CalculateFrustumPlanes(m_fieldCamera);

        if(GeometryUtility.TestPlanesAABB(planes, player.GetComponent<Collider>().bounds))
        {
            if (m_currTarget != null || m_currTarget == player.transform)
                return;

            m_currTarget = player.transform;
            transform.parent.SendMessage("SetTarget", player.transform, SendMessageOptions.DontRequireReceiver);
        }
        else
        {
            m_currTarget = null;
        }
    }
}
