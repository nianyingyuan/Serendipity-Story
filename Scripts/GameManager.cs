using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameConf GameConf;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            //��֡��������60
            Application.targetFrameRate = 60;
            GameConf = Resources.Load<GameConf>("GameConf");
            //��Ե���������ֻ�ܴ���һ������������������кܶ��gamemanager������ÿ�ο�ʼ��ʱ��Ҫɾ�������Ա��ٴδ���
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
