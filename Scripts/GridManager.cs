using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    private List<Vector2> pointList = new List<Vector2>();
    private List<Grid> GridList = new List<Grid>();
    void Start()
    {
        

    }

    private void Awake()
    {
        Instance = this;
        CreatGridsBasePointList();
    }
    private void Update()
    {
        
    }

    //把子类的position全部加入到PointList里面
    private void CreatGridsBasePointList()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform childTransform = transform.GetChild(i);
            Vector3 childPosition = childTransform.position;
            pointList.Add(childPosition);
            GridList.Add(new Grid(childPosition,childPosition, false)); //将childposition的游戏坐标，世界坐标，是否存在干员传入gridlist
        }
    }

    //通过鼠标获取坐标点
    public Vector2 GetGridPointByMouse()
    {
        return GetGridPointByWorldPos(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    public Vector2 GetGridPointByWorldPos(Vector2 wordlPos)
    {
        return GetGridByWorldPos(wordlPos).Position;
    }

    public Grid GetGridByWorldPos(Vector2 wordlPos) //此函数用于寻找距离目标位置最近的网格
    {
        float dis = 100000;
        Grid grid = null;
        for (int i = 0; i < GridList.Count; i++)
        {
            if (Vector2.Distance(wordlPos, GridList[i].Point) < dis)
            {
                dis = Vector2.Distance(wordlPos, GridList[i].Point);
                grid = GridList[i];
            }
        }
        return grid;
    }

    //通过y轴来寻找网格，从下往上，0开始
    public Grid GetGridByVerticalNum(int verticalNum)
    {
        for(int i=0; i < GridList.Count; i++)
        {
            Debug.Log(GridList[i].Point);
            if (GridList[i].Point.x > 8 && GridList[i].Point.x < 9 && Mathf.Abs(GridList[i].Point.y - verticalNum) < 0.1 )
            {
                return GridList[i];
            }
        }
        return null;
    }

    public Grid GetGridByMouse()
    {
        return GetGridByWorldPos(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }
}
