using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class EFAudio : MonoBehaviour
{
    private AudioSource audioSource;
    public void Init(AudioClip clip) //传入要调用的音乐
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(clip);
    }
    void Update()
    {
        //如果不在使用音乐，那么调用预制体
        if(audioSource.isPlaying == false)
        {
            PoolManager.Instance.PushObj(GameManager.Instance.GameConf.EFAudio,gameObject);
        }
    }
}
