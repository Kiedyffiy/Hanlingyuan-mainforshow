using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class invent : MonoBehaviour
{
    //public GameObject bag;
    public Item thisItem = null;
    public inventory playerbag;
    public inventory temp;
    public bool isIn = false;

    public bool Init(Item thisItem)
    {
        if (!playerbag.itemList.Contains(thisItem))
        {
            isIn = true;
            return isIn;
        }
        else return isIn;
    }
    // Start is called before the first frame update
    void Start()
    {
       
           playerbag = Instantiate(temp);
        Debug.Log(playerbag.itemList.Count + "uuu");
          // playerbag = Instantiate(temp);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
