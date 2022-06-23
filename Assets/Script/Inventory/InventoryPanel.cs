using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPanel : MonoBehaviour, IPoolable
{
    public PanelAndItem panelAndItem;
    [SerializeField]
    private ShowItemInfo showItemInfo;
    [SerializeField]
    private Image itemImage;
    public void SetPanel() {
        itemImage.sprite = panelAndItem.item.itemImage;
    }
    public void ClickThis(){
        showItemInfo.SetItemInfo(panelAndItem);
        //Inventory.Instance.Remove(panelAndItem);
    }
    public void OnPool(){

    }
}
