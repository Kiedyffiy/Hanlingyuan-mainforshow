using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DialogStatus
{
    Com=0, //普通
    shop=1, //商店
    Quest,  //任务
    Triggered, //自动触发
    Scene,
}

[CreateAssetMenu(fileName = "Configuration", menuName = "DialogSystem/DialogConf")]
public class Con_DialogData : ScriptableObject
{
    public int NPCID;//对话者ID
    public string DialogName;//对话名字
    public DialogStatus DialogStatus;//任务状态
    public int Show;//表示是否显示
    public DialogData[] dialoguedatas;//对话数据
    [Serializable]//结构体
    public struct DialogData
    {
        public string charName;//角色名字
//        public Color NameColor;//角色颜色 总是选择太麻烦我后面删去了
        [TextArea(1, 5)]
        public string Dialogue;//对话内容
        public AudioClip DialogueVoice;//对话语音
        public selectDialog[] diaselect;//回话选项
        public string Event;
        public bool end;
    }
    [Serializable]
    public struct selectDialog
    {
        public int next;//下一条回话入口
        public string selectText;//回话内容  如果为空则无选项直接跳转
    }
}
