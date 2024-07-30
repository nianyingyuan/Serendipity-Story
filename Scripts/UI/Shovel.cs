using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Shovel : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler //指针的点击，进入和退出
{
    private Transform shovelImg;
    //是否在使用取消部署
    private bool isShovel;

    public bool IsShovel {
        get { return isShovel; }
        set
        {
            isShovel = value;
            //需要取消部署
            if (isShovel)
            {
                
            }
            //把铲子放回去
            else
            {
                shovelImg.transform.position = transform.position; //由于我取消部署图标和背景transform一致，所以放回去的时候直接定义为背景位置
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.ButtonClick);
        if (IsShovel == false)
        {
            IsShovel = true;
        }
    }

    public void OnPointerEnter(PointerEventData eventData) //鼠标放上去的操作
    {
        shovelImg.transform.localScale = new Vector2(0.6f, 0.6f);
    }

    public void OnPointerExit(PointerEventData eventData) //鼠标移走的操作
    {
        shovelImg.transform.localScale = new Vector2(0.8f, 0.8f);
    }

    // Start is called before the first frame update
    void Start()
    {
        shovelImg = transform.Find("Image");
    }


    void Update()
    {
        //如果需要取消部署
        if (IsShovel)
        {
            shovelImg.position = Input.mousePosition;
            //如果在铲子使用过程中按下鼠标左键，判断是否要取消部署
            if (Input.GetMouseButtonDown(0))
            {
                Grid grid = GridManager.Instance.GetGridByMouse();
                //如果网格上没有干员，那么直接跳过所有逻辑
                if (grid.CurrOfficialBase == null) return;
                //有且鼠标距离网格内干员小于1.5米，进行取消部署
                if (Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition),grid.CurrOfficialBase.transform.position)<1.5f) //把鼠标坐标转化为世界坐标，再与网格内干员位置比较
                {
                    grid.CurrOfficialBase.DestroyGameobject();
                    grid.CurrOfficialBase.currGrid.CurrOfficialBase = null; //令当前网格为空
                    IsShovel = false;
                }

            }
            //如果在铲子使用过程中按下鼠标右键，那么就把取消部署图标放回去
            if (Input.GetMouseButtonDown(1))
            {
                IsShovel = false;
            }
        }
    }
}
