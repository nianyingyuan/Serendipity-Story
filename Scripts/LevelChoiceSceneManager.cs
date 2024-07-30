using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChoiceSceneManager : MonoBehaviour
{
    public void GoLevel0_1()
    {
        //清理对象池
        PoolManager.Instance.Clear();
        //播放音效
        Invoke("DoGoLevel0_1",0.3f);

    }

    private void DoGoLevel0_1() //包一层使用invoke，防止场景切换过快吞掉按钮音效
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
