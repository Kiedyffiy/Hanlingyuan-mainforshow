using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutPut : MonoBehaviour
{
    public GameObject thisitem;

    public void itemOnCliked()
    {
        Instantiate(thisitem);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
