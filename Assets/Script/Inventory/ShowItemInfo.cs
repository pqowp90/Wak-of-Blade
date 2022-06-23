using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowItemInfo : MonoBehaviour
{
    private Transform playerPos;
    [SerializeField]
    private Image itemImage;
    [SerializeField]
    private Text itemName;
    [SerializeField]
    private Text itemDescripsion;
    private PanelAndItem nowPanelAndItem;
    private void Start(){
        playerPos = FindObjectOfType<PlayerMove>().transform;
    }
    public void SetItemInfo(PanelAndItem panelAndItem){
        nowPanelAndItem = panelAndItem;
        itemImage.sprite = nowPanelAndItem.item.itemImage;
        itemName.text = nowPanelAndItem.item.itemName;
        itemDescripsion.text = nowPanelAndItem.item.description;
        gameObject.SetActive(true);
    }
    public void ThrowThisItem(){
        gameObject.SetActive(false);
        PoolManager.GetItem<PickUpItem>(nowPanelAndItem.item.PrefabName).transform.position = playerPos.position + Vector3.up;
        Inventory.Instance.Remove(nowPanelAndItem);
        WaponManager.Instance.SetWaponThis(null);
    }
    public void JangChakThisItem(){
        WaponManager.Instance.SetWaponThis(nowPanelAndItem.item);
    }
}
