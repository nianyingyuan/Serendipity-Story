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

    //关卡开始时的黑幕操作
    public void TextAction()
    {
        TransparencyChange(0f);
        StartCoroutine(TextAppearDisappear(1.0f, 1.0f, 1.0f));
    }

    private void TurnToTransParentSlowly() //此函数用于调用文字消失的协程
    {
        StartCoroutine(TextSlowlyFade(1.0f)); // 1.0f 表示淡出所需的时间，可以根据需要调整
    }
    private void TurnToBlackSlowly() //此函数用于调用文字出现的协程
    {
        StartCoroutine(TextSlowlyAppear(1.0f)); // 1.0f 表示淡入所需的时间，可以根据需要调整
    }

    public void TransparencyChange(float targetColor) //此函数用于改变文字透明度
    {
        Color startColor = spriteRenderer.color; // 获取黑屏初始颜色
        Color newColor = new Color(startColor.r, startColor.g, startColor.b, targetColor);
        spriteRenderer.color = newColor;
    }
    private IEnumerator TextSlowlyFade(float fadeDuration) //此协程用于文字逐渐消失
    {
        float currentTime = 0;
        Color startColor = spriteRenderer.color; // 获取黑屏初始颜色
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 0); // 目标颜色是完全透明的

        while (currentTime < fadeDuration)
        {
            currentTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startColor.a, targetColor.a, currentTime / fadeDuration);
            Color newColor = new Color(startColor.r, startColor.g, startColor.b, alpha);
            spriteRenderer.color = newColor;
            yield return null; // 等待一帧后继续执行，实现逐帧淡出效果
        }

        spriteRenderer.color = targetColor; // 确保最终透明度达到目标值
    }
    private IEnumerator TextSlowlyAppear(float fadeDuration) //此协程用于文字逐渐出现
    {
        float currentTime = 0;
        Color startColor = spriteRenderer.color; // 获取黑屏初始颜色
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 1); // 目标颜色是完全显现的

        while (currentTime < fadeDuration)
        {
            currentTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startColor.a, targetColor.a, currentTime / fadeDuration);
            Color newColor = new Color(startColor.r, startColor.g, startColor.b, alpha);
            spriteRenderer.color = newColor;
            yield return null; // 等待一帧后继续执行，实现逐帧淡出效果
        }

        spriteRenderer.color = targetColor; // 确保最终透明度达到目标值
        if(spriteRenderer.color == targetColor)
        {
            actionComplete = true;
        }
    }

    private IEnumerator TextAppearDisappear(float appearDuration, float delayDuration, float fadeDuration)
    {
        // 先让黑屏逐渐出现
        yield return StartCoroutine(TextSlowlyAppear(appearDuration));

        // 停顿一段时间
        yield return new WaitForSeconds(delayDuration);

        // 然后让黑屏逐渐消失
        yield return StartCoroutine(TextSlowlyFade(fadeDuration));

        actionComplete = true;
    }


}
