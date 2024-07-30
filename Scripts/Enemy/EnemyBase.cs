using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public enum EnemyState
{
    Idel,
    Walk,
    Attack,
    Dead,
    InDoor,
    Skill
}
public class EnemyBase : MonoBehaviour
{
    //���˵�״̬
    protected EnemyState state;

    protected Animator animator;
    protected Grid currGrid; //��������������
    public int lineNum; //������������
    public int arrangeNum; //������������
    protected SpriteRenderer spriteRenderer;

    //�ٶȣ������Ҽ�����һ��
    public float speed;
    //����
    public float hp;
    protected float maxHp;
    public float physicalDefense;
    public float spellDefense;
    protected float attackValue;

    //������ʱ��
    protected float attackTime;
    protected float attackTimer;

    public GameObject enemyHealthBar; //����һ����Ϸ��������ΪhealthBar

    //��������ܵ�����ʱ�ĳ�ʼ��ɫ���ܻ���ɫ
    protected Color startColor = new Color(1f, 1f, 1f);  // ��ʼ��ɫΪ��ɫ
    protected Color targetColor = new Color(0.96f, 0.56f, 0.57f);  // Ŀ����ɫΪ��ɫ

    //�������Ѫ��������
    protected float healthBarLength;

    //�޸�״̬��ֱ�Ӹı䶯��
    public EnemyState State { get => state;
        set{
            state = value;
            switch (state)
            {
                case EnemyState.Idel:
                    animator.Play("Idle");
                    break;
                case EnemyState.Walk:
                    animator.Play("Move");
                    break;
                case EnemyState.Attack:
                    animator.Play("Attack");
                    break;
                case EnemyState.Dead:
                    animator.Play("Die");
                    break;
                case EnemyState.InDoor:
                    animator.Play("Idle");
                    break;
                case EnemyState.Skill:
                    animator.Play("Skill");
                    break;
            }
        }
    }

    public Grid CurrGrid { get => currGrid; } //����currGrid��ʹ�����Ե��ã�ɾ��setʹ����޷��޸�

    //״̬���,��ÿ��״̬Ҫ��ʲô
    protected virtual void FSM()
    {
        switch (State)
        {
            case EnemyState.Idel:
                State = EnemyState.Walk;
                break;
            case EnemyState.Walk:
                //�����ߣ�����ֲ��ṥ������������������
                Move();
                break;
            case EnemyState.Attack:
                Attack(currGrid.CurrOfficialBase);
                break;
            case EnemyState.Dead:
                break;
            case EnemyState.InDoor:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyHealthBar != null)
        {
            enemyHealthBar.GetComponent<HealthBar>().HealthBarUpdate(hp / maxHp * healthBarLength); //����Ѫ���ͼ������º���
        }
        CheckGrid(); //����Լ����ĸ�����
        FSM(); ////״̬���,��ÿ��״̬Ҫ��ʲô

        //��ȡ�Լ�������������
        if(LVManager.Instance.CurrLV == 1)
        {
            LineNumCheckFor0_1(); //����Լ�������һ��
            ArrangeNumCheckFor0_1(); //����Լ�������һ��
        }
        else if(LVManager.Instance.CurrLV == 2)
        {
            LineNumCheckForUr_1();
            ArrangeNumCheckForUr_1();
        }
        else if(LVManager.Instance.CurrLV == 3)
        {
            LineNumCheckForKr_1();
            ArrangeNumCheckForKr_1();
        }

        HpMinimumGuarantee(); //Ѫ�����׻��ƣ�ʹѪ����Զ������0
    }

