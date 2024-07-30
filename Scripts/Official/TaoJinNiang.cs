using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class TaoJinNiang : OfficialBase
{
    void Start()
    {
        officialCode = 1;
        hp = 100f;
        maxHp = 100f;
        sp = 7f;
        maxSp = 14f;
        physicalDefense = 10f;
        spellDefense = 0f;

        TransitionToDisable();
        anim = GetComponent<Animator>();
        StateUpdate();
        healthBar = transform.Find("HealthBar").gameObject; //��Ѫ����ʵ�帳��healthBar
        energyBar = transform.Find("EnergyBar").gameObject; //����������ʵ�帳��energyBar

    }



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

    private void StateUpdate() //״̬��ʼ��
    {
        anim.SetBool("isIdle", true); 
        anim.SetBool("isSkillStart", false);
        anim.SetBool("isSkillEnd", false);
    }

    private void Skill() //�˺������ڶ��弼�ܳ����ڼ����
    {
        Invoke("TaoJinNiangAddCost", 1f);
        Invoke("TaoJinNiangAddCost", 2f);
        Invoke("TaoJinNiangAddCost", 3f);
        Invoke("TaoJinNiangAddCost", 4f);
        Invoke("TaoJinNiangAddCost", 5f);
        Invoke("TaoJinNiangAddCost", 6f);
        Invoke("TaoJinNiangAddCost", 7f);
    }

    private void SkillStateUpdate() //�˺������ڿ��Ƽ��ܵĿ�����ر�
    {
        if(sp <= 0)
        {
            isSkill = false;
            anim.SetBool("isSkillStart", false);
            anim.SetBool("isSkillEnd", true);
            Invoke("IsSkillEnd", 0.1f);
        }
        else if(sp >= maxSp)
        {
            isSkill = true;
            AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.SkillStart);
            anim.SetBool("isSkillStart", true);
            Skill();
        }
    }

    private void SpController() //�˺������ڿ���sp������
    {
        if (isSkill == false)
        {
            sp += Time.deltaTime * 2;
        }
        else if (isSkill == true)
        {
            sp -= Time.deltaTime * 2;
        }
    }

    private void IsSkillEnd() //�˺������ڵ�������isSkillEndΪfalse����Ϊ��Ҫʹ��invoke�ӳٲ���
    {
        anim.SetBool("isSkillEnd", false);
    }

    private void TaoJinNiangAddCost()
    {
        UIManager.Instance.AddCost(1);
    }

    private void EnableUpdate()
    {
        if(healthBar != null)
        {
            healthBar.GetComponent<HealthBar>().HealthBarUpdate(hp / maxHp * 0.8f); //����Ѫ�����º���
        }
        if(energyBar != null)
        {
            energyBar.GetComponent<EnergyBar>().EnergyBarUpdate(sp / maxSp * 0.8f); //���ü������º���
        }

        SpController();
        SkillStateUpdate();
    }
    public void DisableUpdate()
    {

    }
}
