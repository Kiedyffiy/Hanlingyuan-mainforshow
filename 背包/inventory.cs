using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item" , menuName = "Inventory/New Inventory")]
//����һ����Ʒ����
public class inventory :ScriptableObject
{
    public List<Item> itemList = new List<Item>();

}
