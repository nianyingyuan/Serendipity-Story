using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum OfficialState //植物有两个状态，一个是正常状态（可以攻击和摇摆），一个是禁用状态（比如说拖拽过程中）
{
    Disable,
    Enable
}
//植物的基类
public class OfficialBase : MonoBehaviour
{
    protected Animator anim; //引入动画组件，命名为anim
    protected SpriteRenderer spriteRenderer;

    public GameObject healthBar; //定义一个游戏物体命名为healthBar
    public GameObject energyBar; //定义一个游戏物体命名为energybar
    public GameObject poolObj;

    public bool isSkill = false;

    public OfficialState officialState = OfficialState.Disable; //最开始为禁用状态

    //当前植物所在的网格与行数列数
    public Grid currGrid;
    public int lineNum ;
    public int arrangeNum ;

    //定义各种属性
    public int officialCode;
    public float hp; 
    public float maxHp; 
    public float sp; 
    public float maxSp;
    public float physicalDefense;
    public float spellDefense;
    public float attackValue;

    //定义干员受到攻击时的初始颜色与受击颜色
    private Color startColor = new Color(1f, 1f, 1f);  // 开始颜色为白色
    private Color targetColor = new Color(0.96f, 0.56f, 0.57f);  // 目标颜色为橙色

    protected void Find()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    //创建时的初始化
    public void InitForCreate(bool inGrid)
    {
        Find();
        if (inGrid)
        {
            spriteRenderer.sortingOrder = -1;
            spriteRenderer.color = new Color(1, 1, 1, 0.6f);
        }
        else
        {
            spriteRenderer.sortingOrder = 1; //使跟随鼠标移动的干员在所有干员上方
        }
    }
    public void TransitionToDisable() //将状态更换为disable
    {
        officialState = OfficialState.Disable;
        GetComponent<Animator>().enabled = false; //禁用播放动画组件
        hp = maxHp;
    }
    public void TransitionToEnable(Grid grid)
    {
        OfficialManager.Instance.AddOfficial(this);
        currGrid = grid;
        currGrid.CurrOfficialBase = this;
        transform.position = grid.Position;
        officialState = OfficialState.Enable;
        GetComponent<Animator>().enabled = true; //启用播放动画组件
                                                 //获取自己的行数和列数
        if (LVManager.Instance.CurrLV == 1)
        {
            LineNumCheckFor0_1(); //检测自己处于哪一行
            ArrangeNumCheckFor0_1(); //检测自己处于哪一列
        }
        else if (LVManager.Instance.CurrLV == 2)
        {
            LineNumCheckForUr_1();
            ArrangeNumCheckForUr_1();
        }
        else if (LVManager.Instance.CurrLV == 3)
        {
            LineNumCheckForKr_1();
            ArrangeNumCheckForKr_1();
        }
        CheckOrder(); //定义层级
        //播放干员部署语音
        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.OfficialPlace);
        if (officialCode == 1)
        {
            AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.TaoJinNiangPlace);
        }else if (officialCode == 2)
        {
            AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.KeLuoSiPlace);
        }else if(officialCode == 3)
        {
            AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.UrbianPlace);
        }else if(officialCode == 4)
        {
            AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.KakarotPlace);
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
                    StartCoroutine(TransitionColor());
                    print("造成了" + (hurtValue - physicalDefense) + "点物理伤害");
                }
                else
                {
                    print(hurtValue + "<" + physicalDefense);
                }
                break;
            case 1:
                hp -= (hurtValue / (1 - spellDefense));
                StartCoroutine(TransitionColor());
                print("造成了" + (hurtValue / (1 - spellDefense)) + "点法术伤害");
                break;
            case 2:
                hp -= hurtValue;
                StartCoroutine(TransitionColor());
                print("造成了" + hurtValue + "点真实伤害");
                break;
            default:
                break;
        }
        if(hp <= 0)
        {
            Dead();
        }
    }
    
    protected virtual void Dead() //此函数定义干员自然死亡时的操作
    {
        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.OfficialDie);
        anim.Play("Die");
        Destroy(healthBar);
        Destroy(energyBar);
        currGrid.CurrOfficialBase = null; //令当前网格为空
        Invoke("DestroyGameobject", 1.2f); // 在1.2s后销毁对象
        OfficialManager.Instance.RemoveOfficial(this);
    }

    public void DestroyGameobject()
    {
        Destroy(gameObject);
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

    private void LineNumCheckForKr_1()
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
        if ((currGrid.Position.x + 3.9) > 0.1f && (currGrid.Position.x + 3.9) < 0.8f)
        {
            arrangeNum = 0;
        }
        else if ((currGrid.Position.x + 3.9) > 1f && (currGrid.Position.x + 3.9) < 2f)
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
        else if ((currGrid.Position.x + 3.9) > 9f && (currGrid.Position.x + 3.9) < 10f)
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
    protected void HpMinimumGuarantee()
    {
        if (hp < 0)
        {
            hp = 0;
        }
    }
    protected void CheckOrder() //此函数定义显示层级
    {
        //敌人在哪条线路上，越靠近0越大,反之越小
        //1层大于2层
        spriteRenderer.sortingOrder = (5 - lineNum) * (arrangeNum + 1);
    }
}
