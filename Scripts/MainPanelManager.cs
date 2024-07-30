using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPanelManager : MonoBehaviour
{
    public static MainPanelManager Instance;
    private GameObject mainPanel; //����mainPanel��Ϸ����

    private void Awake()
    {
        Instance = this;
        mainPanel = transform.gameObject;
    }

    public void SetMainPanelActive(bool isShow) //����MainPanel�Ƿ���ӻ������������Ϊtrue����Ϊ���ӻ������򲻿���
    {
        mainPanel.SetActive(isShow);
    }
}
