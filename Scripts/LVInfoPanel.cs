using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LVInfoPanel : MonoBehaviour
{
    public static LVInfoPanel Instance;
    //������Ϸ����
    private GameObject infoPanel;

    private void Awake()
    {
        Instance = this;
        infoPanel = transform.gameObject;
    }

    public void SetMainPanelActive(bool isShow) //����MainPanel�Ƿ���ӻ������������Ϊtrue����Ϊ���ӻ������򲻿���
    {
        infoPanel.SetActive(isShow);
    }
}
