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
            //将帧率锁定到60
            Application.targetFrameRate = 60;
            GameConf = Resources.Load<GameConf>("GameConf");
            //针对单例，由于只能存在一个单例，而后面可能有很多个gamemanager，所以每次开始的时候要删除自身以便再次创建
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
