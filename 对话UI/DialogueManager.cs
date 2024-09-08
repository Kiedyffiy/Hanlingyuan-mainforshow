using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MxM;
using UnityEngine.AI;
using Cinemachine;

public class DialogueManager : MonoBehaviour
{
    public List<Con_DialogData> allDialoglist = new List<Con_DialogData>();//所有对话列表

    public Transform dialogueButtons;//对话按钮框
    public Object ButtonsItem;//对话按钮预制体
    public GameObject item;
    public Transform dialogueReply;//回复按钮框
    public Object ReplyItem;//回复按钮预制体
    public GameObject player;
    public GameObject Hutao;
    public GameObject hutao;
    public GameObject Yupei;
    public Transform dialogueBox;//对话框
    public Dialoglogic dialoglogic;//对话逻辑处理类
    public ButtonManager buttonManager1;
    public ButtonManager buttonManager;
    //public GameObject Playerpos;
    public GameManager gameManager;
    public BlackScreen blackScreen;//黑屏

    public Black black;//黑屏带文本
    public DialogScene dialogScene;
    private void Awake()
    {
        dialogueButtons.gameObject.SetActive(false);
        dialogueReply.gameObject.SetActive(false);
        dialogueBox.gameObject.SetActive(false);
        dialoglogic.AutoEnd.SetActive(false);
        dialoglogic.AutoStart.SetActive(false);
        blackScreen.gameObject.SetActive(false);
    }

    public void dialogbuttonInit(int npcID)//对话选择按钮
    {
        dialogueButtons.gameObject.SetActive(true);
        dialogueReply.gameObject.SetActive(false);
        dialogueBox.gameObject.SetActive(false);
        dialoglogic.AutoEnd.SetActive(false);
        dialoglogic.AutoStart.SetActive(false);
    //    clearButton(dialogueButtons);

        for(int i = 0; i < allDialoglist.Count; i ++)
        {
            if(allDialoglist[i].NPCID == npcID)
            {
                //生成对话选择按钮
                GameObject selectButton = (GameObject)Instantiate(ButtonsItem, dialogueButtons);
                selectButton.GetComponent<dialogueItem>().Initbutton(allDialoglist[i], this);
                selectButton.GetComponent<dialogueItem>().item = this.item;

            }
        }
        Invoke("InitItem", 0.01F);
        //buttonManager.InitItem();
    }

    void InitItem()
    {
        buttonManager.InitItem();
    }
    void InitItem1()
    {
        buttonManager1.InitItem();
    }
    public void DialogButtonOver()
    {
        dialogueButtons.gameObject.SetActive(false);
        dialogueReply.gameObject.SetActive(false);
        dialogueBox.gameObject.SetActive(false);
        clearButton(dialogueButtons);
    }

    public void dialogReplyInit(Con_DialogData con_DialogData, int currentLine)//对话选项生成
    {
        dialogueReply.gameObject.SetActive(true);
        clearButton(dialogueReply);
        dialogueButtons.gameObject.SetActive(false);
        for(int i = 0; i < con_DialogData.dialoguedatas[currentLine].diaselect.Length; i++)//获取具体某行有几个对话选项的长度
        {
            GameObject selectButton = (GameObject)Instantiate(ReplyItem, dialogueReply);
            selectButton.GetComponent<dialogueItem>().InitSelect(this, con_DialogData, currentLine, i);
        }
        Invoke("InitItem1", 0.01F);
    }

