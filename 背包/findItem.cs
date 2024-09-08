using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class findItem : MonoBehaviour
{
    public inventory playerinventory;
    public Transform originalParent;
    // Start is called before the first frame update
    
    
    public void ItemUse()
    {
        Debug.Log("Listener 的方法！");
        GameObject bunt = null;
        originalParent = transform.parent;
        //点击物品的下标
        int index = originalParent.GetComponent<Slot>().slotId;
        playerinventory = GameObject.FindGameObjectWithTag("Player").GetComponent<invent>().playerbag;

        //GameObject it = Instantiate(playerinventory.itemList[index].wuti);
        //it.transform.GetComponent<npc0>().name0();
        Debug.Log(index);
        playerinventory.itemList[index].wuti.transform.GetComponent<npc0>().name0();
        Debug.Log(index);
        foreach (Transform child1 in transform)
        {
            Debug.Log(child1.gameObject.name);
            if (child1.gameObject.name == "use")
            {
                bunt = child1.gameObject;
                break;
            }
        }
        bunt.SetActive(false);

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
