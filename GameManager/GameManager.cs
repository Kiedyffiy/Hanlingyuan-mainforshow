using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MxM;
using Cinemachine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour//主管理
{
    public bool isact1;//处于哪一幕
    public bool isact2;
    public bool isact3;
    public bool isact4;
    public GameObject player;//获得玩家
    //public GameObject Hutao;
    public GameObject root;//获得对话根目录
    public GameObject Playerpos;//获得玩家位置组
    public GameObject freelook;//获得自由相机
    public GameObject cameraset;//获得相机组
    public GameObject mainMC;//获得主相机
    public GameObject mainpos;//获得主相机位置
    public GameObject text0;
    public GameObject text1;
    public GameObject text2;
    public GameObject pro;
    public GameObject shanzi;
    public Text Text;
    private bool dogalldie;
    public DialogueManager dialogueManager;
    public GameObject NPCpos;//获得NPC组位置
    public GameObject dogs;
    public GameObject NPCpos2;//获得NPC组2位置
    public GameObject OtherPerson;//获得NPC组3位置
    public GameObject MUSIC1;//获得音乐
    public GameObject MUSIC2;
    public GameObject MUSIC3;
    public GameObject MUSIC4;

    // Start is called before the first frame update
    void Start()
    {
        //lockmove();
        //cameraset.SetActive(false);
        //debugmode();
        finmode();//设置游玩模式
        setshow();
        isact1 = true;//初始环境设置
        isact2 = false;
        isact3 = false;
        isact4 = false;
        dogalldie = false;
        //dialogueManager.dialogbuttonInit(995);
    }
    
    // Update is called once per frame
    void Update()//更新
    {
        
        music_ctrl();//音乐控制
        if (!dogalldie)//所有狗狗死去
        {
            if (checkdogs())
            {
                dogalldie = true;
                stopallmusic();//停止所有音乐
                dialogueManager.dialogbuttonInit(6667);
            }
        }
    }
    public void setmsg(string msg)//任务指引
    {
        Text.text = msg;
    }
    void debugmode()//测试模式
    {
        Invoke("firstblack", 0.01f);
        Invoke("setcamera", 1f);
    }
    void finmode()//游玩模式
    {
        text0.SetActive(true);
        mainMC.transform.GetComponent<CinemachineBrain>().enabled = true;
        freelook.transform.GetComponent<CinemachineFreeLook>().enabled = false;
        cameraset.transform.GetComponent<CinemachineBlendListCamera>().enabled = true;
        Invoke("text1start", 40.5f);
        Invoke("text2start", 57.2f);
        Invoke("firstblack", 61.2f);
        Invoke("setcamera", 62.2f);
    }
    public void setshow()
    {
        DialogueManager dia = root.GetComponent<DialogueManager>();
        foreach(Con_DialogData i in dia.allDialoglist)
        {
            if (i.Show == 1) i.Show = 0;
        }
    }
    public void firstblack()//第一黑幕
    {
        dialogueManager.dialogbuttonInit(995);
    }
    public void setcamera()//相机设置
    {

        mainMC.transform.GetComponent<CinemachineBrain>().enabled = false;
        mainMC.transform.position = mainpos.transform.position;
        mainMC.transform.rotation = mainpos.transform.rotation;
        freelook.transform.GetComponent<CinemachineFreeLook>().enabled = true;
        cameraset.transform.GetComponent<CinemachineBlendListCamera>().enabled = false;
        cameraset.SetActive(false);
    }
    public bool checkdogs()//检查狗狗状态
    {
        if (dogs.transform.childCount == 0) return true;
        else return false;
    }
    public void stopallmusic()//停止所有音乐
    {
        isact1 = false;
        isact2 = false;
        isact3 = false;
        isact4 = false;
        MUSIC3.GetComponent<AudioSource>().Stop();
        MUSIC4.GetComponent<AudioSource>().Stop();
        MUSIC1.GetComponent<AudioSource>().Stop();
        MUSIC2.GetComponent<AudioSource>().Stop();
    }
    public void setpersonid(string npcname)//更新对话
    {
        bool findis = false;
        foreach (Transform child in NPCpos.transform)
        {
            
            foreach (Transform cchild in child)
            {
                //Debug.Log(cchild.gameObject.tag);
                if (cchild.gameObject.name == npcname)
                {
                    cchild.GetComponent<NPCController>().npcID += 1;
                    findis = true;
                    //Debug.Log("1");
                }
            }
            if (findis) break;
        }
    }
    public void setotherid(string npcname)//单独设置对话文本更新
    {
        foreach (Transform child in OtherPerson.transform)
        {
            if (child.gameObject.name == npcname)
            {
                child.GetComponent<NPCController>().npcID += 1;
                //Debug.Log("1");
            }
        }
    }
    
    public void setnpcID()//每一幕换对话
    {
        int offset_id = 1;
        if (isact2) offset_id = 2;
        if (isact3) offset_id = 3;
        foreach (Transform child in NPCpos.transform)
        {
            Transform npc = null;
            foreach (Transform cchild in child)
            {
                //Debug.Log(cchild.gameObject.tag);
                if (cchild.gameObject.tag == "NPC")
                {
                    npc = cchild;
                    //Debug.Log("1");
                }
            }
            if (npc != null && npc.GetComponent<NPCController>() != null) npc.GetComponent<NPCController>().npcID = npc.GetComponent<NPCController>().baseid + offset_id;

        }
    }
    void text1start()//推镜头文本1
    {
        text1.SetActive(true);
    }
    void text2start()//推镜头文本2
    {
        text2.SetActive(true);
    }
    public void movePlayer(string posnum)//移动玩家位置
    {
        Transform pos = null;
        foreach (Transform cchild in Playerpos.transform)
        {
            //Debug.Log(cchild.gameObject.tag);
            if (cchild.gameObject.tag == posnum)
            {
                pos = cchild;
                //Debug.Log("2");
            }
        }
        Vector3 targetPos = pos.position;
        Quaternion lookRotation = Quaternion.LookRotation(pos.forward);
        player.transform.position = targetPos;
        player.transform.rotation = lookRotation;

    }
    public void moveSPNPC1(string npcname,string posnum)//移动特定NPC位置（第一幕）
    {
        foreach (Transform child in NPCpos.transform)
        {

            Transform npc = null;
            foreach (Transform cchild in child)
            {
                //Debug.Log(cchild.gameObject.tag);
                if (cchild.gameObject.name == npcname)
                {
                    npc = cchild;
                    //Debug.Log("1");
                }
            }
            Transform pos = null;
            foreach (Transform cchild in child)
            {
                //Debug.Log(cchild.gameObject.tag);
                if (cchild.gameObject.tag == posnum)
                {
                    pos = cchild;
                    //Debug.Log("2");
                }
            }
            if (pos != null && npc != null)
            {
                Vector3 targetPos = pos.position;
                Quaternion lookRotation = Quaternion.LookRotation(pos.forward);
                npc.position = targetPos;
                npc.rotation = lookRotation;
            }
        }
    }
    public void moveNPC2(string posnum)//移动特定NPC位置第二幕
    {
        //Debug.Log(posnum);
        foreach (Transform child in NPCpos2.transform)
        {

            Transform npc = null;
            foreach (Transform cchild in child)
            {
                //Debug.Log(cchild.gameObject.tag);
                if (cchild.gameObject.tag == "NPC")
                {
                    npc = cchild;
                    
                    //Debug.Log("1");
                }
            }
            Transform pos = null;
            foreach (Transform cchild in child)
            {
                //Debug.Log(cchild.gameObject.tag);
                if (cchild.gameObject.tag == posnum)
                {
                    pos = cchild;
                    //Debug.Log("2");
                }
            }
            if (pos != null && npc != null)
            {
                npc.GetComponent<sitorstand>().isstand = true;
                Vector3 targetPos = pos.position;
                Quaternion lookRotation = Quaternion.LookRotation(pos.forward);
                npc.position = targetPos;
                npc.rotation = lookRotation;
            }
        }
    }
    public void moveNPC(string posnum)//移动NPC任意
    {
        //Debug.Log(posnum);
        foreach(Transform child in NPCpos.transform)
        {

            Transform npc = null; 
            foreach(Transform cchild in child)
            {
                //Debug.Log(cchild.gameObject.tag);
                if (cchild.gameObject.tag == "NPC")
                {
                    npc = cchild;
                    //Debug.Log("1");
                }
            }
            Transform pos = null;
            foreach (Transform cchild in child)
            {
                //Debug.Log(cchild.gameObject.tag);
                if (cchild.gameObject.tag == posnum)
                {
                    pos = cchild;
                    //Debug.Log("2");
                }
            }
            if (pos != null && npc != null)
            {
                Vector3 targetPos = pos.position;
                Quaternion lookRotation = Quaternion.LookRotation(pos.forward);
                npc.position = targetPos;
                npc.rotation = lookRotation;
            }
        }
    }
    void music_ctrl()//音乐控制
    {
        if (isact1 && !MUSIC1.GetComponent<AudioSource>().isPlaying)
        {
            MUSIC3.GetComponent<AudioSource>().Stop();
            MUSIC4.GetComponent<AudioSource>().Stop();
            MUSIC2.GetComponent<AudioSource>().Stop();
            MUSIC1.GetComponent<AudioSource>().Play();
        }
        else if (isact2 && !MUSIC2.GetComponent<AudioSource>().isPlaying)
        {
            MUSIC1.GetComponent<AudioSource>().Stop();
            MUSIC4.GetComponent<AudioSource>().Stop();
            MUSIC3.GetComponent<AudioSource>().Stop();
            MUSIC2.GetComponent<AudioSource>().Play();
        }
        else if (isact3 && !MUSIC3.GetComponent<AudioSource>().isPlaying)
        {
            MUSIC1.GetComponent<AudioSource>().Stop();
            MUSIC4.GetComponent<AudioSource>().Stop();
            MUSIC2.GetComponent<AudioSource>().Stop();
            MUSIC3.GetComponent<AudioSource>().Play();
        }else if(isact4 && !MUSIC4.GetComponent<AudioSource>().isPlaying)
        {
            MUSIC1.GetComponent<AudioSource>().Stop();
            MUSIC3.GetComponent<AudioSource>().Stop();
            MUSIC2.GetComponent<AudioSource>().Stop();
            MUSIC4.GetComponent<AudioSource>().Play();
        }
    }
    public void lockmove()//锁住玩家移动
    {
        player.transform.GetComponent<MxMTrajectoryGenerator>().MaxSpeed = 0;
    }
    public void unlockmove()//解锁玩家移动
    {
        player.transform.GetComponent<MxMTrajectoryGenerator>().MaxSpeed = 4;

    }
    public void npctalkable()//npc是否可以对话
    {
        foreach (Transform child in NPCpos.transform)
        {
            foreach (Transform cchild in child)
            {
                if (cchild.gameObject.tag == "NPC")
                {
                    if (cchild.GetComponent<Collider>() != null)
                        cchild.GetComponent<Collider>().enabled = true;
                }
            }
        }
    }
    public void npcuntalkable()//npc不能对话
    {
        foreach (Transform child in NPCpos.transform)
        {
            foreach (Transform cchild in child)
            {
                if (cchild.gameObject.tag == "NPC")
                {
                    if (cchild.GetComponent<Collider>() != null)
                        cchild.GetComponent<Collider>().enabled = false;
                }
            }
        }
    }
}
