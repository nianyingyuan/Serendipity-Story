using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buu : EnemyBase
{
    // Start is called before the first frame update

    private bool isAttacking;
    public float sp;
    public float maxSp;

    public float hpTimer;

    void Start()
    {
        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.BuuPlace);
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyHealthBar = transform.Find("EnemyHealthBar").gameObject; //将血条的实体赋给healthBar
        EnemyManager.Instance.AddEnemy(this);
        spriteRenderer.sortingOrder = 999;
        speed = 0.6f;
        maxHp = 1000f;
        hp = maxHp;
        physicalDefense = 40;
        spellDefense = 0;
        attackValue = 70f;
        attackTime = 5f;
        attackTimer = 0f;
        healthBarLength = 0.45f;

        isAttacking = false;

        state = EnemyState.Idel;

        sp = 5;
        maxSp = 15;

        hpTimer = 0f;
    }

    protected override void FSM()
    {
        if(currGrid.CurrOfficialBase == null)
        {
            print("NUll");
        }
        HPProducer();
        switch (State)
        {
            case EnemyState.Idel:
                if(LVManager.Instance.buuMove == true)
                {
                    state = EnemyState.Walk;
                }
                break;
            case EnemyState.Walk:
                //向左走，遇到植物会攻击，攻击结束继续走
                animator.Play("Move");
                Move();
                SkillController();
                break;
            case EnemyState.Attack:
                Attack(currGrid.CurrOfficialBase);
                SkillController();
                break;
            case EnemyState.Dead:
                break;
            case EnemyState.InDoor:
                break;
            case EnemyState.Skill:
                animator.Play("Skill");
                break;
        }
    }

    protected override void Attack(OfficialBase official)
    {
        if (currGrid.HaveOfficial)
        {
            if (transform.position.x < currGrid.CurrOfficialBase.transform.position.x) //如果布欧位置在干员后方，则转身
            {
                spriteRenderer.flipX = false;
            }
            if (isAttacking == false)
            {
                StartCoroutine(AttackProcess(official));
            }
        }
        else
        {
            spriteRenderer.flipX = true;
            State = EnemyState.Walk;
            return;
        }

    }

    private void AttackOneTime(OfficialBase official)
    {
        if (official != null)
        {
            official.Hurt(attackValue, 0);
        }
    }

    IEnumerator AttackProcess(OfficialBase official)
    {
        isAttacking = true;
        yield return new WaitForSeconds(0.4237f);
        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.KakarotAttack);
        AttackOneTime(currGrid.CurrOfficialBase);
        yield return new WaitForSeconds(0.5085f);
        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.KakarotAttack);
        AttackOneTime(currGrid.CurrOfficialBase);
        yield return new WaitForSeconds(0.2966f);
        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.KakarotAttack);
        AttackOneTime(currGrid.CurrOfficialBase);
        yield return new WaitForSeconds(0.678f);
        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.KakarotAttack);
        AttackOneTime(currGrid.CurrOfficialBase);
        yield return new WaitForSeconds(0.5932f);
        isAttacking = false;
    }

    protected override void CheckGrid()
    {
        currGrid = GridManager.Instance.GetGridByWorldPos(transform.position + new Vector3(-2f, 0, 0));
    }

    private void SkillController()
    {
        if (sp < maxSp)
        {
            sp += Time.deltaTime;
        }
        if (sp >= maxSp && OfficialManager.Instance.officials.Count > 0)
        {
            StopAllCoroutines();
            state = EnemyState.Skill;
            AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.BuuSkillSpeaking);
            StartCoroutine(SkillProcess(OfficialManager.Instance.officials , EnemyManager.Instance.enemies));
            sp -= maxSp;
        }
    }

    private void SkillAttack(List<OfficialBase> officials, List<EnemyBase> enemies)
    {
        for(int i = 0; i < officials.Count; i++)
        {
            officials[i].Hurt(10, 2);
        }
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].Hurt(20, 2);
        }

    }

    IEnumerator SkillProcess(List<OfficialBase> officials , List<EnemyBase> enemies)
    {
        Vector3 scale = transform.localScale;
        Vector3 newScale = new Vector3(scale.x * 2, scale.y * 2, scale.z * 2);
        transform.localScale = newScale;
        state = EnemyState.Skill;
        yield return new WaitForSeconds(2.197f);
        SkillAttack(officials, enemies);
        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.BuuSkillEF);
        for (int i = 0; i < 34; i++)
        {
            yield return new WaitForSeconds(0.08449f);
            SkillAttack(officials, enemies);
        }
        yield return new WaitForSeconds(1.01334f);
        transform.localScale = scale;
        state = EnemyState.Walk;
        isAttacking = false;
    }

    private void HPProducer() //此函数用于布欧的回血
    {
        hpTimer += Time.deltaTime;
        if(hpTimer >= 1f)
        {
            hp += 200;
            hpTimer -= 1f;
        }
    }
}
