using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VictoryPanel : MonoBehaviour
{
    private Image victoryImage; //����һ��image
    private void Start()
    {
        victoryImage = transform.Find("VictoryImage").GetComponent<Image>(); //��victoryimg�������
        //��ʼʱ����Ϊ���ɼ�
        victoryImage.gameObject.SetActive(false);
        //��ʼ����λ��
        victoryImage.transform.position = new Vector3(-642.25f, 353.5f, 0);

    }

    public void MissionFinished()
    {
        //��ʾͼƬ
        if (victoryImage != null)
        {
            victoryImage.gameObject.SetActive(true);
            StartCoroutine(PanelColorEF());
        }
        //��ͼƬ�����м䣬ͣ��һ�����ƿ�
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
