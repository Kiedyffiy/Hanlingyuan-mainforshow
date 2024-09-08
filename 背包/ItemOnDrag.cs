using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class ItemOnDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform originalParent;
    public inventory myBag;
    public int currentItemID;
    public void OnBeginDrag(PointerEventData eventData)
    {
        //获取物品最初的位置信息，eventdata是鼠标点击的信息
        originalParent = transform.parent;
        currentItemID = originalParent.GetComponent<Slot>().slotId;
        transform.SetParent(transform.parent.parent);
        transform.position = eventData.position;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
        Debug.Log(eventData.pointerCurrentRaycast.gameObject.name);//输出鼠标当前位置下到第一个碰到的物体名字
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(eventData.pointerCurrentRaycast.gameObject.name == "Image")
        {

            
            //交换别拖拽物体的位置
           transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform.parent.parent);
           transform.position = eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.position;
           // 物品存储位置改变
                 var temp = myBag.itemList[currentItemID];
                //鼠标点击物品获得物品当前id（存放在数组中的位置）
                 myBag.itemList[currentItemID] = myBag.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotId];
                 myBag.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotId] = temp;
            //交换原来位置物体的位置
            eventData.pointerCurrentRaycast.gameObject.transform.parent.position = originalParent.position;
            eventData.pointerCurrentRaycast.gameObject.transform.parent.SetParent(originalParent);
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            //InventoryManager.RefreshItem();
            return;
        }
       //if (eventData.pointerCurrentRaycast.gameObject.name == "Slot(Clone)")
        //{
        
            //否则直接挂在slot空格下面
            transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform);
            transform.position = eventData.pointerCurrentRaycast.gameObject.transform.position;
        //物品存储位置改变
        var temp2 = myBag.itemList[currentItemID];
       // if(eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotId != currentItemID)
       // {
            myBag.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotId] = myBag.itemList[currentItemID];
            myBag.itemList[currentItemID] = null;
       // }
        
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        //}
            
    }
}
