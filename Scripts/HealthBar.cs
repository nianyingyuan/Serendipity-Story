using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public GameObject healthBar;
    public void HealthBarUpdate(float scaleX)
    {
        // 检查是否已经引用了血条物体
        if (healthBar != null)
        {
            // 获取血条物体的 Transform 组件
            Transform barTransform = healthBar.transform;

            // 修改血条物体的 x 轴缩放
            Vector3 scale = barTransform.localScale;
            scale.x = scaleX;
            barTransform.localScale = scale;
        }
        else
        {
            Debug.LogWarning("Health bar reference is not set!");
        }
    }
}
