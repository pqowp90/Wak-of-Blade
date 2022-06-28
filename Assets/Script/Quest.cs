using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestType{
    killMushroom,
    KillBigMushroom,
}


[System.Serializable]
public class Quest
{
    public string questName;
    public string[] questDiscription;
    public string[] questClearDiscription;
    public QuestType questType;
    public int targetCount;
    public int count = 0;
    public int minLevel;
    public int maxLevel;
    public bool isClear = false;
    public GameObject returnCourtesyPrefab;
    public UnlockSkillType returnCourtesySkillType;
    public int levelUp;
    
}
