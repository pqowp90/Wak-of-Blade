using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum UnlockSkillType{
    none,
    charge,
    uppercut,
}
public class NPC : MonoBehaviour
{
    [SerializeField]
    private float npcSight;
    [SerializeField]
    private bool showMe;
    [SerializeField]
    private float rotationDemp;
    Transform target;
    private int questProgress = 0;
    private int usingCount = 0;
    public List<Quest> quests = new List<Quest>();
    public TalkBox talkBox;
    private Coroutine talkCorutine;
    private SkillUse skillUse;
    
    
    // Start is called before the first frame update
    void Start()
    {
        skillUse = FindObjectOfType<SkillUse>();
        target = FindObjectOfType<PlayerMove>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, target.position)<=npcSight&&showMe){
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(target.position-transform.position), rotationDemp * Time.deltaTime) ;
            transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
            if(quests.Count<=questProgress||talkCorutine!=null)return;
            if(QuestManager.Instance.questLevel < quests[questProgress].minLevel)return;
            if((quests.Count>usingCount&&usingCount<=questProgress)||(quests[questProgress].isClear)){
                talkBox.UpdateText("!!!");
            }
        }
        
    }
    public void TalkWithNPC(){
        if(talkCorutine==null)
            talkCorutine = StartCoroutine(GoTalk());
    }
    private IEnumerator GoTalk(){
        
        if(quests.Count>questProgress){
            if(QuestManager.Instance.questLevel < quests[questProgress].minLevel){
                talkBox.UpdateText("심부름좀 더 하다가 와라");
                yield return new WaitForSeconds(2f);
                talkBox.ShowThis = false;
            }
            else if(quests.Count>usingCount&&usingCount<=questProgress){
                Quest quest = quests[usingCount];
                usingCount++;
                
                for(int i=0;i<quest.questDiscription.Length;++i){
                    talkBox.UpdateText(quest.questDiscription[i]);
                    yield return new WaitForSeconds(quest.questDiscription[i].Length * 0.1f);
                }
                
                QuestManager.Instance.AddQuest(quest);
                talkBox.ShowThis = false;
            }
            else if(quests[questProgress].isClear){
                Quest quest = quests[questProgress];
                QuestManager.Instance.RemoveQuest(quests[questProgress]);
                if(quest.returnCourtesyPrefab!=null)
                    Instantiate(quest.returnCourtesyPrefab, transform.position + Vector3.forward, Quaternion.identity);
                if(quest.returnCourtesySkillType != UnlockSkillType.none){
                    skillUse.UnlockSkillType(quest.returnCourtesySkillType);
                }
                for(int i=0;i<quest.questClearDiscription.Length;++i){
                    talkBox.UpdateText(quest.questClearDiscription[i]);
                    yield return new WaitForSeconds(quest.questClearDiscription[i].Length * 0.1f);
                }
                QuestManager.Instance.questLevel += quests[questProgress].levelUp;
                questProgress++;
                yield return new WaitForSeconds(2f);
                talkBox.ShowThis = false;

            }else{
                talkBox.UpdateText("내가 맡긴일은 다 한거야?");
                yield return new WaitForSeconds(2f);
                talkBox.ShowThis = false;
            }
        }
        talkCorutine = null;
        
    }
}
