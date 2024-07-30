using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeLuoSi_Bullet : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public EnemyBase targetEnemy; //����Ŀ�����
    public Vector3 targetPosition; //Ŀ����˵�λ��

    //������Ŀ��ľ���
    private float distanceX;
    private float distanceY;

    //���幥�����빥������
    public float attackNum;

    //��������ʱ��
    private float destroyTimer;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        targetPosition = targetEnemy.transform.position;
        distanceX = targetPosition.x - transform.position.x - 2;
        distanceY = targetPosition.y - transform.position.y;
        transform.rotation = Quaternion.Euler(-70f, 0f, 0f);
        destroyTimer = 0f;
    }

    private void Update()
    {
        destroyTimer += Time.deltaTime;
        RotationUpdate();
        TransformUpdate();
    }

    private void RotationUpdate()
    {
        Vector3 currentRotation = transform.rotation.eulerAngles;
        currentRotation.x -= 0.01f;
        transform.rotation = Quaternion.Euler(currentRotation);
    }

    private void TransformUpdate()
    {
        Vector3 currentPosition = transform.position;
        currentPosition.x += distanceX / 20;
        currentPosition.y += distanceY / 20;
        transform.position = currentPosition;
        if(destroyTimer > 0.2f ) //�����߾������0.5fʱ��Ϊ����Ŀ��
        {
            if(targetEnemy.hp > 0)
            {
                targetEnemy.Hurt(attackNum, 0);
            }
            Destroy();
        }
    }

    private void Destroy()
    {
        //ȡ������ȫ��Э�̺��ӳٵ���,���û�оͲ���д
       
        //�Ž�����أ�������ʵ����
        PoolManager.Instance.PushObj(GameManager.Instance.GameConf.Keluosi_bullet, gameObject);
    }
}
