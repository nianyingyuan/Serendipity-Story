using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    public void GoLevelChoice()
    {
        //清理对象池
        PoolManager.Instance.Clear();
        //播放音效
        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.ButtonClick);
        //切换场景
        SceneManager.LoadScene("Level_Choice");
    }

    public void Quit()
    {
        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.ButtonClick);
        Application.Quit();
    }
}
