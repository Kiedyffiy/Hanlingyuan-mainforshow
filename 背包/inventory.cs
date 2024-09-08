using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item" , menuName = "Inventory/New Inventory")]
//创建一个物品背包
public class inventory :ScriptableObject
{
    public List<Item> itemList = new List<Item>();

}
