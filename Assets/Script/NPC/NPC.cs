using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField]
    private float npcSight;
    [SerializeField]
    private bool showMe;
    [SerializeField]
    private float rotationDemp;
    Transform target;
    [SerializeField]
    private GameObject[] wapons;
    private int questProgress = 0;
    private int usingCount = 0;
    public List<Quest> quests = new List<Quest>();
    public TalkBox talkBox;
    private Coroutine talkCorutine;
    
    // Start is called before the first frame update
    void Start()
    {
        target = FindObjectOfType<PlayerMove>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, target.position)<=npcSight&&showMe){
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(target.position-transform.position), rotationDemp * Time.deltaTime) ;
            transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
        }
    }
    public void TalkWithNPC(){
        if(talkCorutine==null)
            talkCorutine = StartCoroutine(GoTalk());
    }
    private IEnumerator GoTalk(){
        // if(PlayerGoldManager.Instance.GetGold()>=100){
        //     PlayerGoldManager.Instance.UseGold(60);
        //     Instantiate(wapons[0], transform.position + Vector3.forward, Quaternion.identity);
        // }
        // else if(PlayerGoldManager.Instance.GetGold()>=60){
        //     PlayerGoldManager.Instance.UseGold(60);
        //     Instantiate(wapons[1], transform.position + Vector3.forward, Quaternion.identity);
        // }
        if(quests.Count>usingCount&&usingCount<=questProgress&&QuestManager.Instance.questLevel >= quests[questProgress].minLevel){
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

            QuestManager.Instance.RemoveQuest(quests[questProgress]);
            questProgress++;
            talkBox.UpdateText("수고했어~");
            yield return new WaitForSeconds(2f);
            talkBox.ShowThis = false;

        }else{
            talkBox.UpdateText("내가 맡긴일은 다 한거야?");
            yield return new WaitForSeconds(2f);
            talkBox.ShowThis = false;
        }
        talkCorutine = null;
        
    }
}
