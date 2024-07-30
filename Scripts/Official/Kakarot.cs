using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Form
{
    First,
    Second,
    Third
}

public class Kakarot : OfficialBase
{
    //���ܹ�������
    private List<EnemyBase> enemies;
    //��ͨ��������
    private EnemyBase targetEnemy;
    private float attackCD;
    public float attackTimer;
    private float attackTime;

    //�Ƿ��ڽ��й���
    private bool isAttacking;

    //�Ƿ��ڿ�ʼ����
    public bool isStarting;

    //��̬
    public Form form;

    //�����ʼ��С
    private Vector3 ownScale;
    // Start is called before the first frame update
    void Start()
    {
        officialCode = 4;
        FirstStateAttributeUpdate();
        TransitionToDisable();
        anim = GetComponent<Animator>();
        healthBar = transform.Find("HealthBar").gameObject; //��Ѫ����ʵ�帳��healthBar
        energyBar = transform.Find("EnergyBar").gameObject; //����������ʵ�帳��energyBar

        ownScale = transform.localScale;

    }

    // Update is called once per frame
    void Update()
    {
        if (!isAttacking) transform.localScale = ownScale;
        if (officialState == OfficialState.Disable) return;
        if (healthBar != null)
        {
            healthBar.GetComponent<HealthBar>().HealthBarUpdate(hp / maxHp * 0.3f); //����Ѫ�����º���
        }
        if (energyBar != null)
        {
            energyBar.GetComponent<EnergyBar>().EnergyBarUpdate(sp / maxSp * 0.3f); //���ü������º���
        }
        HpMinimumGuarantee(); //Ѫ�����׻��ƣ�ʹѪ����Զ�������0
        switch (form)
        {
            case Form.First:
                FirstStateUpdate();
                break;
            case Form.Second:
                SecondStateUpdate();
                break;
            case Form.Third:
                ThirdStateUpdate();
                break;
            default:
                break;
        }

        AttackController();

        SpController();
    }

    private void FirstStateAttributeUpdate()
    {
        officialCode = 4;
        hp = 150f;
        maxHp = 150f;
        sp = 0f;
        maxSp = 10f;
        physicalDefense = 15f;
        spellDefense = 15f;
        attackValue = 30;
        form = Form.First;
        isStarting = true;
        Invoke("OverStartAnim", 1f);
    }

    private void SecondStateAttributeUpdate()
    {
        hp += 150f;
        maxHp = 300f;
        sp = 0f;
        maxSp = 20f;
        physicalDefense = 30f;
        spellDefense = 30f;
        attackValue = 60;
        form = Form.Second;
        isStarting = true;
        Invoke("OverStartAnim", 0.666f);
    }

    private void ThirdStateAttributeUpdate()
    {
        hp = 1250f;
        maxHp = 1250f;
        sp = 0f;
        maxSp = 45f;
        physicalDefense = 60f;
        spellDefense = 50f;
        attackValue = 150;
        form = Form.Third;
        isStarting = true;
        Invoke("OverStartAnim", 0.75f);
        LVManager.Instance.buuMove = true;
    }

    private void FirstStateUpdate()
    {

    }

    private void SecondStateUpdate()
    {

    }

    private void ThirdStateUpdate()
    {

    }

