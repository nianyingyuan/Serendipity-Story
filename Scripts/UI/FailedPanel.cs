using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FailedPanel : MonoBehaviour
{
    private Image failedImage; //定义一个image

    private void Start()
    {
        failedImage = transform.Find("FailedImage").GetComponent<Image>(); //将failedimg赋予变量
        //开始时设置为不可见
        failedImage.gameObject.SetActive(false); 
        //初始化其透明度为0
        failedImage.color = new Color(1, 1, 1, 0);

    }

    public void MissionFailed()
    {
        //显示图片
        if(failedImage != null)
        {
            failedImage.gameObject.SetActive(true);
            StartCoroutine(PanelColorEF());
        }
        //让图片透明度慢慢变为1
    }

    IEnumerator PanelColorEF()
    {
        float a = 0;
        for(int i=0; i < 20; i++)
        {
            a += 0.05f;
            failedImage.color = new Color(1, 1, 1, a);
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(2f);
        DoBackLevelChoiceScene();
    }

    private void DoBackLevelChoiceScene()
    {
        EnemyManager.Instance.ClearAllEnemy();
        SceneManager.LoadScene("Level_Choice");
    }
}
