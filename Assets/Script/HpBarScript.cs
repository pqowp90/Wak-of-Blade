using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBarScript : MonoBehaviour
{
    [SerializeField]
    private Transform canvas;

    List<Enemy>m_objectList = new List<Enemy>();
    List<Hpbar>m_hpBarList = new List<Hpbar>();
    Camera m_camera = null;
    void Start()
    {
        m_camera = Camera.main;
        

        Enemy[] t_Object = GameObject.FindObjectsOfType<Enemy>();
        foreach (var g_Object in t_Object)
        {
            m_objectList.Add(g_Object);
            Hpbar t_Hpbar = PoolManager.GetItem<Hpbar>("HpBar");
            t_Hpbar.transform.SetParent(canvas);
            t_Hpbar.enemyTransform = g_Object.transform;
            
            g_Object.SetHpbar(t_Hpbar);
            m_hpBarList.Add(t_Hpbar);
        }
    }

    // Update is called once per frame
    
    void Update()
    {
        for (int i=0;i<m_hpBarList.Count;i++)
        {
            if(m_objectList[i]==null||m_objectList[i].gameObject.activeSelf==false){
                m_hpBarList[i].ShowThis = false;
                //m_objectList.RemoveAt(i);
                //m_hpBarList.RemoveAt(i);
                continue;
            }
            

            //Vector3.Dot(m_hpBarList[i].transform.position, Camera.main.transform.forward)<=0
            m_hpBarList[i].inScreen = GameManager.InScreen(m_objectList[i].transform);

            m_hpBarList[i].transform.position = m_camera.WorldToScreenPoint(m_objectList[i].transform.position + Vector3.up);
        }
    }
}
