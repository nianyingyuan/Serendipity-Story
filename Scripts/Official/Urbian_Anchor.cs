using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Urbian_Anchor : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public EnemyBase targetEnemy; //定义目标敌人
    public Vector3 targetPosition; //目标敌人的位置

    //定义与目标的距离
    private float distanceX;

    //定义攻击力与攻击类型
    public float attackNum;

    //定义销毁时间
    private float destroyTimer;

    //定义所在行数，方面寻找目标
    public int lineNum;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        destroyTimer = 0f;
        attackNum = 240;
    }

    private void Update()
    {
        targetEnemy = EnemyManager.Instance.GetEnemyByLineMinDistance(lineNum, transform.position);
        destroyTimer += Time.deltaTime;
        if (destroyTimer >= 1)
        {
            Destroy(gameObject);
            return;
        }
        TransformUpdate();
    }

    private void TransformUpdate()
    {
        Vector3 currentPosition = transform.position;
        currentPosition.x += 1f;
        transform.position = currentPosition;
        if (Vector2.Distance(targetEnemy.transform.position, transform.position) < 1.5f)
        {
            AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.UrbianAnchorBoomMusic);
            int boomLineNum = targetEnemy.lineNum;
            int boomArrangeNum = targetEnemy.arrangeNum;
            GameObject boomPrefab = GameManager.Instance.GameConf.Anchor_Boom;
            GameObject boom = GameObject.Instantiate(boomPrefab, transform.position, Quaternion.identity, null);
            List<EnemyBase> enemies = EnemyManager.Instance.GetEnemiesAround(boomLineNum, boomArrangeNum);
            for(int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Hurt(attackNum, 0);
            }
            Destroy(gameObject);
        }
    }

}
