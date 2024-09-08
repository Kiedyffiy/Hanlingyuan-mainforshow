using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MxM;
using Cinemachine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour//������
{
    public bool isact1;//������һĻ
    public bool isact2;
    public bool isact3;
    public bool isact4;
    public GameObject player;//������
    //public GameObject Hutao;
    public GameObject root;//��öԻ���Ŀ¼
    public GameObject Playerpos;//������λ����
    public GameObject freelook;//����������
    public GameObject cameraset;//��������
    public GameObject mainMC;//��������
    public GameObject mainpos;//��������λ��
    public GameObject text0;
    public GameObject text1;
    public GameObject text2;
    public GameObject pro;
    public GameObject shanzi;
    public Text Text;
    private bool dogalldie;
    public DialogueManager dialogueManager;
    public GameObject NPCpos;//���NPC��λ��
    public GameObject dogs;
    public GameObject NPCpos2;//���NPC��2λ��
    public GameObject OtherPerson;//���NPC��3λ��
    public GameObject MUSIC1;//�������
    public GameObject MUSIC2;
    public GameObject MUSIC3;
    public GameObject MUSIC4;

    // Start is called before the first frame update
    void Start()
    {
        //lockmove();
        //cameraset.SetActive(false);
        //debugmode();
        finmode();//��������ģʽ
        setshow();
        isact1 = true;//��ʼ��������
        isact2 = false;
        isact3 = false;
        isact4 = false;
        dogalldie = false;
        //dialogueManager.dialogbuttonInit(995);
    }
    
    // Update is called once per frame
    void Update()//����
    {
        
        music_ctrl();//���ֿ���
        if (!dogalldie)//���й�����ȥ
        {
            if (checkdogs())
            {
                dogalldie = true;
                stopallmusic();//ֹͣ��������
                dialogueManager.dialogbuttonInit(6667);
            }
        }
    }
    public void setmsg(string msg)//����ָ��
    {
        Text.text = msg;
    }
    void debugmode()//����ģʽ
    {
        Invoke("firstblack", 0.01f);
        Invoke("setcamera", 1f);
    }
    void finmode()//����ģʽ
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
    public void firstblack()//��һ��Ļ
    {
        dialogueManager.dialogbuttonInit(995);
    }
    public void setcamera()//�������
    {

        mainMC.transform.GetComponent<CinemachineBrain>().enabled = false;
        mainMC.transform.position = mainpos.transform.position;
        mainMC.transform.rotation = mainpos.transform.rotation;
        freelook.transform.GetComponent<CinemachineFreeLook>().enabled = true;
        cameraset.transform.GetComponent<CinemachineBlendListCamera>().enabled = false;
        cameraset.SetActive(false);
    }
    public bool checkdogs()//��鹷��״̬
    {
        if (dogs.transform.childCount == 0) return true;
        else return false;
    }
    public void stopallmusic()//ֹͣ��������
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
    public void setpersonid(string npcname)//���¶Ի�
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
    public void setotherid(string npcname)//�������öԻ��ı�����
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
    
    public void setnpcID()//ÿһĻ���Ի�
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
    void text1start()//�ƾ�ͷ�ı�1
    {
        text1.SetActive(true);
    }
    void text2start()//�ƾ�ͷ�ı�2
    {
        text2.SetActive(true);
    }
    public void movePlayer(string posnum)//�ƶ����λ��
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
    public void moveSPNPC1(string npcname,string posnum)//�ƶ��ض�NPCλ�ã���һĻ��
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
    public void moveNPC2(string posnum)//�ƶ��ض�NPCλ�õڶ�Ļ
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
    public void moveNPC(string posnum)//�ƶ�NPC����
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
    void music_ctrl()//���ֿ���
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
    public void lockmove()//��ס����ƶ�
    {
        player.transform.GetComponent<MxMTrajectoryGenerator>().MaxSpeed = 0;
    }
    public void unlockmove()//��������ƶ�
    {
        player.transform.GetComponent<MxMTrajectoryGenerator>().MaxSpeed = 4;

    }
    public void npctalkable()//npc�Ƿ���ԶԻ�
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
    public void npcuntalkable()//npc���ܶԻ�
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
