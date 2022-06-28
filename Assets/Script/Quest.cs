using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestType{
    killMushroom
}


[System.Serializable]
public class Quest
{
    public string questName;
    public string[] questDiscription;
    public QuestType questType;
    public int targetCount;
    public int count = 0;
    public int minLevel;
    public int maxLevel;
    public bool isClear = false;
    public GameObject returnCourtesyPrefab;
    
}