    protected void Move()
    {
        switch (LVManager.Instance.CurrLV)
        {
            case 1:
                //����ߵ����������
                if (transform.position.x < -5f)
                {
                    //ֹͣ���е��˵��˶�
                    EnemyManager.Instance.StopAllEnemies();
                    //��Ϸ����
                    LVManager.Instance.MissionFailed();
                    State = EnemyState.InDoor;
                }
                break;
            case 2:
                //����ߵ����������
                if (transform.position.x < -7f)
                {
                    //ֹͣ���е��˵��˶�
                    EnemyManager.Instance.StopAllEnemies();
                    //��Ϸ����
                    LVManager.Instance.MissionFailed();
                    State = EnemyState.InDoor;
                }
                break;
            case 3:
                //����ߵ����������
                if (transform.position.x < -8.7f)
                {
                    //ֹͣ���е��˵��˶�
                    EnemyManager.Instance.StopAllEnemies();
                    //��Ϸ����
                    LVManager.Instance.MissionFailed();
                    State = EnemyState.InDoor;
                }
                break;
        }
        if (!currGrid.HaveOfficial && hp > 0) //������������û�и�Ա����ô�����ƶ�
        {
            spriteRenderer.flipX = true;
            transform.Translate(-(new Vector2(1.33f, 0) * (Time.deltaTime / 1)) * speed); //ÿ��λ����
        }
        else if(currGrid.HaveOfficial && hp > 0) //����������֮���и�Ա����ô���������еĸ�Ա
        {
            State = EnemyState.Attack;
            return;
        }
        return;
    }

    protected virtual void CheckGrid() //�˺������ڼ�������ڵĸ���
    {
        currGrid = GridManager.Instance.GetGridByWorldPos(transform.position);
    }

    protected virtual void Attack(OfficialBase official) //�˺������ڶ��幥������߼�
    {
        if (currGrid.HaveOfficial)
        {
            if (transform.position.x < currGrid.CurrOfficialBase.transform.position.x) //���Դʯ��λ���ڸ�Ա�󷽣���ת��
            {
                spriteRenderer.flipX = false;
            }
        }
        else
        {
            spriteRenderer.flipX = true;
        }
        attackTimer += Time.deltaTime;
        if(attackTimer >= attackTime && official != null)
        {
            official.Hurt(attackValue, 0);
            attackTimer -= attackTime;
        }
        if (!currGrid.HaveOfficial) //������������û�и�Ա����ô�����ƶ�
        {
            attackTimer = 0.35f;
            attackTime = 0.75f;
            State = EnemyState.Walk;
            return;
        }
    }

    private void LineNumCheckFor0_1() //�˺��������жϵ�������һ��
    {
        if ((currGrid.Position.y + 0.61) > 0.9f && (currGrid.Position.y + 0.61) < 1f)
        {
            lineNum = 1;
        }
        else if ((currGrid.Position.y + 0.61) > 1.9f && (currGrid.Position.y + 0.61) < 2.2f)
        {
            lineNum = 2;
        }
    }

    private void LineNumCheckForUr_1() 
    {
        if ((currGrid.Position.y + 0.61) > -0.6f && (currGrid.Position.y + 0.61) < -0.5f)
        {
            lineNum = 0;
        }
        else if ((currGrid.Position.y + 0.61) > 0.8f && (currGrid.Position.y + 0.61) < 1f)
        {
            lineNum = 1;
        }
        else if ((currGrid.Position.y + 0.61) > 2f && (currGrid.Position.y + 0.61) < 2.5f)
        {
            lineNum = 2;
        }
        else if ((currGrid.Position.y + 0.61) > 3f && (currGrid.Position.y + 0.61) < 3.5f)
        {
            lineNum = 3;
        }
    }

    protected void LineNumCheckForKr_1()
    {
        if ((currGrid.Position.y + 0.61) > -1.5f && (currGrid.Position.y + 0.61) < 0f)
        {
            lineNum = 0;
        }
        else if ((currGrid.Position.y + 0.61) > 0f && (currGrid.Position.y + 0.61) < 1f)
        {
            lineNum = 1;
        }
        else if ((currGrid.Position.y + 0.61) > 1f && (currGrid.Position.y + 0.61) < 2f)
        {
            lineNum = 2;
        }
        else if ((currGrid.Position.y + 0.61) > 2f && (currGrid.Position.y + 0.61) < 3f)
        {
            lineNum = 3;
        }
        else if ((currGrid.Position.y + 0.61) > 3f && (currGrid.Position.y + 0.61) < 4f)
        {
            lineNum = 4;
        }
    }

