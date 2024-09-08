using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 
//黑屏，用一张全黑图片覆盖屏幕，调整透明度使用curve。
public class BlackScreen : MonoBehaviour
{
    public Image image;//覆盖屏幕的一张全黑图片
    public AnimationCurve curve; //在Inspector上调整自己喜欢的曲线
    public Dialoglogic dialoglogic;
    [Range(0.5f, 2f)]public float speed = 1f; //控制渐入渐出的速度
 
    private void Awake()
    {
        if (image == null) 
            image = GetComponent<Image>();
    }
 
    //开启自动播放黑屏
    private void OnEnable()
    {
        StartCoroutine(Black());
        
    }
 
    Color tmpColor; //用于传递颜色的变量
    public IEnumerator Black()
    {
        float timer = 0f;
        tmpColor = image.color;
        do
        {
            timer += Time.deltaTime;
            SetColorAlpha(curve.Evaluate(timer * speed));
            yield return null;
 
        } while (tmpColor.a > 0);
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
}