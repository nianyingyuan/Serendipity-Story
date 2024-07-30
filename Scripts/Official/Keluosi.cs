using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keluosi : OfficialBase
{
    private float attackCD;
    public float attackTimer;
    private float attackTime;

    void Start()
    {
        officialCode = 2;
        hp = 100f;
        maxHp = 100f;
        sp = 0f;
        maxSp = 5f;
        physicalDefense = 10f;
        spellDefense = 0f;
        attackValue = 30;

        attackCD = 0.9166f;
        attackTimer = -0.2f;


        TransitionToDisable();
        anim = GetComponent<Animator>();
        healthBar = transform.Find("HealthBar").gameObject; //将血条的实体赋给healthBar
        energyBar = transform.Find("EnergyBar").gameObject; //将技力条的实体赋给energyBar
    }

    private Vector3 creatBulletOffsetPos = new Vector2(0.39f, 0.23f); //定义子弹的偏移量


    void Update()
    {
        switch (officialState)
        {
            case OfficialState.Disable:
                DisableUpdate();
                break;
            case OfficialState.Enable:
                EnableUpdate();
                break;
            default:
                break;
        }

    }


    private void Attack(EnemyBase enemy) //此函数定义攻击方法-循环检测
    {
        //实例化一个子弹,从gameconf调用实体
        KeLuoSi_Bullet bullet = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.Keluosi_bullet).GetComponent<KeLuoSi_Bullet>();
        bullet.transform.SetParent(transform);
        bullet.transform.position = transform.position + creatBulletOffsetPos;
        //KeLuoSi_Bullet bullet = GameObject.Instantiate<GameObject>(GameManager.Instance.GameConf.Keluosi_bullet,transform.position + creatBulletOffsetPos , Quaternion.identity , transform).GetComponent<KeLuoSi_Bullet>();
        bullet.targetEnemy = enemy;
        if (sp == maxSp)
        {
            bullet.attackNum = attackValue * 2;
            sp -= maxSp;
        }
        else if(sp < maxSp)
        {
            bullet.attackNum = attackValue;
        }
        AddSp(); //每攻击一次，sp就加一
        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.KeLuoSiAttack);
    }

    private void EnableUpdate()
    {
        if (healthBar != null)
        {
            healthBar.GetComponent<HealthBar>().HealthBarUpdate(hp / maxHp * 1.8f); //调用血条更新函数
        }
        if (energyBar != null)
        {
            energyBar.GetComponent<EnergyBar>().EnergyBarUpdate(sp / maxSp * 1.8f); //调用技力更新函数
        }
        HpMinimumGuarantee(); //血量保底机制，使血量永远不会低于0

        ////从僵尸管理器中获取一个与我在同一行且离我最近的敌人
        EnemyBase enemy = EnemyManager.Instance.GetEnemyByLineMinDistance(lineNum, transform.position);
        if (enemy == null ) //如果没有敌人，则保持Idle，计时器归位
        {
            attackTimer = 0.45f;
            anim.SetBool("isAttacking", false);
        }
        else if(enemy != null)
        {
            anim.SetBool("isAttacking", true);
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackCD)
            {
                Attack(enemy);
                attackTimer -= attackCD;
            }

        }


    }
    public void DisableUpdate()
    {
        
    }

    private void AddSp()
    {
        sp += 1;
    }
}
