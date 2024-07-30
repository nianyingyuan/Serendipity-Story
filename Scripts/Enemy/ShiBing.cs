using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShiBing : EnemyBase
{
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyHealthBar = transform.Find("EnemyHealthBar").gameObject; //��Ѫ����ʵ�帳��healthBar
        EnemyManager.Instance.AddEnemy(this);
        CheckOrder(); //������ʾ����
        speed = 0.4f;
        maxHp = 160f;
        hp = maxHp;
        physicalDefense = 12;
        spellDefense = 5;
        attackValue = 40f;
        attackTime = 0.9834f;
        attackTimer = 0.5934f;
        healthBarLength = 0.45f;
    }


}
