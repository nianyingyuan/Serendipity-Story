using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

//关卡状态
public enum LVState
{
    //开始游戏
    Start,
    //战斗中
    Fighting,
    //游戏失败
    Failed,
    //结束游戏
    Over
}

public class LVManager : MonoBehaviour
{
    public static LVManager Instance;
    private LVState currLVState;

    //定义当前击败的敌人数和敌人总数，以及蓝门生命值
    public int currEnemyNum;
    private int allEnemyNum;
    private int blueDoorHealth;

    //定义游戏是否结束
    private bool isOver;

    //定义要进入的关卡代码
    private int currLV;

    //定义布欧是否开始行动
    public bool buuMove;

    public int CurrLV
    {
        get => currLV;
        set
        {
            currLV = value;
            StartLV(currLV);
        }
    }

    //定义一个bool值来确定是否转换为fighting状态
    private bool startToFightingCalled = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "Level0_1")
        {
            CurrLV = 1;
        }
        else if(currentScene.name == "Ur_1")
        {
            CurrLV = 2;
        }
        else if (currentScene.name == "Kr_1")
        {
            CurrLV = 3;
        }
        else if(currentScene.name == "Start")
        {
            CurrLV = -1;
        }
        else if (currentScene.name == "Level_Choice")
        {
            CurrLV = 0;
        }

    }

    public void StartLV(int lv)
    {
        if (isOver) return;
        currLV = lv;
        currEnemyNum = 0;
        blueDoorHealth = 1;
        if (currLV == 1)
        {
            allEnemyNum = 14;
            TransitionToStart(); //这个函数已经封装好了从黑屏到战斗开始（cost开始增加）并且生成关卡的所有敌人的一系列操作
        }
        else if(currLV == 2)
        {
            allEnemyNum = 21;
            TransitionToStart(); //这个函数已经封装好了从黑屏到战斗开始（cost开始增加）并且生成关卡的所有敌人的一系列操作
        }
        else if(currLV == 3)
        {
            allEnemyNum = 26;
            TransitionToStart(); //这个函数已经封装好了从黑屏到战斗开始（cost开始增加）并且生成关卡的所有敌人的一系列操作
        }
        else if(currLV <=0 )
        {
            return;
        }
    }

    private void Update()
    {
        if(currEnemyNum == allEnemyNum && !isOver)
        {
            EnemyNumText.Instance.UpdateEnemyNumText(currEnemyNum, allEnemyNum);
            AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.MissionFinished);
            MissionFinished();
        }
        if (isOver) return;
        FSM();
    }

    private void FSM()
    {
        switch (currLVState)
        {
            case LVState.Start:
                break;
            case LVState.Fighting:
                //更新战斗信息
                EnemyNumText.Instance.UpdateEnemyNumText(currEnemyNum, allEnemyNum);
                EnemyWinText.Instance.UpdateEnemyWinText(blueDoorHealth);
                break;
            case LVState.Failed:
                break;
            case LVState.Over:
                break;
        }

    }

    private void TransitionToStart()
    {
        currLVState = LVState.Start;
        //隐藏UI面板
        MainPanelManager.Instance.SetMainPanelActive(false);
        //隐藏战斗信息面板
        LVInfoPanel.Instance.SetMainPanelActive(false);
        //黑屏开始显现,然后消失，然后调用LVStartToFightAction
        BlackScreen.Instance.StartAction(() =>
        {
            if (!startToFightingCalled)
            {
                startToFightingCalled = true;
                StartToFighting();
            }
        });
        GameSceneText.Instance.TextAction();
    }

        private void StartToFighting()
    {
        //让cost开始增加
        UIManager.Instance.IfProduce(true);
        currLVState = LVState.Fighting;
        //显示UI主面板
        MainPanelManager.Instance.SetMainPanelActive(true);
        //显示战斗信息面板
        LVInfoPanel.Instance.SetMainPanelActive(true);
        //开始生成敌人
        if(currLV == 1)
        {
            StartCoroutine(DoUpdateEnemyFor0_1());
        }
        else if(currLV == 2)
        {
            StartCoroutine(DoUpdateEnemyForUr_1());
        }
        else if (currLV == 3)
        {
            StartCoroutine(DoUpdateEnemyForKr_1());
        }

    }

    IEnumerator DoUpdateEnemyFor0_1()
    {
        //开始战斗后2s开始刷新敌人
        yield return new WaitForSeconds(5f);
        EnemyManager.Instance.UpdateEnemyFor0_1(1, 1);
        //然后隔7s继续刷新敌人
        yield return new WaitForSeconds(7f);
        EnemyManager.Instance.UpdateEnemyFor0_1(1, 1);
        EnemyManager.Instance.UpdateEnemyFor0_1(1, 2);
        yield return new WaitForSeconds(5f);
        EnemyManager.Instance.UpdateEnemyFor0_1(1, 1);
        EnemyManager.Instance.UpdateEnemyFor0_1(1, 2);
        yield return new WaitForSeconds(0.5f);
        EnemyManager.Instance.UpdateEnemyFor0_1(1, UnityEngine.Random.Range(1, 3));
        yield return new WaitForSeconds(7f);
        EnemyManager.Instance.UpdateEnemyFor0_1(1, UnityEngine.Random.Range(1, 3));
        EnemyManager.Instance.UpdateEnemyFor0_1(1, UnityEngine.Random.Range(1, 3));
        yield return new WaitForSeconds(0.5f);
        EnemyManager.Instance.UpdateEnemyFor0_1(1, UnityEngine.Random.Range(1, 3));
        EnemyManager.Instance.UpdateEnemyFor0_1(1, UnityEngine.Random.Range(1, 3));
        yield return new WaitForSeconds(7f);
        EnemyManager.Instance.UpdateEnemyFor0_1(1, UnityEngine.Random.Range(1, 3));
        EnemyManager.Instance.UpdateEnemyFor0_1(1, UnityEngine.Random.Range(1, 3));
        yield return new WaitForSeconds(0.5f);
        EnemyManager.Instance.UpdateEnemyFor0_1(1, UnityEngine.Random.Range(1, 3));
        EnemyManager.Instance.UpdateEnemyFor0_1(2, UnityEngine.Random.Range(1, 3));

    }

    IEnumerator DoUpdateEnemyForUr_1()
    {
        //开始战斗后2s开始刷新敌人
        yield return new WaitForSeconds(7f);
        int a = UnityEngine.Random.Range(0, 4);
        EnemyManager.Instance.UpdateEnemyForUr_1(1, a);
        yield return new WaitForSeconds(1f);
        EnemyManager.Instance.UpdateEnemyForUr_1(1, a);

        yield return new WaitForSeconds(10f);
        EnemyManager.Instance.UpdateEnemyForUr_1(1, UnityEngine.Random.Range(0, 4));
        EnemyManager.Instance.UpdateEnemyForUr_1(1, UnityEngine.Random.Range(0, 4));

        yield return new WaitForSeconds(14f);
        EnemyManager.Instance.UpdateEnemyForUr_1(3, UnityEngine.Random.Range(0, 4));
        yield return new WaitForSeconds(1f);
        EnemyManager.Instance.UpdateEnemyForUr_1(1, UnityEngine.Random.Range(0, 4));

        yield return new WaitForSeconds(3f);
        EnemyManager.Instance.UpdateEnemyForUr_1(2, UnityEngine.Random.Range(0, 4));
        EnemyManager.Instance.UpdateEnemyForUr_1(2, UnityEngine.Random.Range(0, 4));

        yield return new WaitForSeconds(3f);
        EnemyManager.Instance.UpdateEnemyForUr_1(2, UnityEngine.Random.Range(0, 4));
        EnemyManager.Instance.UpdateEnemyForUr_1(2, UnityEngine.Random.Range(0, 4));

        yield return new WaitForSeconds(3f);
        EnemyManager.Instance.UpdateEnemyForUr_1(2, UnityEngine.Random.Range(0, 4));
        EnemyManager.Instance.UpdateEnemyForUr_1(2, UnityEngine.Random.Range(0, 4));

        yield return new WaitForSeconds(15f);
        EnemyManager.Instance.UpdateEnemyForUr_1(3, 1);
        EnemyManager.Instance.UpdateEnemyForUr_1(3, 2);
        EnemyManager.Instance.UpdateEnemyForUr_1(3, 3);
        yield return new WaitForSeconds(6f);
        EnemyManager.Instance.UpdateEnemyForUr_1(3, 1);
        EnemyManager.Instance.UpdateEnemyForUr_1(3, 2);
        EnemyManager.Instance.UpdateEnemyForUr_1(3, 3);
        yield return new WaitForSeconds(6f);
        EnemyManager.Instance.UpdateEnemyForUr_1(3, 1);
        EnemyManager.Instance.UpdateEnemyForUr_1(3, 2);
        EnemyManager.Instance.UpdateEnemyForUr_1(3, 3);


    }

    IEnumerator DoUpdateEnemyForKr_1()
    {
        //开始战斗后
        yield return null;
        EnemyManager.Instance.UpdateBuuForKr_1();
        yield return new WaitForSeconds(30f);
        EnemyManager.Instance.UpdateEnemyForKr_1(3, 1);
        EnemyManager.Instance.UpdateEnemyForKr_1(3, 2);
        EnemyManager.Instance.UpdateEnemyForKr_1(3, 3);
        EnemyManager.Instance.UpdateEnemyForKr_1(3, 4);
        EnemyManager.Instance.UpdateEnemyForKr_1(3, 0);
        yield return new WaitForSeconds(5f);
        EnemyManager.Instance.UpdateEnemyForKr_1(3, 1);
        EnemyManager.Instance.UpdateEnemyForKr_1(3, 2);
        EnemyManager.Instance.UpdateEnemyForKr_1(3, 3);
        EnemyManager.Instance.UpdateEnemyForKr_1(3, 4);
        EnemyManager.Instance.UpdateEnemyForKr_1(3, 0);
        yield return new WaitForSeconds(5f);
        EnemyManager.Instance.UpdateEnemyForKr_1(3, 1);
        EnemyManager.Instance.UpdateEnemyForKr_1(3, 2);
        EnemyManager.Instance.UpdateEnemyForKr_1(3, 3);
        EnemyManager.Instance.UpdateEnemyForKr_1(3, 4);
        EnemyManager.Instance.UpdateEnemyForKr_1(3, 0);
        yield return new WaitForSeconds(5f);
        EnemyManager.Instance.UpdateEnemyForKr_1(3, 1);
        EnemyManager.Instance.UpdateEnemyForKr_1(3, 2);
        EnemyManager.Instance.UpdateEnemyForKr_1(3, 3);
        EnemyManager.Instance.UpdateEnemyForKr_1(3, 4);
        EnemyManager.Instance.UpdateEnemyForKr_1(3, 0);
        yield return new WaitForSeconds(5f);
        EnemyManager.Instance.UpdateEnemyForKr_1(3, 1);
        EnemyManager.Instance.UpdateEnemyForKr_1(3, 2);
        EnemyManager.Instance.UpdateEnemyForKr_1(3, 3);
        EnemyManager.Instance.UpdateEnemyForKr_1(3, 4);
        EnemyManager.Instance.UpdateEnemyForKr_1(3, 0);

    }

    private void StateUpdate() //此函数定义关卡结束后各种变量进行归位
    {
        currEnemyNum = 0;
        blueDoorHealth = 0;
        startToFightingCalled = false;
    }

    //任务失败
    public void MissionFailed()
    {
        StopAllCoroutines();
        isOver = true;
        //逻辑
        UIManager.Instance.IfProduce(false);
        //UI
        UIManager.Instance.MissionFailed();
    }

    //任务完成
    public void MissionFinished()
    {
        StopAllCoroutines();
        isOver = true;
        //逻辑
        UIManager.Instance.IfProduce(false);
        //UI
        UIManager.Instance.MissionFinished();
    }
}
