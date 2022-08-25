using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkData
{
    public int npcID;
    public string[] sentences;

    public TalkData(int npcID, string[] sentences)
    {
        this.npcID = npcID;
        this.sentences = sentences;
    }
}
