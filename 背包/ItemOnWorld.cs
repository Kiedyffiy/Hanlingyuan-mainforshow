using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOnWorld : MonoBehaviour
{
    
    public Item thisItem;
    public inventory playerinventory;

    public void GetItem()
        //判断人物和物体的碰撞
    {
        
            //如果玩家碰到地图上的物体，则将其加入到玩家背包
            AddnewItem();
            //摧毁地图上的物品（因为已经加入背包）
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
        else//如果已经有了该物品，则数量增加
        {
            thisItem.itemHeld++;
        }
        //为了使背包显示同步变化，调用RefreshiItem
        InventoryManager.RefreshItem();
    }
    
}
