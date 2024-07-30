using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    private static PoolManager instance;

    public static PoolManager Instance {
        get {
            // 如果 instance 尚未被初始化，则进行初始化
            if (instance == null)
            {
                // 在需要时，找到场景中现有的 PoolManager 实例
                instance = FindObjectOfType<PoolManager>();
                // 如果场景中不存在 PoolManager 实例，则创建一个新的 GameObject 并将 PoolManager 组件添加到其中
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

    //key是预制体，value是具体的object
    private Dictionary<GameObject, List<GameObject>> poolDataDic = new Dictionary<GameObject, List<GameObject>>();

    //获取物体
    public GameObject GetObj(GameObject prefeb)
    {
        GameObject obj = null;
        //如果缓存池数据字典中有这个预制体的资源并且这种资源数量大于0
        //判断有没有根目录
        if (poolObj == null) poolObj = new GameObject("PoolObj");
        if (poolDataDic.ContainsKey(prefeb) && poolDataDic[prefeb].Count > 0)
        {
            //返回list中的第一个
            obj = poolDataDic[prefeb][0];
            //从list中移除第一个
            poolDataDic[prefeb].RemoveAt(0);
        }
        //没有这种资源
        else
        {
            //实例化一个，然后传过去
            obj = GameObject.Instantiate(prefeb);
        }
        //传出去之前让他显示
        obj.SetActive(true);
        //让其没有父物体
        obj.transform.SetParent(null);
        return obj;
    }

    //把物体放进缓存池
    public void PushObj(GameObject prefeb,GameObject obj)
    {
        //判断有没有根目录
        if(poolObj == null) poolObj = new GameObject("PoolObj");

        //判断字典中有没有这个预制体的数据
        if (poolDataDic.ContainsKey(prefeb))
        {
            //把物体放进去
            poolDataDic[prefeb].Add(obj);
        }
        //字典中没有
        else
        {
            //创建这个预制体的缓存池数据
            poolDataDic.Add(prefeb, new List<GameObject>() { obj });
        }
        //如果根目录下没有这个预制体命名的子物体
        if (poolObj.transform.Find(prefeb.name) == false)
        {
            new GameObject(prefeb.name).transform.SetParent(poolObj.transform); //创建目录，并且放到根目录poolobj下面
        }
        //隐藏
        obj.SetActive(false);
        //设置父物体
        obj.transform.SetParent(poolObj.transform.Find(prefeb.name));
    }

    //清除所有数据
    public void Clear()
    {
        poolDataDic.Clear();
    }
}
