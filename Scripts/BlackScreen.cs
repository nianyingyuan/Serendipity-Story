using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


public class BlackScreen : MonoBehaviour
{
    public static BlackScreen Instance;
    private SpriteRenderer spriteRenderer;

    public bool actionComplete;
    private void Awake()
    {
        Instance = this;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void TurnToTransParentSlowly() //�˺������ڵ��ú�����ʧ��Э��
    {
        StartCoroutine(BlackScreenSlowlyFade(1.0f)); // 1.0f ��ʾ���������ʱ�䣬���Ը�����Ҫ����
    }
    private void TurnToBlackSlowly() //�˺������ڵ��ú������ֵ�Э��
    {
        StartCoroutine(BlackScreenSlowlyAppear(1.0f)); // 1.0f ��ʾ���������ʱ�䣬���Ը�����Ҫ����
    }

    public void TransparencyChange(float targetColor) //�˺������ڸı����͸����
    {
        Color startColor = spriteRenderer.color; // ��ȡ������ʼ��ɫ
        Color newColor = new Color(startColor.r, startColor.g, startColor.b, targetColor); 
        spriteRenderer.color = newColor;
    }
    private IEnumerator BlackScreenSlowlyFade(float fadeDuration) //��Э�����ں�������ʧ
    {
        actionComplete = false;
        float currentTime = 0;
        Color startColor = spriteRenderer.color; // ��ȡ������ʼ��ɫ
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 0); // Ŀ����ɫ����ȫ͸����

        while (currentTime < fadeDuration)
        {
            currentTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startColor.a, targetColor.a, currentTime / fadeDuration);
            Color newColor = new Color(startColor.r, startColor.g, startColor.b, alpha);
            spriteRenderer.color = newColor;
            yield return null; // �ȴ�һ֡�����ִ�У�ʵ����֡����Ч��
        }

        spriteRenderer.color = targetColor; // ȷ������͸���ȴﵽĿ��ֵ
        if (spriteRenderer.color == targetColor)
        {
            actionComplete = true;
        }
    }
    private IEnumerator BlackScreenSlowlyAppear(float fadeDuration) //��Э�����ں����𽥳���
    {
        float currentTime = 0;
        Color startColor = spriteRenderer.color; // ��ȡ������ʼ��ɫ
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 1); // Ŀ����ɫ����ȫ���ֵ�

        while (currentTime < fadeDuration)
        {
            currentTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startColor.a, targetColor.a, currentTime / fadeDuration);
            Color newColor = new Color(startColor.r, startColor.g, startColor.b, alpha);
            spriteRenderer.color = newColor;
            yield return null; // �ȴ�һ֡�����ִ�У�ʵ����֡����Ч��
        }

        spriteRenderer.color = targetColor; // ȷ������͸���ȴﵽĿ��ֵ
    }

    private IEnumerator BlackScreenAppearDisappear(float appearDuration, float delayDuration, float fadeDuration , UnityAction action)
    {
        // ���ú����𽥳���
        yield return StartCoroutine(BlackScreenSlowlyAppear(appearDuration));

        // ͣ��һ��ʱ��
        yield return new WaitForSeconds(delayDuration);

        // Ȼ���ú�������ʧ
        yield return StartCoroutine(BlackScreenSlowlyFade(fadeDuration));

        if (action != null) action();
    }

    //�ؿ���ʼʱ�ĺ�Ļ����
    public void StartAction(UnityAction action)
    {
        //һ��ʼ���֣�Ȼ����ʧ����ʧ���յ�ʱ���ô�������ί�з���
        StartCoroutine(BlackScreenAppearDisappear(1f,1f,1f,action));
    }
}