    void clearButton(Transform transform)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        } 
    }

    public void dialogPlay(Con_DialogData Dialog, int start)
    {
        if ((int)Dialog.DialogStatus == 4)
        {
            dialogScene.ShowText.text = Dialog.dialoguedatas[0].Dialogue;
            dialogScene.gameObject.SetActive(true);
        }
        else
        {
            dialogueBox.gameObject.SetActive(true);
            dialogueReply.gameObject.SetActive(false);
            dialogueButtons.gameObject.SetActive(false);
            dialoglogic.DialogInit(Dialog, start);
        }


    }

    public void FinishDialog()
    {
        dialogueBox.gameObject.SetActive(false);
    }

    public void Doevent(string EventName)
    {
        //Debug.LogError("执行事件nei");
        if(EventName == "Blackscreen" || EventName == "Black")
        Invoke(EventName, 0.01F);
        else if (EventName == "tospeech")
        {
            Invoke("Black", 0.01f);
            Invoke(EventName, 1F);
        }else if(EventName == "endspeech")
        {
            Invoke("Black", 0.01f);
            Invoke(EventName, 1F);
        }else if(EventName == "aiai")
        {
            Invoke("Black", 0.01f);
            Invoke(EventName, 0.01F);
        }else if(EventName == "sit")
        {
            Invoke(EventName, 0.01F);
        }else if(EventName == "yupei")
        {
            Invoke("Black", 0.01f);
            Invoke(EventName, 1F);
        }else if (EventName == "sitwithblack")
        {
            Invoke("Black", 0.01f);
            Invoke(EventName, 1f);
            player.transform.GetComponent<Mxmevent>().isroom = true;
        }else if(EventName == "gotoclass")
        {
            Invoke("Black", 0.01f);
            Invoke(EventName, 1F);
        }else if(EventName == "endclass")
        {
            Invoke("Black", 0.01f);
            Invoke(EventName, 1F);
        }else if(EventName == "turntohutao")
        {
            Invoke("Black", 0.01f);
            Invoke(EventName, 1F);
        }else if(EventName == "changecamera")
        {
            Invoke("Black", 0.01f);
            Invoke(EventName, 1F);
        }else if(EventName == "lock")
        {
            gameManager.lockmove();
        }
        else if(EventName == "unlock")
        {
            gameManager.unlockmove();
        }else if(EventName == "notready")
        {
            Invoke("Black", 0.01f);
            Invoke(EventName, 1F);
        }else if(EventName == "ready")
        {
            Invoke("Black", 0.01f);
            player.transform.GetComponent<Mxmevent>().isroom = false;
            player.transform.GetComponent<Mxmevent>().Camera1.transform.GetComponent<CinemachineFreeLook>().enabled = true;
            player.transform.GetComponent<Mxmevent>().Camera2.transform.GetComponent<CinemachineVirtualCamera>().enabled = false;
            Invoke(EventName, 1F);
        }else if(EventName == "gogogo")
        {
            Invoke("Black", 0.01f);
            Invoke(EventName, 1F);
        }else if(EventName == "hehua1")
        {
            gameManager.setmsg("荷花？？");
        }else if (EventName == "hehua2")
        {
            gameManager.setmsg("有空了去池塘看看？");
        }else if(EventName == "hehua3")
        {
            gameManager.setmsg("这下不得不去看看荷花了。。");
        }else if(EventName == "shangke")
        {
            gameManager.setmsg("好好听课");
        }else if (EventName == "zhunbei")
        {
            gameManager.setmsg("了解篁林");
        }else if (EventName == "zhandou")
        {
            gameManager.unlockmove();
            gameManager.setmsg("靠近这些灵兽，尝试击倒他们");
        }else if (EventName == "ennd")
        {
            Invoke("Black", 0.01f);
            gameManager.setmsg("不妨试试按下数字键0？");
        }else if(EventName == "ds")
        {
            gameManager.pro.SetActive(true);
            Invoke("deleteshanzi", 0.02f);
        }

    }
    public void deleteshanzi()
    {
        if(gameManager.shanzi != null)
            Destroy(gameManager.shanzi);
    }
    public void gogogo()
    {
        gameManager.movePlayer("pos5");
    }
    public void ready()
    {
        gameManager.movePlayer("pos4");
        gameManager.isact2 = false;
        gameManager.isact3 = true;
        gameManager.isact4 = false;
        gameManager.isact1 = false;
    }
    public void notready()
    {
        gameManager.setmsg("准备好出发后和芳糅对话");
        gameManager.setotherid("miku");
    }
    public void changecamera()
    {
        gameManager.setmsg("前往广场中央参加开学教仪");
        gameManager.mainMC.GetComponent<CinemachineBrain>().enabled = true;
    }
    public void endclass()
    {
        gameManager.moveNPC2("pos4");
        gameManager.setmsg("四处逛逛，多多交流");
        player.transform.GetComponent<Tagswitch>().sittime = 1f;

    }
    public void Blackscreen()
    {
        //Debug.Log("黑屏！！");
        dialogueButtons.gameObject.SetActive(false);
        dialogueReply.gameObject.SetActive(false);
        dialogueBox.gameObject.SetActive(false);
        dialoglogic.AutoEnd.SetActive(false);
        dialoglogic.AutoStart.SetActive(false);
        blackScreen.gameObject.SetActive(true);
    }
    public void turntohutao()
    {
        hutao.transform.GetComponent<ai_nav1>().enabled = true;
        gameManager.movePlayer("pos0");
    }
    public void yupei()
    {
        Destroy(Yupei);
        gameManager.npcuntalkable();
        gameManager.moveSPNPC1("alharsen", "pos9");
        
    }
    public void gotoclass()
    {
        gameManager.setmsg("入座");
        gameManager.movePlayer("pos3");
        gameManager.isact1 = false;
        gameManager.isact2 = true;
        gameManager.isact3 = false;
        gameManager.isact4 = false;
    }
    public void sitwithblack()
    {
        player.transform.GetComponent<Tagswitch>().sittime = 99999f;
        player.transform.GetComponent<Tagswitch>().readytosit = true;
    }
    //public void talkwithbothboys
    public void sit()
    {
        player.transform.GetComponent<Tagswitch>().sittime = 5f;
        player.transform.GetComponent<Tagswitch>().readytosit = true;
    }
    public void Black()
    {
        dialogueButtons.gameObject.SetActive(false);
        dialogueReply.gameObject.SetActive(false);
        dialogueBox.gameObject.SetActive(false);
        dialoglogic.AutoEnd.SetActive(false);
        dialoglogic.AutoStart.SetActive(false);
        black.gameObject.SetActive(true);
    }
    public void test()
    {
        Debug.Log("测试成功！");
    }
    public void tospeech()
    {
        gameManager.movePlayer("pos2");
    }
    public void aiai()
    {
        gameManager.setotherid("aiai");
    }
    public void endspeech()
    {
        gameManager.moveNPC("pos3");
        gameManager.setnpcID();
        gameManager.npctalkable();
        gameManager.unlockmove();
        DialogButtonOver();
        gameManager.setmsg("四处逛逛，与同学们增进感情");
        //gameManager.setpersonid("hutao");
        Transform hutao = null;
        Transform pos = null;
        foreach(Transform child in Hutao.transform)
        {
            if(child.gameObject.tag == "NPC")
            {
                hutao = child;
            }else if (child.gameObject.tag == "pos3")
            {
                pos = child;
            }
        }
        hutao.GetComponent<ai_nav1>().place1 = pos.gameObject;
    }
    public void lockmove()
    {
        player.transform.GetComponent<MxMTrajectoryGenerator>().MaxSpeed = 0;
    }
    public void unlockmove()
    {
        player.transform.GetComponent<MxMTrajectoryGenerator>().MaxSpeed = 4;

    }
}
