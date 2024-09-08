using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    static InventoryManager instance;
    public inventory temp;
    public inventory maBag;
    public GameObject player;
    public GameObject slotGrid;
    //public Slot slotPrefab;
    public GameObject emptySlot;
    public Text itemInfo;

    public List<GameObject> slots = new List<GameObject>();//管理生成的18个slots
    //控制模块
    void Awake()//不知道没解释，找找抄的
    {
        maBag = GameObject.FindGameObjectWithTag("Player").transform.GetComponent<invent>().playerbag;
        //maBag = Instantiate(temp);
        if (instance != null)
        {
            Destroy(this);
        }
        instance = this;
    }

    private void OnEnable()//用于更新物品介绍
    {
        RefreshItem();
        instance.itemInfo.text = "";
    }
    public static void updateInfo(string itemDescription)
    {
        instance.itemInfo.text = itemDescription;
    }
    /* public static void CreateNewItem(Item item)
    {
        Slot newItem = Instantiate(instance.slotPrefab, instance.slotGrid.transform.position, Quaternion.identity);
        newItem.gameObject.transform.SetParent(instance.slotGrid.transform);
        newItem.slotItem = item;
        newItem.slotImage.sprite = item.itemImage;
        newItem.slotNum.text = item.itemHeld.ToString();
    }*/
    public static void RefreshItem()
    {
        instance.maBag = GameObject.FindGameObjectWithTag("Player").transform.GetComponent<invent>().playerbag;
        for (int i =0; i<instance.slotGrid.transform.childCount; i++)//先删除背包原本的物品
        {
            if (instance.slotGrid.transform.childCount == 0)
                break;
            Destroy(instance.slotGrid.transform.GetChild(i).gameObject);
            instance.slots.Clear();
        }
        for(int i =0; i<instance.maBag.itemList.Count; i++)//再一个一个加新的物品
        {
            /*CreateNewItem(instance.maBag.itemList[i]);*/
            instance.slots.Add(Instantiate(instance.emptySlot));
            instance.slots[i].transform.SetParent(instance.slotGrid.transform);
            instance.slots[i].GetComponent<Slot>().slotId = i;
            instance.slots[i].GetComponent<Slot>().SetupSlot(instance.maBag.itemList[i]);
            //instance.slots[i].GetComponent<Slot>().SetupSlot(null);
        }
    }
}
