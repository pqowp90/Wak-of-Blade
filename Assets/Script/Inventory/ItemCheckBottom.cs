using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCheckBottom : MonoBehaviour
{
    [SerializeField]
    private Transform center;
    [SerializeField]
    private float radius;
    private PickUpItem pickUpItem;
    [SerializeField]
    Collider[] hitColliders;
    private bool ChackBottom(){
        float maxDistance = 100f;
        hitColliders = null;
        hitColliders = Physics.OverlapSphere(center.position, radius, 1<<LayerMask.NameToLayer("Item"));
        if(hitColliders.Length==0)return false;
        Collider selectedCollider = hitColliders[0];
        foreach(Collider collider in hitColliders){
            float distance = Vector3.Distance(transform.position, collider.transform.position);
            if(distance<maxDistance){
                maxDistance = distance;
                selectedCollider = collider;
            }
        }
        pickUpItem = selectedCollider.GetComponent<PickUpItem>();
        return true;
    }
    [SerializeField]
    private Transform center2;
    [SerializeField]
    private float radius2;
    private NPC npc;
    private bool ChackForward(){
        float maxDistance = 100f;
        Collider[] hitColliders = Physics.OverlapSphere(center2.position, radius2, 1<<LayerMask.NameToLayer("NPC"));
        if(hitColliders.Length==0)return false;
        Collider selectedCollider = hitColliders[0];
        foreach(Collider collider in hitColliders){
            float distance = Vector3.Distance(transform.position, collider.transform.position);
            if(distance<maxDistance){
                maxDistance = distance;
                selectedCollider = collider;
            }
        }
        npc = selectedCollider.GetComponent<NPC>();
        return true;
    }
    void Update()
    {

        if(ChackBottom()){
            if(Inventory.Instance.isFull()){
                ShowKeyUI.Instance.ShowUI(Key_UI_State.full);
            }else
                ShowKeyUI.Instance.ShowUI(Key_UI_State.pickUp);
            if(Input.GetKeyDown(KeyCode.F)){
                pickUpItem.PickUp();
            }
        }else if(ChackForward()){
            ShowKeyUI.Instance.ShowUI(Key_UI_State.talk);
            if(Input.GetKeyDown(KeyCode.F)){
                npc.TalkWithNPC();
            }
            
        }
        
    }
    private void OnDrawGizmos(){
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawSphere(center.position, radius);
    }
}
