using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    //����cost�������������ʼΪ8
    public int costNum;
    //������������ʱ���¼�
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

    //���������������ʱ���¼�����
    public void AddCostNumUpdateActionListener(UnityAction action)
    {
        CostNumUpdateAction += action;
    }

    //����cost�����ֵΪ99
    public void costMaxController()
    {
        if(costNum > 99)
        {
            costNum = 99;
        }
    }
}
