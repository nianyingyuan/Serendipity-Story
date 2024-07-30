using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FailedPanel : MonoBehaviour
{
    private Image failedImage; //����һ��image

    private void Start()
    {
        failedImage = transform.Find("FailedImage").GetComponent<Image>(); //��failedimg�������
        //��ʼʱ����Ϊ���ɼ�
        failedImage.gameObject.SetActive(false); 
        //��ʼ����͸����Ϊ0
        failedImage.color = new Color(1, 1, 1, 0);

    }

    public void MissionFailed()
    {
        //��ʾͼƬ
        if(failedImage != null)
        {
            failedImage.gameObject.SetActive(true);
            StartCoroutine(PanelColorEF());
        }
        //��ͼƬ͸����������Ϊ1
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
