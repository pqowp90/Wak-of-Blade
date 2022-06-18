using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PanelAndItem{
        public PanelAndItem(Item _item, InventoryPanel _inventoryPanel){
            item = _item;
            inventoryPanel = _inventoryPanel;
        }
        public Item item;
        public InventoryPanel inventoryPanel;
    }
public class Inventory : MonoSingleton<Inventory>
{
    [SerializeField]
    private GameObject InventoryUI;
    [SerializeField]
    private int inventoryMax;
    [SerializeField]
    List<PanelAndItem> inventorySpace = new List<PanelAndItem>();
    [SerializeField]
    private Transform panelPrefab;
    Stack<InventoryPanel> removedPanels = new Stack<InventoryPanel>();
    private PlayerMove playerMove;
    private CameraMove cameraMove;
    private void Start(){
        PoolManager.CreatePool<PickUpItem>("Sword1Item", gameObject);
        PoolManager.CreatePool<PickUpItem>("Sword2Item", gameObject);
        PoolManager.CreatePool<PickUpItem>("Sword3Item", gameObject);
        playerMove = FindObjectOfType<PlayerMove>();
        cameraMove = FindObjectOfType<CameraMove>();
    }
    public void Update(){
        if(Input.GetKeyDown(KeyCode.E)){
            InventoryUI.SetActive(!InventoryUI.activeSelf);
            playerMove.NoInput = InventoryUI.activeSelf;
            cameraMove.NoInput = InventoryUI.activeSelf;
        }
    }
    public void AddItem(Item item, GameObject itemObject){
        if(isFull())return;
        itemObject.SetActive(false);
        InventoryPanel _inventoryPanel = GetPanel();
        PanelAndItem _panelAndItem = new PanelAndItem(item, _inventoryPanel);
        inventorySpace.Add(_panelAndItem);
        _inventoryPanel.transform.SetAsLastSibling();
        _inventoryPanel.panelAndItem = _panelAndItem;
        _inventoryPanel.SetPanel();
        _inventoryPanel.gameObject.SetActive(true);
    }
    private InventoryPanel GetPanel(){
        if(removedPanels.Count==0){
            return Instantiate(panelPrefab, panelPrefab.parent).GetComponent<InventoryPanel>();
        }else{
            return removedPanels.Pop();
        }
    }
    public bool isFull(){
        return (inventoryMax<=inventorySpace.Count);
    }
    public void Remove(PanelAndItem panelAndItem){
        removedPanels.Push(panelAndItem.inventoryPanel);
        panelAndItem.inventoryPanel.gameObject.SetActive(false);
        inventorySpace.Remove(panelAndItem);
    }
}
