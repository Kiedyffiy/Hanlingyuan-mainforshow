using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public List<dialogueItem> buttonItems = new List<dialogueItem>();//对话列表
    public int select = -1;
    public Item thisItem;
    public inventory playerinventory;
    public GameObject Item;
    float ScrollWheel;

    void Start()
    {
        playerinventory = GameObject.FindGameObjectWithTag("Player").GetComponent<invent>().playerbag;
        Item = null;
        buttonItems.Clear();//清空列表
    }

    void Update()
    {
        // for(int i = 0; i < buttonItems.Count; i++)
        // {
        //     if(buttonItems[i].Dialog.Show == 1)
        //     {
        //         buttonItems.RemoveAt(i);
        //         i--;
        //     }
        // }
        //buttonItems.RemoveAll(x => x.Dialog.Show == 1);
        if(buttonItems.Count > 0)
        {
            ScrollWheel = Input.GetAxis("Mouse ScrollWheel");

            if(ScrollWheel != 0)
            {
                //print(ScrollWheel);
                select += (int)(ScrollWheel*10);
                toSelectButton();
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                buttonItems[select].dialogStart();
                if (buttonItems[select].item != null)
                {
                    Item = buttonItems[select].item;
                    thisItem = Item.transform.GetComponent<NPCController>().thisItem;
                    GetItem();
                    buttonItems.Clear();
                }
            }
                
        }
    }

    public void InitItem()
    {
        buttonItems = new List<dialogueItem>();
        buttonItems.Clear();
        select = -1;
        for(int i = 0;i < this.transform.childCount; i++)
        {
            if(i > buttonItems.Count - 1)
                buttonItems.Add(this.transform.GetChild(i).GetComponent<dialogueItem>());
            else
                buttonItems[i] = this.transform.GetChild(i).GetComponent<dialogueItem>();
        }
        if(buttonItems.Count > 0)
        {
            //select = 0;
            select = buttonItems.Count - 1;
            for(int i = 0; i < buttonItems.Count; i++)
            {
                buttonItems[i].SelectImage.SetActive(false);
            }
            buttonItems[buttonItems.Count-1].SelectImage.SetActive(true);
            //buttonItems[0].SelectImage.SetActive(true);
        }
    }

    void toSelectButton()
    {
        for(int i = 0; i < buttonItems.Count; i++)
        {
            buttonItems[i].SelectImage.SetActive(false);
        }
        
        if(select<0)
        {
            select = 0;
            buttonItems[select].SelectImage.SetActive(true);
        }
        else if(select > buttonItems.Count - 1)
        {
            select = buttonItems.Count - 1;
            buttonItems[select].SelectImage.SetActive(true);
        }
        else 
            buttonItems[select].SelectImage.SetActive(true);
    }
    public void GetItem()
    {

        //如果玩家碰到地图上的物体，则将其加入到玩家背包
        AddnewItem();
        //摧毁地图上的物品（因为已经加入背包）
        if (Item != null)
            Destroy(Item);

    }

    private void AddnewItem()
    {
        if (!playerinventory.itemList.Contains(thisItem))
        {
            //playerinventory.itemList.Add(thisItem);
            // InventoryManager.CreateNewItem(thisItem);
            //如果没有该物品，则在背包中找到空位将物品放入
            for (int i = 0; i < playerinventory.itemList.Count; i++)
            {
                if (playerinventory.itemList[i] == null)
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



