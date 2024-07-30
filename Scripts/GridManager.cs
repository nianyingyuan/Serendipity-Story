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

    //�������positionȫ�����뵽PointList����
    private void CreatGridsBasePointList()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform childTransform = transform.GetChild(i);
            Vector3 childPosition = childTransform.position;
            pointList.Add(childPosition);
            GridList.Add(new Grid(childPosition,childPosition, false)); //��childposition����Ϸ���꣬�������꣬�Ƿ���ڸ�Ա����gridlist
        }
    }

    //ͨ������ȡ�����
    public Vector2 GetGridPointByMouse()
    {
        return GetGridPointByWorldPos(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    public Vector2 GetGridPointByWorldPos(Vector2 wordlPos)
    {
        return GetGridByWorldPos(wordlPos).Position;
    }

    public Grid GetGridByWorldPos(Vector2 wordlPos) //�˺�������Ѱ�Ҿ���Ŀ��λ�����������
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

    //ͨ��y����Ѱ�����񣬴������ϣ�0��ʼ
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
