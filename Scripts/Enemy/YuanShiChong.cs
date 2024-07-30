using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YuanShiChong : EnemyBase
{
    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyHealthBar = transform.Find("EnemyHealthBar").gameObject; //��Ѫ����ʵ�帳��healthBar
        EnemyManager.Instance.AddEnemy(this);
        CheckOrder(); //������ʾ����
        speed = 0.5f;
        maxHp = 100f;
        hp = maxHp;
        physicalDefense = 10;
        spellDefense = 0;
        attackValue = 30f;
        attackTime = 0.75f;
        attackTimer = 0.35f;
        healthBarLength = 2.1f;
    }
}