    private void ArrangeNumCheckFor0_1() //�˺��������жϵ�������һ��
    {
        if((currGrid.Position.x + 3.9) > 0.1f && (currGrid.Position.x + 3.9) < 0.8f)
        {
            arrangeNum = 0;
        }
        else if((currGrid.Position.x + 3.9) > 1f && (currGrid.Position.x + 3.9) < 2f)
        {
            arrangeNum = 1;
        }
        else if ((currGrid.Position.x + 3.9) > 2.7f && (currGrid.Position.x + 3.9) < 3.2f)
        {
            arrangeNum = 2;
        }
        else if ((currGrid.Position.x + 3.9) > 4f && (currGrid.Position.x + 3.9) < 4.5f)
        {
            arrangeNum = 3;
        }
        else if ((currGrid.Position.x + 3.9) > 5.4f && (currGrid.Position.x + 3.9) < 5.8f)
        {
            arrangeNum = 4;
        }
        else if ((currGrid.Position.x + 3.9) > 6.7f && (currGrid.Position.x + 3.9) < 7.2f)
        {
            arrangeNum = 5;
        }
        else if ((currGrid.Position.x + 3.9) > 7.8f && (currGrid.Position.x + 3.9) < 8.6f)
        {
            arrangeNum = 6;
        }
        else if((currGrid.Position.x + 3.9) > 9f && (currGrid.Position.x + 3.9) < 10f)
        {
            arrangeNum = 7;
        }

    }

    private void ArrangeNumCheckForUr_1()
    {
        if ((currGrid.Position.x + 3.9) > -3.1f && (currGrid.Position.x + 3.9) < -2f)
        {
            arrangeNum = 0;
        }
        else if ((currGrid.Position.x + 3.9) > -1.5f && (currGrid.Position.x + 3.9) < -0.5f)
        {
            arrangeNum = 1;
        }
        else if ((currGrid.Position.x + 3.9) > 0.2f && (currGrid.Position.x + 3.9) < 0.7f)
        {
            arrangeNum = 2;
        }
        else if ((currGrid.Position.x + 3.9) > 1.8f && (currGrid.Position.x + 3.9) < 2.15f)
        {
            arrangeNum = 3;
        }
        else if ((currGrid.Position.x + 3.9) > 3.4f && (currGrid.Position.x + 3.9) < 3.6f)
        {
            arrangeNum = 4;
        }
        else if ((currGrid.Position.x + 3.9) > 4.9f && (currGrid.Position.x + 3.9) < 5.05f)
        {
            arrangeNum = 5;
        }
        else if ((currGrid.Position.x + 3.9) > 6.3f && (currGrid.Position.x + 3.9) < 6.8f)
        {
            arrangeNum = 6;
        }
        else if ((currGrid.Position.x + 3.9) > 7.7f && (currGrid.Position.x + 3.9) < 8.5f)
        {
            arrangeNum = 7;
        }
        else if ((currGrid.Position.x + 3.9) > 9f && (currGrid.Position.x + 3.9) < 10f)
        {
            arrangeNum = 8;
        }
    }

