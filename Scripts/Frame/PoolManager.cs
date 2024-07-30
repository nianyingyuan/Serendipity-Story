using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    private static PoolManager instance;

    public static PoolManager Instance {
        get {
            // ��� instance ��δ����ʼ��������г�ʼ��
            if (instance == null)
            {
                // ����Ҫʱ���ҵ����������е� PoolManager ʵ��
                instance = FindObjectOfType<PoolManager>();
                // ��������в����� PoolManager ʵ�����򴴽�һ���µ� GameObject ���� PoolManager �����ӵ�����
                if (instance == null)
                {
                    GameObject poolManagerObj = new GameObject("PoolManager");
                    instance = poolManagerObj.AddComponent<PoolManager>();
                }
            }
            return instance;
        }
    }

    private GameObject poolObj;

    //key��Ԥ���壬value�Ǿ����object
    private Dictionary<GameObject, List<GameObject>> poolDataDic = new Dictionary<GameObject, List<GameObject>>();

    //��ȡ����
    public GameObject GetObj(GameObject prefeb)
    {
        GameObject obj = null;
        //�������������ֵ��������Ԥ�������Դ����������Դ��������0
        //�ж���û�и�Ŀ¼
        if (poolObj == null) poolObj = new GameObject("PoolObj");
        if (poolDataDic.ContainsKey(prefeb) && poolDataDic[prefeb].Count > 0)
        {
            //����list�еĵ�һ��
            obj = poolDataDic[prefeb][0];
            //��list���Ƴ���һ��
            poolDataDic[prefeb].RemoveAt(0);
        }
        //û��������Դ
        else
        {
            //ʵ����һ����Ȼ�󴫹�ȥ
            obj = GameObject.Instantiate(prefeb);
        }
        //����ȥ֮ǰ������ʾ
        obj.SetActive(true);
        //����û�и�����
        obj.transform.SetParent(null);
        return obj;
    }

    //������Ž������
    public void PushObj(GameObject prefeb,GameObject obj)
    {
        //�ж���û�и�Ŀ¼
        if(poolObj == null) poolObj = new GameObject("PoolObj");

        //�ж��ֵ�����û�����Ԥ���������
        if (poolDataDic.ContainsKey(prefeb))
        {
            //������Ž�ȥ
            poolDataDic[prefeb].Add(obj);
        }
        //�ֵ���û��
        else
        {
            //�������Ԥ����Ļ��������
            poolDataDic.Add(prefeb, new List<GameObject>() { obj });
        }
        //�����Ŀ¼��û�����Ԥ����������������
        if (poolObj.transform.Find(prefeb.name) == false)
        {
            new GameObject(prefeb.name).transform.SetParent(poolObj.transform); //����Ŀ¼�����ҷŵ���Ŀ¼poolobj����
        }
        //����
        obj.SetActive(false);
        //���ø�����
        obj.transform.SetParent(poolObj.transform.Find(prefeb.name));
    }

    //�����������
    public void Clear()
    {
        poolDataDic.Clear();
    }
}
