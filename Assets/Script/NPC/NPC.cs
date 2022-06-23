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
    public void GoTalk(){

    }
}
