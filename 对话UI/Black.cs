using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//该脚本实现黑屏上显示文本
public class Black : MonoBehaviour 
{
    public Image image;//覆盖屏幕的一张全黑图片
    public AnimationCurve curve1; //在Inspector上调整自己喜欢的曲线
    public AnimationCurve curve2;
    public Dialoglogic dialoglogic;
    public Text ShowText; //文本位置
    private bool isMouseClicked;
    [Range(0.5f, 2f)]public float speed = 1f; //控制渐入渐出的速度
 
    private void Awake()
    {
        if (image == null) 
            image = GetComponent<Image>();
    }
 
    //开启自动播放黑屏
    private void OnEnable()
    {
        StartCoroutine(BlackSc());
        
    }
 
    Color tmpColor; //用于传递颜色的变量
    public IEnumerator BlackSc()
    {
        ShowText.text = "";//文本清空
        float timer = 0f;
        tmpColor = image.color;
        do
        {
            timer += Time.deltaTime;
            SetColorAlpha(curve1.Evaluate(timer * speed));
            yield return null;
 
        } while (tmpColor.a < 1);

        StartCoroutine("ShowDialog"); //显示文本
        
        yield return new WaitForSeconds(0.5f);
        isMouseClicked = false;
        yield return StartCoroutine(WaitForMouseClick()); //等待鼠标点击

        ShowText.text = "";//文本清空
        timer = 0f;
        do
        {
            timer += Time.deltaTime;
            SetColorAlpha(curve2.Evaluate(timer * speed));
            yield return null;
 
        } while (tmpColor.a > 0); //黑屏消失

        gameObject.SetActive(false);
        if(dialoglogic.currentLine < dialoglogic.con_DialogData.dialoguedatas.Length)
        {
            dialoglogic.gameObject.SetActive(true);
            dialoglogic.playDialog();
        }
    }

    //通过调整图片的透明度实现渐入渐出
    void SetColorAlpha(float a)
    {
        tmpColor.a = a;
        image.color = tmpColor;
    }

    IEnumerator ShowDialog()
    {
        foreach (char c in dialoglogic.con_DialogData.dialoguedatas[dialoglogic.currentLinenow].Dialogue)//滚动输出字符
        {
            ShowText.text += c;
            yield return new WaitForSeconds(0.08f);
        }
    }

    IEnumerator WaitForMouseClick()
    {
        while (!isMouseClicked)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isMouseClicked = true;
            }

            yield return null;
        }
    }
}
