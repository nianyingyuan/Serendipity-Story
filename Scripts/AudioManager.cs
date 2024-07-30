using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    private void Awake()
    {
        //ʹ���ܹ��糡�����Ҿ���Ψһ��
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayEFAudio(AudioClip clip)
    {
        //�Ӷ���ػ�ȡһ����Ч����
        EFAudio eF = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.EFAudio).GetComponent<EFAudio>();
        eF.Init(clip);
    }
}
