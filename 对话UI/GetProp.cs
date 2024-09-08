using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetProp : MonoBehaviour
{
    public Text PropName;//任务道具名称
//    public Text Tips;//提示获得任务道具
    public Image images;//道具图片
    public Image Imageback; //图片背景设置
 
    //开启自动播放
    private void OnEnable()
    {
        //Tips.text = "获得任务道具";
        StartCoroutine(Prop());
        
    }
 
    public IEnumerator Prop()
    {
        yield return new WaitForSeconds(4f);  //等待4秒后跳转
        gameObject.SetActive(false);
    }
}
