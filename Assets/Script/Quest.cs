using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum questType{
    killMushroom
}


[System.Serializable]
public class Quest
{
    public string questName;
    public string[] questDiscription;
    public questType questType;
    public int targetCount;
    public int count = 0;
    public int minLevel;
    public int maxLevel;
    public bool isClear = false;
    
}
