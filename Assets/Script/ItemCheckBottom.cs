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
    private bool ChackBottom(){
        float maxDistance = 100f;
        Collider[] hitColliders = Physics.OverlapSphere(center.position, radius, 1<<LayerMask.NameToLayer("Item"));
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
    void Update()
    {
        if(ChackBottom()){
            if(Inventory.Instance.isFull()){
                ShowKeyUI.Instance.ShowUI(ShowKeyUI.Key_UI_State.full);
            }else
                ShowKeyUI.Instance.ShowUI(ShowKeyUI.Key_UI_State.pickUp);
            if(Input.GetKeyDown(KeyCode.F)){
                pickUpItem.PickUp();
            }
        }
        
    }
    private void OnDrawGizmos(){
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(center.position, radius);
    }
}
