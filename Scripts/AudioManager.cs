using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    private void Awake()
    {
        //使其能够跨场景并且具有唯一性
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
        //从对象池获取一个音效物体
        EFAudio eF = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.EFAudio).GetComponent<EFAudio>();
        eF.Init(clip);
    }
}
