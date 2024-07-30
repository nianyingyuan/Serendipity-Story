using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Urbian : OfficialBase
{
    private List<EnemyBase> enemies;
    private float attackCD;
    public float attackTimer;
    private float attackTime;

    //定义技能的持续时间与计时器
    private float skillTime;
    private float skillTimer;

    //定义技能释放对象
    EnemyBase skillTarget;

    //定义技能中船锚是否抛出去
    private bool isThrow;

    //定义是否处于开始动画状态与开始状态计时器
    private bool isStarting;
    private float startTimer;
    void Start()
    {
        officialCode = 3;
        hp = 500f;
        maxHp = 500f;
        sp = 10f;
        maxSp = 15f;
        physicalDefense = 0f;
        spellDefense = 0f;
        attackValue = 100;

        attackCD = 2.0555f;
        attackTimer = 0.7188f;

        skillTimer = 0;
        skillTime = 1.667f;

        isStarting = true;
        startTimer = 0;

        TransitionToDisable();
        anim = GetComponent<Animator>();
        healthBar = transform.Find("HealthBar").gameObject; //将血条的实体赋给healthBar
        energyBar = transform.Find("EnergyBar").gameObject; //将技力条的实体赋给energyBar
    }

    private Vector3 creatBulletOffsetPos = new Vector2(0.39f, 0); //定义子弹的偏移量


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
        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.UrbianAttack);
        enemy.Hurt(attackValue, 0);
    }

    private void EnableUpdate()
    {
        if (healthBar != null)
        {
            healthBar.GetComponent<HealthBar>().HealthBarUpdate(hp / maxHp * 0.6f); //调用血条更新函数
        }
        if (energyBar != null)
        {
            energyBar.GetComponent<EnergyBar>().EnergyBarUpdate(sp / maxSp * 0.6f); //调用技力更新函数
        }
        HpMinimumGuarantee(); //血量保底机制，使血量永远不会低于0

        //开始状态控制器
        startController();

        //攻击状态控制器
        AttackController();

        //sp控制器
        SpController();

        //技能控制器
        SkillController();

    }
    public void DisableUpdate()
    {

    }

    private void SpController()
    {
        skillTarget = EnemyManager.Instance.GetEnemyByLineMinDistance(lineNum, transform.position);
        if (sp < maxSp)
        {
            sp += Time.deltaTime;
        }
        else if (sp >= maxSp && skillTarget == null)
        {
            return;
        }
        else if(sp >= maxSp && skillTarget != null)
        {
            anim.SetBool("isSkill", true);
            isSkill = true;
            sp -= maxSp;
            AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.UrbianSkill);
        }
        
    }

    private void SkillController()
    {
        if (!isSkill) return;
        skillTimer += Time.deltaTime;

        if(isThrow ==false)
        {
            AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.UrbianSkillSpeaking);
            AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.SkillStart);
            StartCoroutine(ThrowAnchor());
            isThrow = true;
        }

        //当到达持续时间后回到idle或者attack状态
        if (skillTimer >= skillTime)
        {
            isSkill = false;
            anim.SetBool("isSkill", false);
            skillTimer = 0;
            isThrow = false;
        }

    }

    private void AttackController()
    {
        //当处于技能状态或者开始状态时重置攻击计时器，并且返回
        if (isSkill || isStarting)
        {
            attackTimer = 1.5521f;
            return;
        }
        ////从敌人管理器中获取与我在同一行且跟我在同一格或者前面一格的所有敌人
        enemies = EnemyManager.Instance.GetEnemiesInTheFront(lineNum, arrangeNum);
        if (enemies.Count == 0 ) //如果没有敌人，则保持Idle，计时器归位,这里有一种可能是处于开始状态但是范围内出现敌人
        {
            attackTimer = 1.5521f;
            
            anim.SetBool("isAttacking", false);
        }
        else if (enemies.Count != 0 )
        {
            anim.SetBool("isAttacking", true);
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackCD)
            {
                for (int i = 0; i < enemies.Count; i++)
                {
                    Attack(enemies[i]);
                }
                attackTimer -= attackCD;
            }

        }
    }

    private void startController()
    {
        if (!isStarting) return;
        startTimer += Time.deltaTime;
        if (startTimer >= 0.8333)
        {
            isStarting = false;
        }
    }

    IEnumerator ThrowAnchor()
    {
        yield return new WaitForSeconds(0.6303f);
        GameObject anchorPrefab = GameManager.Instance.GameConf.Urbian_anchor;
        GameObject anchor = GameObject.Instantiate(anchorPrefab, transform.position, Quaternion.identity, transform);
        Urbian_Anchor anchorScript = anchor.GetComponent<Urbian_Anchor>();
        anchorScript.lineNum = lineNum;
    }
}
