using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Urbian : OfficialBase
{
    private List<EnemyBase> enemies;
    private float attackCD;
    public float attackTimer;
    private float attackTime;

    //���弼�ܵĳ���ʱ�����ʱ��
    private float skillTime;
    private float skillTimer;

    //���弼���ͷŶ���
    EnemyBase skillTarget;

    //���弼���д�ê�Ƿ��׳�ȥ
    private bool isThrow;

    //�����Ƿ��ڿ�ʼ����״̬�뿪ʼ״̬��ʱ��
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
        healthBar = transform.Find("HealthBar").gameObject; //��Ѫ����ʵ�帳��healthBar
        energyBar = transform.Find("EnergyBar").gameObject; //����������ʵ�帳��energyBar
    }

    private Vector3 creatBulletOffsetPos = new Vector2(0.39f, 0); //�����ӵ���ƫ����


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
        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.UrbianAttack);
        enemy.Hurt(attackValue, 0);
    }

    private void EnableUpdate()
    {
        if (healthBar != null)
        {
            healthBar.GetComponent<HealthBar>().HealthBarUpdate(hp / maxHp * 0.6f); //����Ѫ�����º���
        }
        if (energyBar != null)
        {
            energyBar.GetComponent<EnergyBar>().EnergyBarUpdate(sp / maxSp * 0.6f); //���ü������º���
        }
        HpMinimumGuarantee(); //Ѫ�����׻��ƣ�ʹѪ����Զ�������0

        //��ʼ״̬������
        startController();

        //����״̬������
        AttackController();

        //sp������
        SpController();

        //���ܿ�����
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

        //���������ʱ���ص�idle����attack״̬
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
        //�����ڼ���״̬���߿�ʼ״̬ʱ���ù�����ʱ�������ҷ���
        if (isSkill || isStarting)
        {
            attackTimer = 1.5521f;
            return;
        }
        ////�ӵ��˹������л�ȡ������ͬһ���Ҹ�����ͬһ�����ǰ��һ������е���
        enemies = EnemyManager.Instance.GetEnemiesInTheFront(lineNum, arrangeNum);
        if (enemies.Count == 0 ) //���û�е��ˣ��򱣳�Idle����ʱ����λ,������һ�ֿ����Ǵ��ڿ�ʼ״̬���Ƿ�Χ�ڳ��ֵ���
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
