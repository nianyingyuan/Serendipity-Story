using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LVInfoPanel : MonoBehaviour
{
    public static LVInfoPanel Instance;
    //定义游戏物体
    private GameObject infoPanel;

    private void Awake()
    {
        Instance = this;
        infoPanel = transform.gameObject;
    }

    public void SetMainPanelActive(bool isShow) //设置MainPanel是否可视化，如果括号里为true，则为可视化，否则不可视
    {
        infoPanel.SetActive(isShow);
    }
}
