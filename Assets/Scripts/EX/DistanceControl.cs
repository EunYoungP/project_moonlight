using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceControl : MonoBehaviour
{
    private GameObject m_player;
    private List<Mob> m_mobs = new List<Mob>();

    void Start()
    {
        m_player = GameObject.FindGameObjectWithTag("Player");
        m_mobs.AddRange( GameObject.FindObjectsOfType<Mob>() );
    }

    void Update()
    {
        foreach (Mob mob in m_mobs)
        {
            float distance = Vector3.Distance(mob.transform.position, 
                                                m_player.transform.position);

            if (distance > mob.m_viewDist)
                mob.gameObject.SetActive(false);
            else
                mob.gameObject.SetActive(true);
        }
    }
}
