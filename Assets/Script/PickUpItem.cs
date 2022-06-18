using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour, IPoolable
{
    [SerializeField]
    private Item item;

    public void OnPool()
    {
        
    }

    public void PickUp(){
        Inventory.Instance.AddItem(item, gameObject);
    }
}
