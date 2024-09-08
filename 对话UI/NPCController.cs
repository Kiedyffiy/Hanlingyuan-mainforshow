using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public int npcID;//NPCid 对应到文本
    public DialogueManager dialogueManager;//对话管理
    public Animator animator;//动作管理
    public GameObject player;//获得主角
    public int baseid;
    public bool issit;
    public bool isItem;
    public Item thisItem;
    //public inventory playerinventory;
    public bool ispick = false;//是否可以捡取
    private float sittingtime = 0f;
    private bool issitting = false;//是否为坐下的人
    //public Text DialogText;//对话文本内容
    // Start is called before the first frame update
    void Start()
    {

        foundPlayer();//寻找周围玩家
    }

    // Update is called once per frame
    void Update()
    {
        if (sittingtime > 0f) timecheck();//时间检查
    }
    void timecheck()
    {
        sittingtime -= Time.deltaTime;
        if (sittingtime <= 0f) issitting = false;
    }
    private void OnTriggerEnter(Collider other)//玩家靠近判断
    {
        if (isItem)
        {
            Debug.Log("isItem");
            dialogueManager.item = transform.gameObject;
        }
        else
            dialogueManager.item = null;
        if (!issitting)
        {
            if (other.tag == "Player")
            {
                foundPlayer();
                player.transform.GetComponent<Tagswitch>().Sitpos = gameObject;
                player.transform.GetComponent<Mxmevent>().ispick = ispick;
                dialogueManager.dialogbuttonInit(npcID);
                if (issit)
                {
                    issitting = true;
                    sittingtime = 6f;
                }
            }
        }
            
    }
    
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            dialogueManager.DialogButtonOver();
        }
            
    }
    bool foundPlayer()
    {
        var colliders = Physics.OverlapSphere(transform.position, 10f);
        foreach (var target in colliders)
        {
            if (target.CompareTag("Player"))
            {
                //Debug.Log("found!");
                player = target.transform.parent.gameObject;
                return true;
            }
        }
        player = null;
        // Debug.Log("not found!");
        return false;
    }
}
