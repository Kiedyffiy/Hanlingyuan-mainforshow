using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//�洢��Ʒ�ĸ�����Ϣ
public class Slot : MonoBehaviour
{
    public Item slotItem;
    public Image slotImage;
    public Text slotNum;
    public string slotInfo;
    public int slotId;//�ո�idҲ����Ʒid
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
    public void SetupSlot(Item item)//���ڽ���λ��
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
