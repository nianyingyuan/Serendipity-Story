using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPanelManager : MonoBehaviour
{
    public static MainPanelManager Instance;
    private GameObject mainPanel; //定义mainPanel游戏物体

    private void Awake()
    {
        Instance = this;
        mainPanel = transform.gameObject;
    }

    public void SetMainPanelActive(bool isShow) //设置MainPanel是否可视化，如果括号里为true，则为可视化，否则不可视
    {
        mainPanel.SetActive(isShow);
    }
}
