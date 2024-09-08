using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogScene : MonoBehaviour
{
    public Text ShowText; //显示文本

    private void OnEnable()
    {
        StartCoroutine(startscene());
        
    }

    public IEnumerator startscene()
    {
        
        yield return new WaitForSeconds(3f);  //等待一秒后跳转
        gameObject.SetActive(false);
    }
}