    private void AttackController()
    {
        if (isStarting) return;
        targetEnemy = EnemyManager.Instance.GetEnemyInTheFront(lineNum,arrangeNum,transform.position);
        enemies = EnemyManager.Instance.GetEnemiesInLine(lineNum, arrangeNum ,transform.position);
        if(targetEnemy != null && isAttacking == false)
        {
            switch (form)
            {
                case Form.First:
                    StartCoroutine(Attack1());
                    break;
                case Form.Second:
                    StartCoroutine(Attack2());
                    break;
                case Form.Third:
                    StartCoroutine(Attack3());
                    break;
            }
        }
        if (isAttacking)
        {
            anim.SetBool("isAttacking", true);
        }
        else
        {
            anim.SetBool("isAttacking", false);
        }

    }
    IEnumerator Attack1()
    {
        isAttacking = true;
        yield return new WaitForSeconds(0.3306f);
        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.KakarotAttack);
        Attack(targetEnemy, attackValue / 2);
        yield return new WaitForSeconds(0.4258f);
        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.KakarotAttack);
        Attack(targetEnemy, attackValue / 2);
        yield return new WaitForSeconds(0.2555f);
        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.KakarotAttack);
        Attack(targetEnemy, attackValue / 2);
        yield return new WaitForSeconds(0.3406f);
        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.KakarotAttack);
        Attack(targetEnemy, attackValue / 2);
        yield return new WaitForSeconds(0.6812f);
        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.KakarotAttack);
        Attack(targetEnemy, attackValue);
        yield return new WaitForSeconds(0.1333f);
        isAttacking = false;
    }

    IEnumerator Attack2()
    {
        isAttacking = true;
        yield return new WaitForSeconds(0.3402f);
        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.KakarotAttack);
        Attack(targetEnemy, attackValue / 2);
        yield return new WaitForSeconds(1.0205f);
        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.KakarotAttack);
        Attack(targetEnemy, attackValue / 2);
        yield return new WaitForSeconds(0.3402f);
        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.KakarotAttack);
        Attack(targetEnemy, attackValue / 2);
        yield return new WaitForSeconds(0.5102f);
        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.KakarotAttack);
        Attack(targetEnemy, attackValue / 2);
        yield return new WaitForSeconds(1.1906f);
        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.KakarotAttack);
        Attack(targetEnemy, attackValue);
        yield return new WaitForSeconds(0.7653f);
        isAttacking = false;
    }

    IEnumerator Attack3()
    {
        isAttacking = true;
        yield return new WaitForSeconds(0.2555f);
        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.KakarotAttack);
        Attack(targetEnemy, attackValue / 2);
        yield return new WaitForSeconds(0.7238f);
        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.KakarotAttack);
        Attack(targetEnemy, attackValue / 2);
        yield return new WaitForSeconds(0.7238f);
        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.KakarotAttack);
        Attack(targetEnemy, attackValue);
        yield return new WaitForSeconds(0.2554f);
        isAttacking = false;
    }

    IEnumerator Skill3()
    {
        isAttacking = true;
        Vector3 scale = transform.localScale;
        Vector3 newScale = new Vector3(scale.x * 1.5f, scale.y * 1.5f, scale.z * 1.5f);
        transform.localScale = newScale;
        yield return new WaitForSeconds(1.5274f);
        SkillHurt();
        for(int i = 0; i < 34; i++)
        {
            yield return new WaitForSeconds(0.0849f);
            SkillHurt();
        }
        transform.localScale = scale;
        yield return new WaitForSeconds(0.253f);
        isAttacking = false;
        sp = 0;
    }
    private void Attack(EnemyBase enemy,float value) //�˺������幥������-ѭ�����
    {
        if (enemy != null)
        {
            enemy.Hurt(value, 0);
        }
    }

    private void SkillAttack(EnemyBase enemy, float value) //�˺������幥������-ѭ�����
    {
        if (enemy != null)
        {
            enemy.Hurt(60, 2);
        }
    }

    protected override void Dead() //�˺��������Ա��Ȼ����ʱ�Ĳ���,���ڿ��������������������������Լ��˸�switch��ע����������Ҫ����1.2s
    {
        if (hp <= 0)
        {
            switch (form)
            {
                case Form.First:
                    anim.Play("Die1");
                    break;
                case Form.Second:
                    anim.Play("Die2");
                    break;
                case Form.Third:
                    anim.Play("Die3");
                    break;
            }
        }
        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.OfficialDie);
        Destroy(healthBar);
        Destroy(energyBar);
        OfficialManager.Instance.RemoveOfficial(this);
        currGrid.CurrOfficialBase = null; //�ǰ����Ϊ��
        Invoke("DestroyGameobject", 1.2f); // ��1.2s�����ٶ���
    }

    private void OverStartAnim()
    {
        isStarting = false;
    }

    private void SpController()
    {
        if (sp < maxSp)
        {
            sp += Time.deltaTime;
        }
        else
        {
            switch (form)
            {
                case Form.First:
                    StopAllCoroutines(); //ֹͣ���е�Э�̣�����������
                    isAttacking = false; //������״̬����Ϊfalse


                    Invoke("Speaking2", 0.4f);
                    anim.Play("1To2");
                    SecondStateAttributeUpdate();
                    break;
                case Form.Second:
                    StopAllCoroutines();
                    isAttacking = false;
                    GameObject baoqi2Prefab = GameManager.Instance.GameConf.BaoQi2;
                    GameObject baoqi2 = GameObject.Instantiate(baoqi2Prefab, transform.position + new Vector3(0.25f, 0.25f, 0), Quaternion.identity, transform);
                    AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.KakarotSpeaking3);
                    AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.Explosion);
                    anim.Play("2To3");
                    ThirdStateAttributeUpdate();
                    break;
                case Form.Third:
                    if(enemies.Count > 0)
                    {
                        StopAllCoroutines();
                        isAttacking = false;
                        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.Teleport);
                        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.GuiPaiQiGong);
                        StartCoroutine(Skill3());
                        anim.Play("Skill3");
                        sp = 0;
                        Invoke("OverStartAnim", 4.667f);
                    }
                    else
                    {
                        return;
                    }
                    break;

            }
        }
    }

    private void SkillHurt()
    {
        for(int j = 0; j < enemies.Count; j++)
        {
            SkillAttack(enemies[j], attackValue / 2);
        }
    }

    private void Speaking2()
    {
        GameObject baoqi1Prefab = GameManager.Instance.GameConf.BaoQi1;
        GameObject baoqi1 = GameObject.Instantiate(baoqi1Prefab, transform.position + new Vector3(0, 2f, 0), Quaternion.identity, transform);
        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.KakarotSpeaking2);
        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.Explosion);
    }

}
