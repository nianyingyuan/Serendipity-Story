using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class EFAudio : MonoBehaviour
{
    private AudioSource audioSource;
    public void Init(AudioClip clip) //����Ҫ���õ�����
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(clip);
    }
    void Update()
    {
        //�������ʹ�����֣���ô����Ԥ����
        if(audioSource.isPlaying == false)
        {
            PoolManager.Instance.PushObj(GameManager.Instance.GameConf.EFAudio,gameObject);
        }
    }
}
