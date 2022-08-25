using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceControl : MonoBehaviour
{
    GameObject m_player;
    List<Mob> m_mobs = new List<Mob>();
    // Start is called before the first frame update
    void Start()
    {
        m_player = GameObject.FindGameObjectWithTag("Player");
        m_mobs.AddRange( GameObject.FindObjectsOfType<Mob>() );

    }

    // Update is called once per frame
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
