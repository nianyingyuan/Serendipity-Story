using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VictoryPanel : MonoBehaviour
{
    private Image victoryImage; //定义一个image
    private void Start()
    {
        victoryImage = transform.Find("VictoryImage").GetComponent<Image>(); //将victoryimg赋予变量
        //开始时设置为不可见
        victoryImage.gameObject.SetActive(false);
        //初始化其位置
        victoryImage.transform.position = new Vector3(-642.25f, 353.5f, 0);

    }

    public void MissionFinished()
    {
        //显示图片
        if (victoryImage != null)
        {
            victoryImage.gameObject.SetActive(true);
            StartCoroutine(PanelColorEF());
        }
        //让图片从左到中间，停顿一下再移开
    }

    IEnumerator PanelColorEF()
    {
        float startX = -642.25f;
        float endX = 635.75f;
        float moveDuration = 0.5f;

        // Move from startX to endX in 1 second
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / moveDuration;
            float newX = Mathf.Lerp(startX, endX, t);
            Vector3 newPosition = new Vector3(newX, 353.5f, 0);
            victoryImage.transform.position = newPosition;
            yield return null;
        }
        if(t >= 1f)
        {
            victoryImage.transform.position = new Vector3(635.75f, 353.5f, 0);
        }
        // Wait for 1 second
        yield return new WaitForSeconds(1f);
        DoBackLevelChoiceScene();
    }
    private void DoBackLevelChoiceScene()
    {
        EnemyManager.Instance.ClearAllEnemy();
        SceneManager.LoadScene("Level_Choice");
    }
}
