using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChoiceSceneManager : MonoBehaviour
{
    public void GoLevel0_1()
    {
        //��������
        PoolManager.Instance.Clear();
        //������Ч
        Invoke("DoGoLevel0_1",0.3f);

    }

    private void DoGoLevel0_1() //��һ��ʹ��invoke����ֹ�����л������̵���ť��Ч
    {
        SceneManager.LoadScene("Level0_1");
    }

    public void GoStartScene()
    {
        SceneManager.LoadScene("Start");
    }

    public void DoGoLevelUr_1()
    {
        SceneManager.LoadScene("Ur_1");
    }
    public void DoGoLevelKr_1()
    {
        SceneManager.LoadScene("Kr_1");
    }
}
