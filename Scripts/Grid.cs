using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid 
{
    public Vector3 Point; //��¼����������
    public Vector3 Position; //��¼�������������
    public bool HaveOfficial; //��¼�����Ƿ���ֲ�����У����ܴ����ڶ���ֲ��

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
