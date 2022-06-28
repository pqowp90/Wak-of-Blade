using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoSingleton<Respawn>
{
    private List<Enemy> monsters = new List<Enemy>();
    private List<Vector3> monsterPoss = new List<Vector3>();
    // Start is called before the first frame update
    void Start()
    {
        for(int i=0;i<transform.childCount;i++){
            monsters.Add(transform.GetChild(i).GetComponent<Enemy>());
            monsterPoss.Add(transform.GetChild(i).position);
        }
    }
    public void RespawnEveryone(){
        for(int i=0;i<transform.childCount;i++){
            monsters[i].transform.position = monsterPoss[i];
            monsters[i].HpReset();
            monsters[i].gameObject.SetActive(true);
        }
    }
}