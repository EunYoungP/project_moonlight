using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Quest : MonoBehaviour
{
    public abstract void AddQuest();

    public abstract void RemoveQuest();

    public abstract void CompleteQuest();
}
