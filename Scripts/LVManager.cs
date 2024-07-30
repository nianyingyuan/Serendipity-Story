using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

//�ؿ�״̬
public enum LVState
{
    //��ʼ��Ϸ
    Start,
    //ս����
    Fighting,
    //��Ϸʧ��
    Failed,
    //������Ϸ
    Over
}

public class LVManager : MonoBehaviour
{
    public static LVManager Instance;
    private LVState currLVState;

    //���嵱ǰ���ܵĵ������͵����������Լ���������ֵ
    public int currEnemyNum;
    private int allEnemyNum;
    private int blueDoorHealth;

    //������Ϸ�Ƿ����
    private bool isOver;

    //����Ҫ����Ĺؿ�����
    private int currLV;

    //���岼ŷ�Ƿ�ʼ�ж�
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

    //����һ��boolֵ��ȷ���Ƿ�ת��Ϊfighting״̬
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
            TransitionToStart(); //��������Ѿ���װ���˴Ӻ�����ս����ʼ��cost��ʼ���ӣ��������ɹؿ������е��˵�һϵ�в���
        }
        else if(currLV == 2)
        {
            allEnemyNum = 21;
            TransitionToStart(); //��������Ѿ���װ���˴Ӻ�����ս����ʼ��cost��ʼ���ӣ��������ɹؿ������е��˵�һϵ�в���
        }
        else if(currLV == 3)
        {
            allEnemyNum = 26;
            TransitionToStart(); //��������Ѿ���װ���˴Ӻ�����ս����ʼ��cost��ʼ���ӣ��������ɹؿ������е��˵�һϵ�в���
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
                //����ս����Ϣ
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
        //����UI���
        MainPanelManager.Instance.SetMainPanelActive(false);
        //����ս����Ϣ���
        LVInfoPanel.Instance.SetMainPanelActive(false);
        //������ʼ����,Ȼ����ʧ��Ȼ�����LVStartToFightAction
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
        //��cost��ʼ����
        UIManager.Instance.IfProduce(true);
        currLVState = LVState.Fighting;
        //��ʾUI�����
        MainPanelManager.Instance.SetMainPanelActive(true);
        //��ʾս����Ϣ���
        LVInfoPanel.Instance.SetMainPanelActive(true);
        //��ʼ���ɵ���
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
        //��ʼս����2s��ʼˢ�µ���
        yield return new WaitForSeconds(5f);
        EnemyManager.Instance.UpdateEnemyFor0_1(1, 1);
        //Ȼ���7s����ˢ�µ���
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
        //��ʼս����2s��ʼˢ�µ���
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
        //��ʼս����
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

    private void StateUpdate() //�˺�������ؿ���������ֱ������й�λ
    {
        currEnemyNum = 0;
        blueDoorHealth = 0;
        startToFightingCalled = false;
    }

    //����ʧ��
    public void MissionFailed()
    {
        StopAllCoroutines();
        isOver = true;
        //�߼�
        UIManager.Instance.IfProduce(false);
        //UI
        UIManager.Instance.MissionFailed();
    }

    //�������
    public void MissionFinished()
    {
        StopAllCoroutines();
        isOver = true;
        //�߼�
        UIManager.Instance.IfProduce(false);
        //UI
        UIManager.Instance.MissionFinished();
    }
}
