using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneText : MonoBehaviour
{
    public static GameSceneText Instance;
    private SpriteRenderer spriteRenderer;

    public bool actionComplete;
    private void Awake()
    {
        Instance = this;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    //�ؿ���ʼʱ�ĺ�Ļ����
    public void TextAction()
    {
        TransparencyChange(0f);
        StartCoroutine(TextAppearDisappear(1.0f, 1.0f, 1.0f));
    }

    private void TurnToTransParentSlowly() //�˺������ڵ���������ʧ��Э��
    {
        StartCoroutine(TextSlowlyFade(1.0f)); // 1.0f ��ʾ���������ʱ�䣬���Ը�����Ҫ����
    }
    private void TurnToBlackSlowly() //�˺������ڵ������ֳ��ֵ�Э��
    {
        StartCoroutine(TextSlowlyAppear(1.0f)); // 1.0f ��ʾ���������ʱ�䣬���Ը�����Ҫ����
    }

    public void TransparencyChange(float targetColor) //�˺������ڸı�����͸����
    {
        Color startColor = spriteRenderer.color; // ��ȡ������ʼ��ɫ
        Color newColor = new Color(startColor.r, startColor.g, startColor.b, targetColor);
        spriteRenderer.color = newColor;
    }
    private IEnumerator TextSlowlyFade(float fadeDuration) //��Э��������������ʧ
    {
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
    }
    private IEnumerator TextSlowlyAppear(float fadeDuration) //��Э�����������𽥳���
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
        if(spriteRenderer.color == targetColor)
        {
            actionComplete = true;
        }
    }

    private IEnumerator TextAppearDisappear(float appearDuration, float delayDuration, float fadeDuration)
    {
        // ���ú����𽥳���
        yield return StartCoroutine(TextSlowlyAppear(appearDuration));

        // ͣ��һ��ʱ��
        yield return new WaitForSeconds(delayDuration);

        // Ȼ���ú�������ʧ
        yield return StartCoroutine(TextSlowlyFade(fadeDuration));

        actionComplete = true;
    }


}
