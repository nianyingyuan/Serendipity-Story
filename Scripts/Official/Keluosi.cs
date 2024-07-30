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
        healthBar = transform.Find("HealthBar").gameObject; //��Ѫ����ʵ�帳��healthBar
        energyBar = transform.Find("EnergyBar").gameObject; //����������ʵ�帳��energyBar
    }

    private Vector3 creatBulletOffsetPos = new Vector2(0.39f, 0.23f); //�����ӵ���ƫ����


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


    private void Attack(EnemyBase enemy) //�˺������幥������-ѭ�����
    {
        //ʵ����һ���ӵ�,��gameconf����ʵ��
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
        AddSp(); //ÿ����һ�Σ�sp�ͼ�һ
        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.KeLuoSiAttack);
    }

    private void EnableUpdate()
    {
        if (healthBar != null)
        {
            healthBar.GetComponent<HealthBar>().HealthBarUpdate(hp / maxHp * 1.8f); //����Ѫ�����º���
        }
        if (energyBar != null)
        {
            energyBar.GetComponent<EnergyBar>().EnergyBarUpdate(sp / maxSp * 1.8f); //���ü������º���
        }
        HpMinimumGuarantee(); //Ѫ�����׻��ƣ�ʹѪ����Զ�������0

        ////�ӽ�ʬ�������л�ȡһ��������ͬһ������������ĵ���
        EnemyBase enemy = EnemyManager.Instance.GetEnemyByLineMinDistance(lineNum, transform.position);
        if (enemy == null ) //���û�е��ˣ��򱣳�Idle����ʱ����λ
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
