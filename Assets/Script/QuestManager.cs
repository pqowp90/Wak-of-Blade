using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestManager : MonoSingleton<QuestManager>
{
    [SerializeField]
    private TextMeshProUGUI questList;
    public int questLevel;
    List<Quest> quests;

    public void AddQuest(Quest _quest){
        quests.Add(_quest);
        string questListText = "";
        foreach(var quest in quests){
            questListText += quest.questName + "\n";
        }
        questList.text = questListText;
    }
    public void UpCount(questType _questType){
        foreach(var quest in quests){
            if(quest.questType == _questType){
                quest.count++;
                if(quest.count>=quest.maxLevel){
                    quest.isClear = true;
                }
            }
        }
    }

}
