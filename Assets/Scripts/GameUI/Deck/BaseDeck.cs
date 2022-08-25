using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseDeck : MonoBehaviour
{
    public virtual void Init() { }

    public virtual void OnClickSlot(Skill skill) { }
}