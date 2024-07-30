using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum OfficialState //ֲ��������״̬��һ��������״̬�����Թ�����ҡ�ڣ���һ���ǽ���״̬������˵��ק�����У�
{
    Disable,
    Enable
}
//ֲ��Ļ���
public class OfficialBase : MonoBehaviour
{
    protected Animator anim; //���붯�����������Ϊanim
    protected SpriteRenderer spriteRenderer;

    public GameObject healthBar; //����һ����Ϸ��������ΪhealthBar
    public GameObject energyBar; //����һ����Ϸ��������Ϊenergybar
    public GameObject poolObj;

    public bool isSkill = false;

    public OfficialState officialState = OfficialState.Disable; //�ʼΪ����״̬

    //��ǰֲ�����ڵ���������������
    public Grid currGrid;
    public int lineNum ;
    public int arrangeNum ;

    //�����������
    public int officialCode;
    public float hp; 
    public float maxHp; 
    public float sp; 
    public float maxSp;
    public float physicalDefense;
    public float spellDefense;
    public float attackValue;

    //�����Ա�ܵ�����ʱ�ĳ�ʼ��ɫ���ܻ���ɫ
    private Color startColor = new Color(1f, 1f, 1f);  // ��ʼ��ɫΪ��ɫ
    private Color targetColor = new Color(0.96f, 0.56f, 0.57f);  // Ŀ����ɫΪ��ɫ

    protected void Find()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    //����ʱ�ĳ�ʼ��
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
            spriteRenderer.sortingOrder = 1; //ʹ��������ƶ��ĸ�Ա�����и�Ա�Ϸ�
        }
    }
    public void TransitionToDisable() //��״̬����Ϊdisable
    {
        officialState = OfficialState.Disable;
        GetComponent<Animator>().enabled = false; //���ò��Ŷ������
        hp = maxHp;
    }
    public void TransitionToEnable(Grid grid)
    {
        OfficialManager.Instance.AddOfficial(this);
        currGrid = grid;
        currGrid.CurrOfficialBase = this;
        transform.position = grid.Position;
        officialState = OfficialState.Enable;
        GetComponent<Animator>().enabled = true; //���ò��Ŷ������
                                                 //��ȡ�Լ�������������
        if (LVManager.Instance.CurrLV == 1)
        {
            LineNumCheckFor0_1(); //����Լ�������һ��
            ArrangeNumCheckFor0_1(); //����Լ�������һ��
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
        CheckOrder(); //����㼶
        //���Ÿ�Ա��������
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
    public void Hurt(float hurtValue, int hurtType) //�˺��������Ա���˺�������Ҫ��ȡ���˵Ĺ������빥�����ͣ�0Ϊ����1Ϊ����2Ϊ��ʵ,��ͬʱ�����ܻ�������Ч
    {
        switch (hurtType)
        {
            case 0:
                if(hurtValue > physicalDefense)
                {
                    hp -= (hurtValue - physicalDefense);
                    StartCoroutine(TransitionColor());
                    print("�����" + (hurtValue - physicalDefense) + "�������˺�");
                }
                else
                {
                    print(hurtValue + "<" + physicalDefense);
                }
                break;
            case 1:
                hp -= (hurtValue / (1 - spellDefense));
                StartCoroutine(TransitionColor());
                print("�����" + (hurtValue / (1 - spellDefense)) + "�㷨���˺�");
                break;
            case 2:
                hp -= hurtValue;
                StartCoroutine(TransitionColor());
                print("�����" + hurtValue + "����ʵ�˺�");
                break;
            default:
                break;
        }
        if(hp <= 0)
        {
            Dead();
        }
    }
    
    protected virtual void Dead() //�˺��������Ա��Ȼ����ʱ�Ĳ���
    {
        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.OfficialDie);
        anim.Play("Die");
        Destroy(healthBar);
        Destroy(energyBar);
        currGrid.CurrOfficialBase = null; //�ǰ����Ϊ��
        Invoke("DestroyGameobject", 1.2f); // ��1.2s�����ٶ���
        OfficialManager.Instance.RemoveOfficial(this);
    }

    public void DestroyGameobject()
    {
        Destroy(gameObject);
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

    private void ArrangeNumCheckFor0_1() //�˺��������жϵ�������һ��
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
    protected void CheckOrder() //�˺���������ʾ�㼶
    {
        //������������·�ϣ�Խ����0Խ��,��֮ԽС
        //1�����2��
        spriteRenderer.sortingOrder = (5 - lineNum) * (arrangeNum + 1);
    }
}
