using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestManager : MonoSingleton<QuestManager>
{
    [SerializeField]
    private ContentSizeFitter contentSizeFitter;
    [SerializeField]
    private TextMeshProUGUI questList;
    public int questLevel;
    List<Quest> quests = new List<Quest>();

    public void AddQuest(Quest _quest){
        quests.Add(_quest);
        UpdatePanel();
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform) contentSizeFitter.transform);
    }
    public void RemoveQuest(Quest _quest){
        quests.Remove(_quest);
        UpdatePanel();
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform) contentSizeFitter.transform);
    }
    public void UpCount(QuestType _questType){
        foreach(var quest in quests){
            if(quest.questType == _questType){
                quest.count++;
                if(quest.count>=quest.targetCount){
                    quest.isClear = true;
                }
            }
        }
        UpdatePanel();
    }
    private void UpdatePanel(){
        string questListText = "";
        foreach(var quest in quests){
            questListText += 
            string.Format("{0} {1}/{2}\n", 
            quest.questName,((quest.count<=quest.targetCount)?quest.count:quest.targetCount),quest.targetCount);
        }
        questList.text = questListText;
    }

}
