using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//游戏配置
[CreateAssetMenu(fileName ="GameConf",menuName ="GameConf")]
public class GameConf : ScriptableObject
{
    [Tooltip("声音预制体")]
    public GameObject EFAudio;

    [Tooltip("音效")]
    public AudioClip ButtonClick;

    public AudioClip MissionFinished;

    public AudioClip EnemyInDoor;

    public AudioClip KeLuoSiPlace;
    public AudioClip TaoJinNiangPlace;
    public AudioClip UrbianPlace;
    public AudioClip KeLuoSiAttack;
    public AudioClip UrbianAttack;
    public AudioClip KakarotAttack;
    public AudioClip UrbianSkill;
    public AudioClip UrbianAnchorBoomMusic;

    public AudioClip KakarotPlace;
    public AudioClip UrbianSkillSpeaking;
    public AudioClip SkillStart;
    public AudioClip KakarotSpeaking2;
    public AudioClip KakarotSpeaking3;
    public AudioClip Explosion;
    public AudioClip BuuPlace;
    public AudioClip BuuSkillSpeaking;
    public AudioClip BuuSkillEF;
    public AudioClip OfficialPlace;
    public AudioClip OfficialDie;
    public AudioClip Teleport;
    public AudioClip GuiPaiQiGong;


    [Tooltip("桃金娘")]
    public GameObject TaoJinNiang;

    [Tooltip("克洛斯")]
    public GameObject KeLuoSi;

    [Tooltip("乌尔比安")]
    public GameObject Urbian;

    [Tooltip("卡卡罗特")]
    public GameObject Kakarot;

    [Tooltip("源石虫")]
    public GameObject YuanShiChong;

    [Tooltip("士兵")]
    public GameObject ShiBing;

    [Tooltip("重装")]
    public GameObject ZhongZhuang;

    [Tooltip("布欧")]
    public GameObject Buu;




    [Tooltip("克洛斯的箭")]
    public GameObject Keluosi_bullet;

    [Tooltip("乌尔比安的锚")]
    public GameObject Urbian_anchor;

    [Tooltip("锚炸裂")]
    public GameObject Anchor_Boom;

    [Tooltip("爆气1")]
    public GameObject BaoQi1;

    [Tooltip("爆气2")]
    public GameObject BaoQi2;

}
