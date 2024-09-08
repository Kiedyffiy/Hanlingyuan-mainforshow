using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//存储物品的各种信息
public class Slot : MonoBehaviour
{
    public Item slotItem;
    public Image slotImage;
    public Text slotNum;
    public string slotInfo;
    public int slotId;//空格id也是物品id
    public GameObject itemInSlot;

    public void itemOnCliked()
    {
        //slotInfo = slotItem.itemInfo;
        InventoryManager.updateInfo(slotInfo);
        GameObject bunt = null;

        foreach(Transform child in transform)
        {
            Debug.Log(child.gameObject.name);
            foreach (Transform child1 in child)
            {
                Debug.Log(child1.gameObject.name);
                if (child1.gameObject.name == "use")
                {
                    bunt = child1.gameObject;
                    break;
                }
            }
                
           
        }
        bunt.SetActive(true);
    }
    public void SetupSlot(Item item)//用于交换位置
    {
        if(item == null)
        {
            itemInSlot.SetActive(false);
            return;
        }
        slotImage.sprite = item.itemImage;
        slotNum.text = item.itemHeld.ToString();
        slotInfo = item.itemInfo;
    }
}
