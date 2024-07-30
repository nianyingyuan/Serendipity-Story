using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; } //创建一个静态变量 instance，它的作用通常是用来实现单例模式,主要目的是确保一个类只有一个实例，并提供一个全局访问点来访问这个实例。

    //定义cost数字以及进度条，注意此时只是建立了一个空对象，随后要去unity进行绑定
    public TextMeshProUGUI costNumText; 
    public Image costBar;

    //定义cost计时器与cd
    private float costTimer = 0;
    private float costTime = 1;

    //设置cost是否开始生产
    private bool isStartProduce = false;

    //当前的干员卡片
    private UIOfficialCard currCard;
    public UIOfficialCard CurrCard { get => currCard;
        set
        {
            //置空上一个卡片的状态
            if (currCard != null)
            {
                currCard.WantPlace = false;
            }
            currCard = value;
        }
    }

    //定义暂停后的设置面板
    public SetPanel setPanel;

    //定义游戏失败与完成的组件
    public FailedPanel failedPanel;
    public VictoryPanel victoryPanel;
    

    private void Awake()
    {
        Instance = this; 
        //将根目录中的setpanel赋予这个变量,并初始化为不可见
        setPanel.Show(false);
    }

    // Update is called once per frame
    void Update()
    {
        CostNumTextUpdate();
        CostBarUpdate();
        if (isStartProduce == true)
        {
            ProduceCost();
        }
        PlayerManager.Instance.costMaxController();
    }

    private void CostNumTextUpdate() //此函数用于更新cost数字
    {
        if(costNumText != null)
        {
            costNumText.text = PlayerManager.Instance.costNum.ToString();
        }
    }

    private void CostBarUpdate() //此函数用于更新cost进度条
    {
        if(costBar != null)
        {
            costBar.fillAmount = costTimer / costTime;
        }
    }

    public void IfProduce(bool isorno) //此函数用于开始生产
    {
        isStartProduce = isorno;
    }

    private void ProduceCost() //此函数用于cost生产的具体操作
    {
        costTimer += Time.deltaTime;
        if(costTimer >= costTime)
        {
            PlayerManager.Instance.costNum += 1;
            costTimer = 0;
        }
    }

    public void SubCost(int point) //此函数用于消耗cost操作
    {
        PlayerManager.Instance.costNum -= point;
        CostNumTextUpdate();
    }
    public void AddCost(int point) //此函数用于增加cost操作
    {
        PlayerManager.Instance.costNum += point;
        CostNumTextUpdate();
    }

    public void ShowSetPanel()
    {
        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.ButtonClick);
        setPanel.Show(true);
    }

    public void MissionFailed()
    {
        if(failedPanel != null)
        {
            failedPanel.MissionFailed();
        }
    }

    public void MissionFinished()
    {
        if(victoryPanel != null)
        {
            victoryPanel.MissionFinished();
        }
    }
}
