using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //引入UI组件
using UnityEngine.EventSystems;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting; //引入世界系统

public enum CardState //卡片的四种状态
{
    //有cost不在CD
    CanPlace,
    //有cost在CD
    InCD,
    //没有cost不在CD
    NotCost,
    //没有cost在CD
    NotAll
}

public class UIOfficialCard : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler //引入鼠标放在图标上的接口与点击接口
{
    //遮罩图片的Img获取
    private Image maskImg;

    //自身的Img组件
    private Image image;

    //种植需要多少阳光
    public int needCostNum;

    //冷却时间
    public float CDTime;

    //当前时间，用于冷却时间的计算
    private float currTimeForCd;

    //是否可以放置干员(只考虑CD)
    private bool canPlace;

    //是否需要放置干员
    private bool wantPlace;

    //用来创建干员
    private OfficialBase official;

    //用来创建鼠标放在网格里面时的透明干员
    private OfficialBase officialInGrid;

    //当前卡片对应的干员类型
    public OfficialType CardOfficialType;

    //卡片状态
    private CardState cardState;

    //干员的预制体
    private GameObject prefeb;
    public CardState CardState { get => cardState;
        set
        {

        }
    
    }

    public bool CanPlace { get => canPlace; set
        {
            canPlace = value;
            CheckState();
        }
    }

    public bool WantPlace
    {
        get => wantPlace;
        set
        {
            wantPlace = value;
        }
    }



    void Start()
    {
        maskImg = transform.Find("Mask").GetComponent<Image>();
        image = GetComponent<Image>();
        CanPlace = true;
        PlayerManager.Instance.AddCostNumUpdateActionListener(CheckState);
    }

    private void Update()
    {
        //如果需要放置干员且放置的干员不为空
        CheckState();
        if(WantPlace==true && official != null)
        {
            //让植物跟随我们的鼠标
            Vector3 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition); //获得鼠标位置
            Grid grid = GridManager.Instance.GetGridByWorldPos(mousePoint); //转换为网格位置
            official.transform.position = new Vector3 (mousePoint.x,mousePoint.y,0);
            //如果距离网格比较近并且没有植物在网格中，需要在网格上出现一个透明的干员
            if (grid.HaveOfficial == false && Vector2.Distance(mousePoint, grid.Position) < 1)
            {
                if(officialInGrid == null)
                {
                    //实例化official，位置是鼠标位置，不旋转
                    officialInGrid = GameObject.Instantiate<GameObject>(official.gameObject, grid.Position, Quaternion.identity, PlayerManager.Instance.poolObj.transform).GetComponent<OfficialBase>();
                    officialInGrid.InitForCreate(true);
                }
                else
                {
                    officialInGrid.transform.position = grid.Position;
                }
                //点击时放置植物
                if (Input.GetMouseButtonDown(0))
                {
                    official.TransitionToEnable(grid);
                    official = null;
                    Destroy(officialInGrid.gameObject);
                    officialInGrid = null;
                    wantPlace = false;
                    canPlace = false;
                    CanPlaceCheck();

                    UIManager.Instance.CurrCard = null;

                    //种植成功，消耗cost
                    PlayerManager.Instance.CostNum -= needCostNum;
                }
            }
            //如果距离太远就销毁
            else
            {
                if (officialInGrid != null)
                {
                    Destroy(officialInGrid.gameObject);
                    officialInGrid = null;
                }

            }
        }
        //如果右键，取消放置状态
        if (Input.GetMouseButtonDown(1))
        {
            if (official != null) Destroy(official.gameObject);
            if (official != null) Destroy(officialInGrid.gameObject);
            official = null;
            officialInGrid = null;
            WantPlace = false;

            UIManager.Instance.CurrCard = null;
        }
    }

    private void CheckState() //此函数用于状态检测
    {
        //有cost且不在CD
        if (canPlace && PlayerManager.Instance.CostNum >= needCostNum)
        {
            CardState = CardState.CanPlace;
            maskImg.fillAmount = 0;
            image.color = Color.white;
        }
        //有cost但是在CD
        else if (!canPlace && PlayerManager.Instance.CostNum >= needCostNum)
        {
            CardState = CardState.InCD;
            image.color = new Color(0.75f, 0.75f, 0.75f);
        }
        //没有cost但是不在CD
        else if (canPlace && PlayerManager.Instance.CostNum < needCostNum)
        {
            CardState = CardState.NotCost;
            maskImg.fillAmount = 0;
            image.color = new Color(0.75f, 0.75f, 0.75f);
        }
        //没有cost并且在CD
        else if (!canPlace && PlayerManager.Instance.CostNum < needCostNum)
        {
            CardState = CardState.NotAll;
            image.color = new Color(0.75f, 0.75f, 0.75f);
        }
    }

    private void CDEnter() //此函数定义进入cd操作
    {
        //重置fillamount
        maskImg.fillAmount = 1;
        //遮罩后开始计算冷却
        StartCoroutine(CalCD());
    }

    

    IEnumerator CalCD() //携程函数计算冷却时间
    {
        float calCD = (1 / CDTime) * 0.01f; //要乘以下方的0.1f
        currTimeForCd = CDTime;
        while (currTimeForCd>=0)
        {
            yield return new WaitForSeconds(0.01f); //这里定义调用函数的时间间隔
            maskImg.fillAmount -= calCD;
            currTimeForCd -= 0.01f;
        }
        //到这里意味着冷却结束，可以放置
        CanPlace = true;

    }
    public void OnPointerEnter(PointerEventData eventData) //初始化IPEnter，定义鼠标移入动作
    {
        if (!canPlace) return;
        Vector2 newScale = transform.localScale;
        newScale.x *= 1.05f;
        newScale.y *= 1.05f;
        transform.localScale = newScale;
    }

    public void OnPointerExit(PointerEventData eventData) //初始化IPExit，定义鼠标移出动作
    {
        if (!canPlace) return;
        Vector2 newScale = transform.localScale;
        newScale.x /= 1.05f;
        newScale.y /= 1.05f;
        transform.localScale = newScale;
    }

    //鼠标点击时的效果
    public void OnPointerClick(PointerEventData eventData) //初始化IPClick，定义鼠标点击动作
    {
        if ((!CanPlace) || (PlayerManager.Instance.CostNum < needCostNum)) return;
        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.ButtonClick);
        if (!WantPlace)
        {
            wantPlace = true;
            prefeb = OfficialManager.Instance.GetOfficialForType(CardOfficialType);
            official = GameObject.Instantiate<GameObject>(prefeb, Vector3.zero, Quaternion.identity, PlayerManager.Instance.poolObj.transform).GetComponent<OfficialBase>();//将干员放在地面上
            official.InitForCreate(false);
            UIManager.Instance.CurrCard = this;
        }
    }
    public void CanPlaceCheck() //此函数用于检测canplace的状态,canplace每改变一次就调用一次
    {
        //如果不能放置，开始冷却
        if (!canPlace)
        {
            //完全遮罩，表示不可点击
            maskImg.fillAmount = 1;
            //开始冷却
            CDEnter();
        }
        else
        {
            maskImg.fillAmount = 0;
        }
    }
}
