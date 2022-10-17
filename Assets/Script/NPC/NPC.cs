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
    
    
    void Start()
    {
        skillUse = FindObjectOfType<SkillUse>();
        target = FindObjectOfType<PlayerMove>().transform;
    }

    void Update()
    {
        PlayerComeIntoSight();
    }
    private void PlayerComeIntoSight(){// 플레이어가 시야에 들어옴
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
            if(QuestManager.Instance.questLevel < quests[questProgress].minLevel){  // 진행도가 부족할때(아직 미션이 없음)


                talkBox.UpdateText("심부름좀 더 하다가 와라");

                
                yield return new WaitForSeconds(2f);
                talkBox.ShowThis = false;
            }
            else if(quests.Count>usingCount&&usingCount<=questProgress){            // 퀘스트를 받을수 있는 상태가 되었다
                Quest quest = quests[usingCount];// 새 퀘스트를 받고 카운트를 올림 
                usingCount++;
                
                for(int i=0;i<quest.questDiscription.Length;++i){// 정해진 대사를 말한다
                    talkBox.UpdateText(quest.questDiscription[i]);
                    yield return new WaitForSeconds(quest.questDiscription[i].Length * 0.1f);
                }
                
                QuestManager.Instance.AddQuest(quest);// 퀘스트 추가!


                talkBox.ShowThis = false;
            }
            else if(quests[questProgress].isClear){                                 // 퀘스트를 완료시 진행도를 증가시키고 퀘스트를 지운다
                Quest quest = quests[questProgress];
                QuestManager.Instance.RemoveQuest(quests[questProgress]);

                if(quest.returnCourtesyPrefab!=null)// 줄 보상이 있으면 보상을 준다
                    Instantiate(quest.returnCourtesyPrefab, transform.position + Vector3.forward, Quaternion.identity);

                if(quest.returnCourtesySkillType != UnlockSkillType.none){// 줄 스킬이 있으면 스킬을 준다
                    skillUse.UnlockSkillType(quest.returnCourtesySkillType);
                }

                for(int i=0;i<quest.questClearDiscription.Length;++i){// 퀘스트를 완료할때 나오는 대사를 말한다
                    talkBox.UpdateText(quest.questClearDiscription[i]);
                    yield return new WaitForSeconds(quest.questClearDiscription[i].Length * 0.1f);
                }

                QuestManager.Instance.questLevel += quests[questProgress].levelUp;// 레벨을 올린다
                questProgress++;


                yield return new WaitForSeconds(2f);
                talkBox.ShowThis = false;

            }else{                                                                  // 퀘스트가 진행중인데 아직 완료를 안했을때


                talkBox.UpdateText("내가 맡긴일은 다 한거야?");

                
                yield return new WaitForSeconds(2f);
                talkBox.ShowThis = false;
            }
        }
        talkCorutine = null;
        
    }
}
