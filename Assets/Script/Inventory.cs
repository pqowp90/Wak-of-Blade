using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoSingleton<Inventory>
{
    [SerializeField]
    private int inventoryMax;
    [SerializeField]
    List<Item> inventorySpace;
    public bool AddItem(Item item){
        bool isMax = (inventoryMax<=inventorySpace.Count);
        if(!isMax){
            inventorySpace.Add(item);
        }
        return isMax;
    }
    public void Remove(Item item){
        inventorySpace.Remove(item);
    }
}
