using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeLuoSi_Bullet : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public EnemyBase targetEnemy; //定义目标敌人
    public Vector3 targetPosition; //目标敌人的位置

    //定义与目标的距离
    private float distanceX;
    private float distanceY;

    //定义攻击力与攻击类型
    public float attackNum;

    //定义销毁时间
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
        if(destroyTimer > 0.2f ) //当二者距离低于0.5f时视为击中目标
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
        //取消自身全部协程和延迟调用,如果没有就不用写
       
        //放进缓存池，不做真实销毁
        PoolManager.Instance.PushObj(GameManager.Instance.GameConf.Keluosi_bullet, gameObject);
    }
}
