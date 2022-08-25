using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonData : MonoBehaviour
{
    private string dungeonName;
    private int dungeonEnterLevel;
    private string dungeonDesc;
    private string dungeonImg;
    private string needScroll;

    public string DungeonName { get { return dungeonName; } }
    public int DungeonEnterLevel { get { return dungeonEnterLevel; } }
    public string DungeonDesc { get { return dungeonDesc; } }
    public string DungeonImg { get { return dungeonImg; } }
    public string NeedScroll { get { return needScroll; } }

    public DungeonData(string dungeonName,
                        int dungeonEnterLevel,
                        string dungeonDesc,
                        string dungeonImg,
                        string needScroll)
    {
        this.dungeonName = dungeonName;
        this.dungeonEnterLevel = dungeonEnterLevel;
        this.dungeonDesc = dungeonDesc;
        this.dungeonImg = dungeonImg;
        this.needScroll = needScroll;
    }
}
