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
    //敌人的状态
    protected EnemyState state;

    protected Animator animator;
    protected Grid currGrid; //定义所在网格数
    public int lineNum; //定义所在行数
    public int arrangeNum; //定义所在列数
    protected SpriteRenderer spriteRenderer;

    //速度，决定我几秒走一格
    public float speed;
    //属性
    public float hp;
    protected float maxHp;
    public float physicalDefense;
    public float spellDefense;
    protected float attackValue;

    //攻击计时器
    protected float attackTime;
    protected float attackTimer;

    public GameObject enemyHealthBar; //定义一个游戏物体命名为healthBar

    //定义敌人受到攻击时的初始颜色与受击颜色
    protected Color startColor = new Color(1f, 1f, 1f);  // 开始颜色为白色
    protected Color targetColor = new Color(0.96f, 0.56f, 0.57f);  // 目标颜色为橙色

    //定义敌人血量条长度
    protected float healthBarLength;

    //修改状态可直接改变动画
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

    public Grid CurrGrid { get => currGrid; } //公开currGrid，使外界可以调用，删除set使外界无法修改

    //状态检测,在每个状态要做什么
    protected virtual void FSM()
    {
        switch (State)
        {
            case EnemyState.Idel:
                State = EnemyState.Walk;
                break;
            case EnemyState.Walk:
                //向左走，遇到植物会攻击，攻击结束继续走
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
            enemyHealthBar.GetComponent<HealthBar>().HealthBarUpdate(hp / maxHp * healthBarLength); //调用血条和技力更新函数
        }
        CheckGrid(); //检测自己在哪个格子
        FSM(); ////状态检测,在每个状态要做什么

        //获取自己的行数和列数
        if(LVManager.Instance.CurrLV == 1)
        {
            LineNumCheckFor0_1(); //检测自己处于哪一行
            ArrangeNumCheckFor0_1(); //检测自己处于哪一列
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

        HpMinimumGuarantee(); //血量保底机制，使血量永远不低于0
    }

    protected void Move()
    {
        switch (LVManager.Instance.CurrLV)
        {
            case 1:
                //如果走到最左边蓝门
                if (transform.position.x < -5f)
                {
                    //停止所有敌人的运动
                    EnemyManager.Instance.StopAllEnemies();
                    //游戏结束
                    LVManager.Instance.MissionFailed();
                    State = EnemyState.InDoor;
                }
                break;
            case 2:
                //如果走到最左边蓝门
                if (transform.position.x < -7f)
                {
                    //停止所有敌人的运动
                    EnemyManager.Instance.StopAllEnemies();
                    //游戏结束
                    LVManager.Instance.MissionFailed();
                    State = EnemyState.InDoor;
                }
                break;
            case 3:
                //如果走到最左边蓝门
                if (transform.position.x < -8.7f)
                {
                    //停止所有敌人的运动
                    EnemyManager.Instance.StopAllEnemies();
                    //游戏结束
                    LVManager.Instance.MissionFailed();
                    State = EnemyState.InDoor;
                }
                break;
        }
        if (!currGrid.HaveOfficial && hp > 0) //如果这个网格中没有干员，那么保持移动
        {
            spriteRenderer.flipX = true;
            transform.Translate(-(new Vector2(1.33f, 0) * (Time.deltaTime / 1)) * speed); //每秒位移量
        }
        else if(currGrid.HaveOfficial && hp > 0) //如果这个网格之中有干员，那么攻击网格中的干员
        {
            State = EnemyState.Attack;
            return;
        }
        return;
    }

    protected virtual void CheckGrid() //此函数用于检测我所在的格子
    {
        currGrid = GridManager.Instance.GetGridByWorldPos(transform.position);
    }

    protected virtual void Attack(OfficialBase official) //此函数用于定义攻击相关逻辑
    {
        if (currGrid.HaveOfficial)
        {
            if (transform.position.x < currGrid.CurrOfficialBase.transform.position.x) //如果源石虫位置在干员后方，则转身
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
        if (!currGrid.HaveOfficial) //如果这个网格中没有干员，那么保持移动
        {
            attackTimer = 0.35f;
            attackTime = 0.75f;
            State = EnemyState.Walk;
            return;
        }
    }

    private void LineNumCheckFor0_1() //此函数用于判断敌人在哪一行
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

    private void ArrangeNumCheckFor0_1() //此函数用于判断敌人在哪一列
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

    public void Hurt(float hurtValue, int hurtType) //此函数定义干员受伤函数，需要获取敌人的攻击力与攻击类型，0为物理1为法术2为真实,且同时出发受击发红特效
    {
        switch (hurtType)
        {
            case 0:
                if(hurtValue > physicalDefense)
                {
                    hp -= (hurtValue - physicalDefense);
                    //print("造成了" + (hurtValue - physicalDefense) + "点物理伤害");
                }
                else
                {
                    //print("造成了0点物理伤害");
                }
                StartCoroutine(TransitionColor());
                break;
            case 1:
                hp -= (hurtValue / (1 - spellDefense));
                StartCoroutine(TransitionColor());
                //print("造成了" + (hurtValue / (1 - spellDefense)) + "点法术伤害");
                break;
            case 2:
                hp -= hurtValue;
                StartCoroutine(TransitionColor());
                //print("造成了" + hurtValue + "点真实伤害");
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
        float duration = 0.2f; // 总变化时长为 0.2 秒
        bool increasing = true; // 标记当前是否正在增加 t

        while (true)
        {
            if (increasing)
            {
                t += Time.deltaTime / duration; // t 从 0 增加到 1
                if (t >= 1f)
                {
                    t = 1f;
                    increasing = false; // 当 t 达到 1 后开始减少
                }
            }
            else
            {
                t -= Time.deltaTime / duration; // t 从 1 减少到 0
                if (t <= 0f)
                {
                    t = 0f;
                    increasing = true; // 当 t 减少到 0 后开始增加
                    yield break; // 执行完一次颜色变化后结束协程
                }
            }

            spriteRenderer.material.color = Color.Lerp(startColor, targetColor, t);

            yield return null;
        }
    }

    public void Dead()
    {
        animator.Play("Die");
        Destroy(enemyHealthBar); //先删除血条
        EnemyManager.Instance.RemoveEnemy(this); //在敌人管理器中移除这个敌人
        LVManager.Instance.currEnemyNum++; //lv管理器中已击败敌人数加一
        Invoke("DestroyGameobject", 1.2f); // 在1.2s后销毁对象
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

    protected void CheckOrder() //此函数定义显示层级
    {
        //敌人在哪条线路上，越靠近0越大,反之越小
        //1层大于2层
        spriteRenderer.sortingOrder = (5 - lineNum) * (arrangeNum + 1);
    }
}
