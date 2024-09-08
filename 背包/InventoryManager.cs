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

    public List<GameObject> slots = new List<GameObject>();//�������ɵ�18��slots
    //����ģ��
    void Awake()//��֪��û���ͣ����ҳ���
    {
        maBag = GameObject.FindGameObjectWithTag("Player").transform.GetComponent<invent>().playerbag;
        //maBag = Instantiate(temp);
        if (instance != null)
        {
            Destroy(this);
        }
        instance = this;
    }

    private void OnEnable()//���ڸ�����Ʒ����
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
        for (int i =0; i<instance.slotGrid.transform.childCount; i++)//��ɾ������ԭ������Ʒ
        {
            if (instance.slotGrid.transform.childCount == 0)
                break;
            Destroy(instance.slotGrid.transform.GetChild(i).gameObject);
            instance.slots.Clear();
        }
        for(int i =0; i<instance.maBag.itemList.Count; i++)//��һ��һ�����µ���Ʒ
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
