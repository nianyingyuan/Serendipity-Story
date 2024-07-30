using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//��Ϸ����
[CreateAssetMenu(fileName ="GameConf",menuName ="GameConf")]
public class GameConf : ScriptableObject
{
    [Tooltip("����Ԥ����")]
    public GameObject EFAudio;

    [Tooltip("��Ч")]
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


    [Tooltip("�ҽ���")]
    public GameObject TaoJinNiang;

    [Tooltip("����˹")]
    public GameObject KeLuoSi;

    [Tooltip("�ڶ��Ȱ�")]
    public GameObject Urbian;

    [Tooltip("��������")]
    public GameObject Kakarot;

    [Tooltip("Դʯ��")]
    public GameObject YuanShiChong;

    [Tooltip("ʿ��")]
    public GameObject ShiBing;

    [Tooltip("��װ")]
    public GameObject ZhongZhuang;

    [Tooltip("��ŷ")]
    public GameObject Buu;




    [Tooltip("����˹�ļ�")]
    public GameObject Keluosi_bullet;

    [Tooltip("�ڶ��Ȱ���ê")]
    public GameObject Urbian_anchor;

    [Tooltip("êը��")]
    public GameObject Anchor_Boom;

    [Tooltip("����1")]
    public GameObject BaoQi1;

    [Tooltip("����2")]
    public GameObject BaoQi2;

}
