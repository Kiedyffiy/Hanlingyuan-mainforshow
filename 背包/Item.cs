using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "New Item",menuName = "Inventory/New Item")]

//创建一个Item类，作为存储物品信息的模块
public class Item : ScriptableObject 
{
    public string itemName;
    public Sprite itemImage;
    public int itemHeld;
    public GameObject wuti;
    [TextArea]
    public string itemInfo;


    public bool equip;
    
}
