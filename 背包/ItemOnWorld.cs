using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOnWorld : MonoBehaviour
{
    
    public Item thisItem;
    public inventory playerinventory;

    public void GetItem()
        //�ж�������������ײ
    {
        
            //������������ͼ�ϵ����壬������뵽��ұ���
            AddnewItem();
            //�ݻٵ�ͼ�ϵ���Ʒ����Ϊ�Ѿ����뱳����
            Destroy(gameObject);
        
    }

    private void AddnewItem()
    {
        if (!playerinventory.itemList.Contains(thisItem)){
            //playerinventory.itemList.Add(thisItem);
           // InventoryManager.CreateNewItem(thisItem);
           for(int i=0; i<playerinventory.itemList.Count; i++)
            {
                if(playerinventory.itemList[i] == null)
                {
                    playerinventory.itemList[i] = thisItem;
                    break;
                }
            }
        }
        else//����Ѿ����˸���Ʒ������������
        {
            thisItem.itemHeld++;
        }
        //Ϊ��ʹ������ʾͬ���仯������RefreshiItem
        InventoryManager.RefreshItem();
    }
    
}
