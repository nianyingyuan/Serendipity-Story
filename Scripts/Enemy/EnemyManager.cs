using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using System;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;
    public List<EnemyBase> enemies = new List<EnemyBase>();
    public GameObject poolObj;
    private void Awake()
    {
        Instance = this;
    }

    public void UpdateEnemyFor0_1(int enemyCode,int lineNum)
    {
        if(enemyCode == 1)
        {
            CreateYuanShiChongFor0_1(lineNum);
        }
        else if (enemyCode == 2)
        {
            CreateShiBingFor0_1(lineNum);
        }
    }

    public void UpdateEnemyForUr_1(int enemyCode,int lineNum)
    {
        if (enemyCode == 1)
        {
            CreateYuanShiChongForUr_1(lineNum);
        }
        else if (enemyCode == 2)
        {
            CreateShiBingForUr_1(lineNum);
        }
        else if (enemyCode == 3)
        {
            CreateZhongZhuangForUr_1(lineNum);
        }
    }

    public void UpdateEnemyForKr_1(int enemyCode,int lineNum)
    {
        if (enemyCode == 1)
        {
            CreateYuanShiChongForKr_1(lineNum);
        }
        else if (enemyCode == 2)
        {
            CreateShiBingForKr_1(lineNum);
        }else if(enemyCode == 3)
        {
            CreateZhongZhuangForKr_1(lineNum);
        }
    }

    public void UpdateBuuForKr_1()
    {
        GameObject.Instantiate<GameObject>(GameManager.Instance.GameConf.Buu, new Vector3(4.22f, 0.9f, 0), Quaternion.identity, poolObj.transform);
    }

    private void CreateYuanShiChongFor0_1(int lineNum)
    {
        if(lineNum == 1)
        {
            GameObject.Instantiate<GameObject>(GameManager.Instance.GameConf.YuanShiChong,new Vector3(5.6f,0.4f,0), Quaternion.identity, poolObj.transform);
        }
        else if (lineNum == 2)
        {
            GameObject.Instantiate<GameObject>(GameManager.Instance.GameConf.YuanShiChong, new Vector3(5.4f, 1.4f, 0), Quaternion.identity, poolObj.transform);
        }
    }

    private void CreateYuanShiChongForUr_1(int lineNum)
    {
        if (lineNum == 0)
        {
            GameObject.Instantiate<GameObject>(GameManager.Instance.GameConf.YuanShiChong, new Vector3(7.6f, -0.9f, 0), Quaternion.identity, poolObj.transform);
        }
        else if (lineNum == 1)
        {
            GameObject.Instantiate<GameObject>(GameManager.Instance.GameConf.YuanShiChong, new Vector3(7.5f, 0.6f, 0), Quaternion.identity, poolObj.transform);
        }
        else if (lineNum == 2)
        {
            GameObject.Instantiate<GameObject>(GameManager.Instance.GameConf.YuanShiChong, new Vector3(7.2f, 1.9f, 0), Quaternion.identity, poolObj.transform);
        }
        else if (lineNum == 3)
        {
            GameObject.Instantiate<GameObject>(GameManager.Instance.GameConf.YuanShiChong, new Vector3(7f, 3.1f, 0), Quaternion.identity, poolObj.transform);
        }
    }

    private void CreateShiBingFor0_1(int lineNum)
    {
        if (lineNum == 1)
        {
            GameObject.Instantiate<GameObject>(GameManager.Instance.GameConf.ShiBing, new Vector3(5.6f, 0.4f, 0), Quaternion.identity, poolObj.transform);
        }
        else if (lineNum == 2)
        {
            GameObject.Instantiate<GameObject>(GameManager.Instance.GameConf.ShiBing, new Vector3(5.4f, 1.4f, 0), Quaternion.identity, poolObj.transform);
        }
    }

    private void CreateShiBingForUr_1(int lineNum)
    {
        if (lineNum == 0)
        {
            GameObject.Instantiate<GameObject>(GameManager.Instance.GameConf.ShiBing, new Vector3(7.6f, -0.9f, 0), Quaternion.identity, poolObj.transform);
        }
        else if (lineNum == 1)
        {
            GameObject.Instantiate<GameObject>(GameManager.Instance.GameConf.ShiBing, new Vector3(7.5f, 0.6f, 0), Quaternion.identity, poolObj.transform);
        }
        else if (lineNum == 2)
        {
            GameObject.Instantiate<GameObject>(GameManager.Instance.GameConf.ShiBing, new Vector3(7.2f, 1.9f, 0), Quaternion.identity, poolObj.transform);
        }
        else if (lineNum == 3)
        {
            GameObject.Instantiate<GameObject>(GameManager.Instance.GameConf.ShiBing, new Vector3(7f, 3.1f, 0), Quaternion.identity, poolObj.transform);
        }
    }

    private void CreateZhongZhuangForUr_1(int lineNum)
    {
        if (lineNum == 0)
        {
            GameObject.Instantiate<GameObject>(GameManager.Instance.GameConf.ZhongZhuang, new Vector3(7.6f, -0.9f, 0), Quaternion.identity, poolObj.transform);
        }
        else if (lineNum == 1)
        {
            GameObject.Instantiate<GameObject>(GameManager.Instance.GameConf.ZhongZhuang, new Vector3(7.5f, 0.6f, 0), Quaternion.identity, poolObj.transform);
        }
        else if (lineNum == 2)
        {
            GameObject.Instantiate<GameObject>(GameManager.Instance.GameConf.ZhongZhuang, new Vector3(7.2f, 1.9f, 0), Quaternion.identity, poolObj.transform);
        }
        else if (lineNum == 3)
        {
            GameObject.Instantiate<GameObject>(GameManager.Instance.GameConf.ZhongZhuang, new Vector3(7f, 3.1f, 0), Quaternion.identity, poolObj.transform);
        }
    }

    private void CreateYuanShiChongForKr_1(int lineNum)
    {
        if (lineNum == 0)
        {
            GameObject.Instantiate<GameObject>(GameManager.Instance.GameConf.ShiBing, new Vector3(9.73f, -1.4f, 0), Quaternion.identity, poolObj.transform);
        }
        else if (lineNum == 1)
        {
            GameObject.Instantiate<GameObject>(GameManager.Instance.GameConf.ShiBing, new Vector3(9.4f, -0.1f, 0), Quaternion.identity, poolObj.transform);
        }
        else if (lineNum == 2)
        {
            GameObject.Instantiate<GameObject>(GameManager.Instance.GameConf.ShiBing, new Vector3(9.04f, 1.04f, 0), Quaternion.identity, poolObj.transform);
        }
        else if (lineNum == 3)
        {
            GameObject.Instantiate<GameObject>(GameManager.Instance.GameConf.ShiBing, new Vector3(8.72f, 2.25f, 0), Quaternion.identity, poolObj.transform);
        }
        else if (lineNum == 4)
        {
            GameObject.Instantiate<GameObject>(GameManager.Instance.GameConf.ShiBing, new Vector3(8.52f, 3.27f, 0), Quaternion.identity, poolObj.transform);
        }
    }

    private void CreateShiBingForKr_1(int lineNum)
    {
        if (lineNum == 0)
        {
            GameObject.Instantiate<GameObject>(GameManager.Instance.GameConf.ShiBing, new Vector3(9.73f, -1.4f, 0), Quaternion.identity, poolObj.transform);
        }
        else if (lineNum == 1)
        {
            GameObject.Instantiate<GameObject>(GameManager.Instance.GameConf.ShiBing, new Vector3(9.4f, -0.1f, 0), Quaternion.identity, poolObj.transform);
        }
        else if (lineNum == 2)
        {
            GameObject.Instantiate<GameObject>(GameManager.Instance.GameConf.ShiBing, new Vector3(9.04f, 1.04f, 0), Quaternion.identity, poolObj.transform);
        }
        else if (lineNum == 3)
        {
            GameObject.Instantiate<GameObject>(GameManager.Instance.GameConf.ShiBing, new Vector3(8.72f, 2.25f, 0), Quaternion.identity, poolObj.transform);
        }
        else if (lineNum == 4)
        {
            GameObject.Instantiate<GameObject>(GameManager.Instance.GameConf.ShiBing, new Vector3(8.52f, 3.27f, 0), Quaternion.identity, poolObj.transform);
        }
    }

    private void CreateZhongZhuangForKr_1(int lineNum)
    {
        if (lineNum == 0)
        {
            GameObject.Instantiate<GameObject>(GameManager.Instance.GameConf.ZhongZhuang, new Vector3(9.73f, -1.4f, 0), Quaternion.identity, poolObj.transform);
        }
        else if (lineNum == 1)
        {
            GameObject.Instantiate<GameObject>(GameManager.Instance.GameConf.ZhongZhuang, new Vector3(9.4f, -0.1f, 0), Quaternion.identity, poolObj.transform);
        }
        else if (lineNum == 2)
        {
            GameObject.Instantiate<GameObject>(GameManager.Instance.GameConf.ZhongZhuang, new Vector3(9.04f, 1.04f, 0), Quaternion.identity, poolObj.transform);
        }
        else if (lineNum == 3)
        {
            GameObject.Instantiate<GameObject>(GameManager.Instance.GameConf.ZhongZhuang, new Vector3(8.72f, 2.25f, 0), Quaternion.identity, poolObj.transform);
        }
        else if (lineNum == 4)
        {
            GameObject.Instantiate<GameObject>(GameManager.Instance.GameConf.ZhongZhuang, new Vector3(8.52f, 3.27f, 0), Quaternion.identity, poolObj.transform);
        }
    }

    public void AddEnemy(EnemyBase enemy)
    {
        enemies.Add(enemy);
    }

    public void RemoveEnemy(EnemyBase enemy)
    {
        enemies.Remove(enemy);
    }

    public void ClearAllEnemy()
    {
        while (enemies.Count > 0)
        {
            enemies[0].Dead();
        }
    }

    public EnemyBase GetEnemyByLineMinDistance(int lineNum,Vector3 pos) //此函数用于获取一个在同一行且距离最近的敌人
    {
        EnemyBase enemy = null;
        float dis = 10000;
        for(int i = 0;i < enemies.Count; i++)
        {
            if (enemies[i].lineNum == lineNum && enemies[i].transform.position.x > pos.x && Vector2.Distance(pos, enemies[i].transform.position)<dis)
            {
                dis = Vector2.Distance(pos, enemies[i].transform.position);
                enemy = enemies[i];
            }
        }
        return enemy;
    }

    public EnemyBase GetEnemyInTheFront(int lineNum , int arrangeNum, Vector3 pos) //此函数用于获取一个在自身或者自身前一格的敌人
    {
        EnemyBase enemy = null;
        float dis = 10000;
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].lineNum == lineNum && enemies[i].transform.position.x > pos.x && Vector2.Distance(pos, enemies[i].transform.position) < dis && enemies[i].arrangeNum == arrangeNum+1)
            {
                dis = Vector2.Distance(pos, enemies[i].transform.position);
                enemy = enemies[i];
            }else if(enemies[i].lineNum == lineNum && enemies[i].transform.position.x > pos.x && Vector2.Distance(pos, enemies[i].transform.position) < dis && enemies[i].arrangeNum == arrangeNum)
            {
                dis = Vector2.Distance(pos, enemies[i].transform.position);
                enemy = enemies[i];
            }
        }
        return enemy;
    }

    public List<EnemyBase> GetEnemiesInTheFront(int lineNum , int arrangeNum) //此函数用于获取所有在自身或者自身前一格的敌人
    {
        List<EnemyBase> enemieslist = new List<EnemyBase>();
        for(int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].lineNum == lineNum && enemies[i].arrangeNum == arrangeNum)
            {
                enemieslist.Add(enemies[i]);
            }
            else if(enemies[i].lineNum == lineNum && enemies[i].arrangeNum == arrangeNum + 1)
            {
                enemieslist.Add(enemies[i]);
            }
        }
        return enemieslist;
    }

    public List<EnemyBase> GetEnemiesAround(int lineNum, int arrangeNum) //此函数用于获取所有在自身周围3*3范围的敌人
    {
        List<EnemyBase> enemieslist = new List<EnemyBase>();
        for (int i = 0; i < enemies.Count; i++)
        {
            if (Math.Abs(enemies[i].lineNum - lineNum) <= 1 && Math.Abs(enemies[i].arrangeNum - arrangeNum) <= 1)
            {
                enemieslist.Add(enemies[i]);
            }
        }
        return enemieslist;
    }

    public void StopAllEnemies()
    {
        for(int i = 0; i < enemies.Count; i++)
        {
            enemies[i].speed = 0;
        }
    }

    public List<EnemyBase> GetEnemiesInLine(int lineNum,int arrangeNum , Vector3 pos) //此函数用于获取所有在自身或者自身前四格的敌人
    {
        List<EnemyBase> enemieslist = new List<EnemyBase>();
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].lineNum == lineNum && enemies[i].transform.position.x > pos.x && enemies[i].arrangeNum-arrangeNum < 5)
            {
                enemieslist.Add(enemies[i]);
            }
        }
        return enemieslist;
    }

}
