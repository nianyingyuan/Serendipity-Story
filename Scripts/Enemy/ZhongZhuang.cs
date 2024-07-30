using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZhongZhuang : EnemyBase
{
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyHealthBar = transform.Find("EnemyHealthBar").gameObject; //��Ѫ����ʵ�帳��healthBar
        EnemyManager.Instance.AddEnemy(this);
        CheckOrder(); //������ʾ����
        speed = 0.2f;
        maxHp = 400f;
        hp = maxHp;
        physicalDefense = 40;
        spellDefense = 10;
        attackValue = 35f;
        attackTime = 1.25f;
        attackTimer = 0.7203f;
        healthBarLength = 0.45f;
    }
}
