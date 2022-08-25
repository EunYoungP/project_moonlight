using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistController : MonoBehaviour
{
    Monster monster;
    List<Monster> mobs = new List<Monster>();

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        
    }
}
