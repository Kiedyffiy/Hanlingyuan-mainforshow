using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class showname : MonoBehaviour
    //挂在每个人身上，显示每个人的名字

{
    // Start is called before the first frame update
    public GameObject MyName;
    public Transform namePoint;
    Text thisName;
    Transform UIbar;
    public Transform tans;
    public Transform cam;
    public GameObject player;
    Vector3 direct = new Vector3();
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
     void Update()
    {
        this.transform.position = tans.position;
        this.transform.rotation = cam.rotation;
    }
    void turn()
    {
        //direct = player.transform.position - transform.position;
    }
}
