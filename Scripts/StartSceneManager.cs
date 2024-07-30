using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    public void GoLevelChoice()
    {
        //��������
        PoolManager.Instance.Clear();
        //������Ч
        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.ButtonClick);
        //�л�����
        SceneManager.LoadScene("Level_Choice");
    }

    public void Quit()
    {
        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.ButtonClick);
        Application.Quit();
    }
}
