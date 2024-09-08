using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "New Item",menuName = "Inventory/New Item")]

//����һ��Item�࣬��Ϊ�洢��Ʒ��Ϣ��ģ��
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
