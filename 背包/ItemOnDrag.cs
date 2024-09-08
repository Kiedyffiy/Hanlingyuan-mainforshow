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
        //��ȡ��Ʒ�����λ����Ϣ��eventdata�����������Ϣ
        originalParent = transform.parent;
        currentItemID = originalParent.GetComponent<Slot>().slotId;
        transform.SetParent(transform.parent.parent);
        transform.position = eventData.position;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
        Debug.Log(eventData.pointerCurrentRaycast.gameObject.name);//�����굱ǰλ���µ���һ����������������
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(eventData.pointerCurrentRaycast.gameObject.name == "Image")
        {

            
            //��������ק�����λ��
           transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform.parent.parent);
           transform.position = eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.position;
           // ��Ʒ�洢λ�øı�
                 var temp = myBag.itemList[currentItemID];
                //�������Ʒ�����Ʒ��ǰid������������е�λ�ã�
                 myBag.itemList[currentItemID] = myBag.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotId];
                 myBag.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotId] = temp;
            //����ԭ��λ�������λ��
            eventData.pointerCurrentRaycast.gameObject.transform.parent.position = originalParent.position;
            eventData.pointerCurrentRaycast.gameObject.transform.parent.SetParent(originalParent);
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            //InventoryManager.RefreshItem();
            return;
        }
       //if (eventData.pointerCurrentRaycast.gameObject.name == "Slot(Clone)")
        //{
        
            //����ֱ�ӹ���slot�ո�����
            transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform);
            transform.position = eventData.pointerCurrentRaycast.gameObject.transform.position;
        //��Ʒ�洢λ�øı�
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
