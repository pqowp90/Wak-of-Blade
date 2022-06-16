using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    [SerializeField]
    private Item item;
    public void PickUp(){
        Inventory.Instance.AddItem(item);
        gameObject.SetActive(false);
    }
}
