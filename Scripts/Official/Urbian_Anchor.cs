using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Urbian_Anchor : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public EnemyBase targetEnemy; //����Ŀ�����
    public Vector3 targetPosition; //Ŀ����˵�λ��

    //������Ŀ��ľ���
    private float distanceX;

    //���幥�����빥������
    public float attackNum;

    //��������ʱ��
    private float destroyTimer;

    //������������������Ѱ��Ŀ��
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