    private void ArrangeNumCheckForKr_1()
    {
        if ((currGrid.Position.x + 3.9) > -3.9f && (currGrid.Position.x + 3.9) < -2f)
        {
            arrangeNum = 0;
        }
        else if ((currGrid.Position.x + 3.9) > -2f && (currGrid.Position.x + 3.9) < -1f)
        {
            arrangeNum = 1;
        }
        else if ((currGrid.Position.x + 3.9) > -1f && (currGrid.Position.x + 3.9) < 0f)
        {
            arrangeNum = 2;
        }
        else if ((currGrid.Position.x + 3.9) > 0.5f && (currGrid.Position.x + 3.9) < 1.8f)
        {
            arrangeNum = 3;
        }
        else if ((currGrid.Position.x + 3.9) > 2.5f && (currGrid.Position.x + 3.9) < 3f)
        {
            arrangeNum = 4;
        }
        else if ((currGrid.Position.x + 3.9) > 3.5f && (currGrid.Position.x + 3.9) < 4.5f)
        {
            arrangeNum = 5;
        }
        else if ((currGrid.Position.x + 3.9) > 5.2f && (currGrid.Position.x + 3.9) < 6.2f)
        {
            arrangeNum = 6;
        }
        else if ((currGrid.Position.x + 3.9) > 6.8f && (currGrid.Position.x + 3.9) < 7.5f)
        {
            arrangeNum = 7;
        }
        else if ((currGrid.Position.x + 3.9) > 8f && (currGrid.Position.x + 3.9) < 9f)
        {
            arrangeNum = 8;
        }
        else if ((currGrid.Position.x + 3.9) > 9.2f && (currGrid.Position.x + 3.9) < 10.6f)
        {
            arrangeNum = 9;
        }
        else if ((currGrid.Position.x + 3.9) > 10.5f && (currGrid.Position.x + 3.9) < 12.5f)
        {
            arrangeNum = 10;
        }
    }

    public void Hurt(float hurtValue, int hurtType) //�˺��������Ա���˺�������Ҫ��ȡ���˵Ĺ������빥�����ͣ�0Ϊ����1Ϊ����2Ϊ��ʵ,��ͬʱ�����ܻ�������Ч
    {
        switch (hurtType)
        {
            case 0:
                if(hurtValue > physicalDefense)
                {
                    hp -= (hurtValue - physicalDefense);
                    //print("�����" + (hurtValue - physicalDefense) + "�������˺�");
                }
                else
                {
                    //print("�����0�������˺�");
                }
                StartCoroutine(TransitionColor());
                break;
            case 1:
                hp -= (hurtValue / (1 - spellDefense));
                StartCoroutine(TransitionColor());
                //print("�����" + (hurtValue / (1 - spellDefense)) + "�㷨���˺�");
                break;
            case 2:
                hp -= hurtValue;
                StartCoroutine(TransitionColor());
                //print("�����" + hurtValue + "����ʵ�˺�");
                break;
            default:
                break;
        }
        if (hp <= 0)
        {
            hp = 0;
            state = EnemyState.Dead;
            Dead();
        }
    }

    IEnumerator TransitionColor()
    {
        float t = 0f;
        float duration = 0.2f; // �ܱ仯ʱ��Ϊ 0.2 ��
        bool increasing = true; // ��ǵ�ǰ�Ƿ��������� t

        while (true)
        {
            if (increasing)
            {
                t += Time.deltaTime / duration; // t �� 0 ���ӵ� 1
                if (t >= 1f)
                {
                    t = 1f;
                    increasing = false; // �� t �ﵽ 1 ��ʼ����
                }
            }
            else
            {
                t -= Time.deltaTime / duration; // t �� 1 ���ٵ� 0
                if (t <= 0f)
                {
                    t = 0f;
                    increasing = true; // �� t ���ٵ� 0 ��ʼ����
                    yield break; // ִ����һ����ɫ�仯�����Э��
                }
            }

            spriteRenderer.material.color = Color.Lerp(startColor, targetColor, t);

            yield return null;
        }
    }

    public void Dead()
    {
        animator.Play("Die");
        Destroy(enemyHealthBar); //��ɾ��Ѫ��
        EnemyManager.Instance.RemoveEnemy(this); //�ڵ��˹��������Ƴ��������
        LVManager.Instance.currEnemyNum++; //lv���������ѻ��ܵ�������һ
        Invoke("DestroyGameobject", 1.2f); // ��1.2s�����ٶ���
    }

    private void DestroyGameobject()
    {
        Destroy(gameObject);
    }

    protected void HpMinimumGuarantee()
    {
        if (hp < 0)
        {
            hp = 0;
        }
        if (hp > maxHp)
        {
            hp = maxHp;
        }
    }

    protected void CheckOrder() //�˺���������ʾ�㼶
    {
        //������������·�ϣ�Խ����0Խ��,��֮ԽС
        //1�����2��
        spriteRenderer.sortingOrder = (5 - lineNum) * (arrangeNum + 1);
    }
}
