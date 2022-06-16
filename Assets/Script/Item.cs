using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType{
    Wapon,
    Accessories,
}
[CreateAssetMenu(fileName = "New Item", menuName = "Item/Create New Item")]
public class Item : ScriptableObject
{
    public int id;
    public string itemName;
    public string description;
    public Sprite itemImage;
    public GameObject prefab;

    //타입
    public ItemType itemTpye;
    //무기
    public int damage;
    public int attackSpeed;
    //장신구
    public int additionalAttackSpeed;
}
