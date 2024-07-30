using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; } //����һ����̬���� instance����������ͨ��������ʵ�ֵ���ģʽ,��ҪĿ����ȷ��һ����ֻ��һ��ʵ�������ṩһ��ȫ�ַ��ʵ����������ʵ����

    //����cost�����Լ���������ע���ʱֻ�ǽ�����һ���ն������Ҫȥunity���а�
    public TextMeshProUGUI costNumText; 
    public Image costBar;

    //����cost��ʱ����cd
    private float costTimer = 0;
    private float costTime = 1;

    //����cost�Ƿ�ʼ����
    private bool isStartProduce = false;

    //��ǰ�ĸ�Ա��Ƭ
    private UIOfficialCard currCard;
    public UIOfficialCard CurrCard { get => currCard;
        set
        {
            //�ÿ���һ����Ƭ��״̬
            if (currCard != null)
            {
                currCard.WantPlace = false;
            }
            currCard = value;
        }
    }

    //������ͣ����������
    public SetPanel setPanel;

    //������Ϸʧ������ɵ����
    public FailedPanel failedPanel;
    public VictoryPanel victoryPanel;
    

    private void Awake()
    {
        Instance = this; 
        //����Ŀ¼�е�setpanel�����������,����ʼ��Ϊ���ɼ�
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

    private void CostNumTextUpdate() //�˺������ڸ���cost����
    {
        if(costNumText != null)
        {
            costNumText.text = PlayerManager.Instance.costNum.ToString();
        }
    }

    private void CostBarUpdate() //�˺������ڸ���cost������
    {
        if(costBar != null)
        {
            costBar.fillAmount = costTimer / costTime;
        }
    }

    public void IfProduce(bool isorno) //�˺������ڿ�ʼ����
    {
        isStartProduce = isorno;
    }

    private void ProduceCost() //�˺�������cost�����ľ������
    {
        costTimer += Time.deltaTime;
        if(costTimer >= costTime)
        {
            PlayerManager.Instance.costNum += 1;
            costTimer = 0;
        }
    }

    public void SubCost(int point) //�˺�����������cost����
    {
        PlayerManager.Instance.costNum -= point;
        CostNumTextUpdate();
    }
    public void AddCost(int point) //�˺�����������cost����
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
