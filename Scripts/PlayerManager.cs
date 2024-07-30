using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    //定义cost数量，设置其初始为8
    public int costNum;
    //阳光数量更新时的事件
    private UnityAction CostNumUpdateAction;

    public GameObject poolObj;
    public int CostNum
    {
        get => costNum;
        set
        {
            costNum = value;
            CostNumUpdateAction();
        }
    }

    private void Awake()
    {
        Instance = this;
        costNum = 8;
    }

    //添加阳光数量更新时的事件监听
    public void AddCostNumUpdateActionListener(UnityAction action)
    {
        CostNumUpdateAction += action;
    }

    //设置cost的最大值为99
    public void costMaxController()
    {
        if(costNum > 99)
        {
            costNum = 99;
        }
    }
}
