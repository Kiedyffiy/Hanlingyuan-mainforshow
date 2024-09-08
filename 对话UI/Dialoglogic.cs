using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialoglogic : MonoBehaviour
{
    public DialogueManager Manager;

    public int currentLine;//对话索引
    public int currentLinenow;
    public Con_DialogData con_DialogData;//对话

    public Text DialogText;//对话文本内容
    public Text CharName;//对话主人名

    private bool canNext = true;
    public AudioSource m_SoundAudio;

    //自动播放
    public GameObject AutoStart;
    public GameObject AutoEnd;
    private bool autoPlay = false;
    private bool autoPlayNext = true;

    public AudioClip next;

    private void Awake()//声音
    {
        m_SoundAudio = this.gameObject.AddComponent<AudioSource>();
    }

    public void DialogInit(Con_DialogData DialogData, int start)//产生对话
    {
        con_DialogData = DialogData;
        currentLine = start;
        //Debug.Log("123!");
        canNext = true;
        autoPlayNext = true;
        AutoPlayEnd();
        playDialog();//播放对话方法
        if((int)con_DialogData.DialogStatus == 3){
            AutoEnd.SetActive(false);
            AutoStart.SetActive(false);
        }
    }

    void Update()
    {
        if(con_DialogData == null)
            return;
        if(autoPlay)
        {
            //播放对话
            if(currentLine < con_DialogData.dialoguedatas.Length && autoPlayNext)
                playDialog();
            else if(currentLine >= con_DialogData.dialoguedatas.Length && autoPlayNext)
            {
                this.gameObject.SetActive(false);
            }
        }
        else{
            //播放对话
            if(currentLine < con_DialogData.dialoguedatas.Length && (Input.GetKeyDown(KeyCode.G) || Input.GetMouseButtonDown(0)) && canNext)
            {
                playDialog();
               // Debug.Log("now  " + currentLine);
            }

            else if(currentLine >= con_DialogData.dialoguedatas.Length && (Input.GetKeyDown(KeyCode.G) || Input.GetMouseButtonDown(0)) && canNext)
            {
               // Debug.Log("false  " + currentLine);
                this.gameObject.SetActive(false);
            }
            else if (currentLine > con_DialogData.dialoguedatas.Length)
            {
                this.gameObject.SetActive(false);
            }
        }
    }

    public void playDialog()
    {
        m_SoundAudio.PlayOneShot(next);
        DialogText.text = "";//对话清空
        StopCoroutine("ShowDialog");
        canNext = false;
        autoPlayNext = false;
        currentLinenow = currentLine;
        //Debug.Log("123!!!!!");
        //播放语音
        if (con_DialogData.dialoguedatas[currentLine].DialogueVoice !=null)
            m_SoundAudio.PlayOneShot(con_DialogData.dialoguedatas[currentLine].DialogueVoice);

        //改变名字
        CharName.text = con_DialogData.dialoguedatas[currentLine].charName;
        //CharName.color = con_DialogData.dialoguedatas[currentLine].NameColor;

        //执行事件
        if (con_DialogData.dialoguedatas[currentLine].Event != "")
        {
           // Debug.LogError("执行事件");
            Manager.Doevent(con_DialogData.dialoguedatas[currentLine].Event);
        }
        else
        {
           // Debug.LogError(con_DialogData.dialoguedatas[currentLine].Event);
           // Debug.LogError(currentLine);
           // Debug.LogError("不执行事件");
        }

        //开启对话
        if(con_DialogData.dialoguedatas[currentLine].Event != "Black")StartCoroutine("ShowDialog");

        if(!autoPlay)//不是自动播放
            if(con_DialogData.dialoguedatas[currentLine].diaselect.Length == 0 || con_DialogData.dialoguedatas[currentLine].diaselect[0].selectText == "")//没有回话选项
                Invoke("CanNext", 0.5F);//可以跳转
    }

    IEnumerator ShowDialog()
    {
        currentLinenow = currentLine;
        //int t = 0;
        foreach (char c in con_DialogData.dialoguedatas[currentLine].Dialogue)//滚动输出字符
        {
            DialogText.text += c;
        //    t += 1;
            yield return new WaitForSeconds(0.04f);
            
        }
        // if((Input.GetKeyDown(KeyCode.G) || Input.GetMouseButtonDown(0)) && t > 5 )
        // {
        //     StartCoroutine("ShowAllDialog");
        // }
        if(autoPlay)
        {
            yield return new WaitForSeconds(1f);  //等待一秒后跳转
            autoPlayNext = true;
            CanNext();
        }
        if(con_DialogData.dialoguedatas[currentLinenow].diaselect.Length >0 && con_DialogData.dialoguedatas[currentLinenow].diaselect[0].selectText != "")//等待全部显示后再出现回复选项
        {
            yield return (string.Compare(DialogText.text, con_DialogData.dialoguedatas[currentLinenow].Dialogue) == 0);
            Invoke("CanNext", 0.1F);//跳转
        }
            
    }

    IEnumerator ShowAllDialog()//点击后显示全部语句
    {
        if(DialogText.text != con_DialogData.dialoguedatas[currentLinenow].Dialogue)
        {
            StopCoroutine("ShowDialog");
            DialogText.text = con_DialogData.dialoguedatas[currentLinenow].Dialogue;
        }
        if(autoPlay)
        {
            yield return new WaitForSeconds(1f);  //等待一秒后跳转
            autoPlayNext = true;
            CanNext();
        }
        if(con_DialogData.dialoguedatas[currentLinenow].diaselect.Length >0 && con_DialogData.dialoguedatas[currentLinenow].diaselect[0].selectText != "")//等待全部显示后再出现回复选项
        {
            Invoke("CanNext", 0.1F);//跳转
        }
    }

    void CanNext()//继续下一句
    {
        int flag =1;
        if(con_DialogData.dialoguedatas[currentLine].end)//对话结束
        {
            
            AutoStart.SetActive(false);
            AutoEnd.SetActive(false);
            if((int)con_DialogData.DialogStatus == 2)
            {
                con_DialogData.Show = 1;
            } 
            flag = 0;         
        }
        if(con_DialogData.dialoguedatas[currentLine].diaselect.Length == 0)//没有回话选择选项
        {
            canNext = true;
            currentLine++;
        }
        else if(con_DialogData.dialoguedatas[currentLine].diaselect.Length == 1 && con_DialogData.dialoguedatas[currentLine].diaselect[0].selectText == "") //无回话选项但跳转
        {
            canNext = true;
            currentLine = con_DialogData.dialoguedatas[currentLine].diaselect[0].next;
        }
        else//有回话选项
        {
            autoPlay = false;//停止播放下一句
            canNext = false;//停止手动播放下一句
            AutoStart.SetActive(false);
            AutoEnd.SetActive(false);
            Manager.dialogReplyInit(con_DialogData, currentLine);//创建回话按钮
        }
        if(flag == 0) currentLine = con_DialogData.dialoguedatas.Length;//相当于直接跳转到结尾。表示结束。
        
    }

    public void AutoPlayStart()
    {
        autoPlay = true;
        AutoStart.SetActive(false);
        AutoEnd.SetActive(true);
        autoPlayNext = true;
    }

    public void AutoPlayEnd()
    {
        autoPlay = false;
        AutoStart.SetActive(true);
        AutoEnd.SetActive(false);
        canNext = true;
    }
}
