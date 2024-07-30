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
        healthBar = transform.Find("HealthBar").gameObject; //将血条的实体赋给healthBar
        energyBar = transform.Find("EnergyBar").gameObject; //将技力条的实体赋给energyBar

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

    private void StateUpdate() //状态初始化
    {
        anim.SetBool("isIdle", true); 
        anim.SetBool("isSkillStart", false);
        anim.SetBool("isSkillEnd", false);
    }

    private void Skill() //此函数用于定义技能持续期间操作
    {
        Invoke("TaoJinNiangAddCost", 1f);
        Invoke("TaoJinNiangAddCost", 2f);
        Invoke("TaoJinNiangAddCost", 3f);
        Invoke("TaoJinNiangAddCost", 4f);
        Invoke("TaoJinNiangAddCost", 5f);
        Invoke("TaoJinNiangAddCost", 6f);
        Invoke("TaoJinNiangAddCost", 7f);
    }

    private void SkillStateUpdate() //此函数用于控制技能的开启与关闭
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

    private void SpController() //此函数用于控制sp的增减
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

    private void IsSkillEnd() //此函数用于单独定义isSkillEnd为false，因为需要使用invoke延迟操作
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
            healthBar.GetComponent<HealthBar>().HealthBarUpdate(hp / maxHp * 0.8f); //调用血条更新函数
        }
        if(energyBar != null)
        {
            energyBar.GetComponent<EnergyBar>().EnergyBarUpdate(sp / maxSp * 0.8f); //调用技力更新函数
        }

        SpController();
        SkillStateUpdate();
    }
    public void DisableUpdate()
    {

    }
}
