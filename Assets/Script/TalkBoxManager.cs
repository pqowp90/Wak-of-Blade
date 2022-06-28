using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkBoxManager : MonoBehaviour
{
    [SerializeField]
    private Transform canvas;


    List<NPC>m_objectList = new List<NPC>();
    List<TalkBox>m_TalkBoxList = new List<TalkBox>();
    Camera m_camera = null;
    void Start()
    {
        m_camera = Camera.main;
        

        NPC[] t_Object = GameObject.FindObjectsOfType<NPC>();
        foreach (var g_Object in t_Object)
        {
            m_objectList.Add(g_Object);
            TalkBox t_TalkBox = PoolManager.GetItem<TalkBox>("TalkBox");
            t_TalkBox.transform.SetParent(canvas);
            t_TalkBox.npcTransfrom = g_Object.transform;
            
            g_Object.talkBox = t_TalkBox;
            m_TalkBoxList.Add(t_TalkBox);
        }
    }
    private void Update() {
        for (int i=0;i<m_TalkBoxList.Count;i++)
        {
            if(m_objectList[i]==null){
                m_TalkBoxList[i].gameObject.SetActive(false);
                m_objectList.RemoveAt(i);
                m_TalkBoxList.RemoveAt(i);
                continue;
            }
            

            //Vector3.Dot(m_hpBarList[i].transform.position, Camera.main.transform.forward)<=0
            m_TalkBoxList[i].inScreen = GameManager.InScreen(m_objectList[i].transform);

            m_TalkBoxList[i].transform.position = m_camera.WorldToScreenPoint(m_objectList[i].transform.position + Vector3.up);
        }
    }
}
