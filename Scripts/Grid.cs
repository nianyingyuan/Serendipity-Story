using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid 
{
    public Vector3 Point; //记录网格的坐标点
    public Vector3 Position; //记录网格的世界坐标
    public bool HaveOfficial; //记录网格是否有植物，如果有，不能创建第二个植物

    private OfficialBase currOfficialBase;

    public Grid (Vector2 point, Vector2 position, bool haveOfficial)
    {
        Point = point;
        Position = position;
        HaveOfficial = haveOfficial;
    }

    public OfficialBase CurrOfficialBase { get => currOfficialBase;
    set
        {
            currOfficialBase = value;
            if(currOfficialBase == null)
            {
                HaveOfficial = false;
            }
            else
            {
                HaveOfficial = true;
            }
        }
    }
}
