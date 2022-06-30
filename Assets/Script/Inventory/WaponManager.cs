using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaponManager : MonoSingleton<WaponManager>
{
    [SerializeField]
    private List<Transform> Wapons = new List<Transform>();
    private Transform lastWapon;
    private PlayerMove playerMove;
    [SerializeField]
    private Transform handle;
    [SerializeField]
    private GameObject end;
    private void Start(){
        Time.timeScale = 1f;
        playerMove = FindObjectOfType<PlayerMove>();
    }
    public void SetWaponThis(Item item){
        if(item.id==2){
            StartCoroutine(TheEnd());
        }
        if(playerMove==null)playerMove = FindObjectOfType<PlayerMove>();
        if(playerMove==null)return;
        if(item == null){
            playerMove.nowWapon = null;
            lastWapon?.gameObject.SetActive(false);
            lastWapon?.SetParent(transform);
            return;
        }
        if(lastWapon != null){
            lastWapon.gameObject.SetActive(false);
            lastWapon.SetParent(transform);
        }
        playerMove.nowWapon = item;
        Transform sword = Wapons[item.id];
        sword.SetParent(handle);
        sword.localPosition = Vector3.zero;
        sword.localRotation = Quaternion.identity;
        sword.gameObject.SetActive(true);
        playerMove.waponCollider = sword.GetComponentInChildren<Collider>();
        lastWapon = sword;
        
    }
    private IEnumerator TheEnd(){
        end.SetActive(true);
        Time.timeScale = 0f;
        
        while(true){
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            yield return new WaitForEndOfFrame();
        }
    }
}
