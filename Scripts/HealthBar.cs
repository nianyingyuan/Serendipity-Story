using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public GameObject healthBar;
    public void HealthBarUpdate(float scaleX)
    {
        // ����Ƿ��Ѿ�������Ѫ������
        if (healthBar != null)
        {
            // ��ȡѪ������� Transform ���
            Transform barTransform = healthBar.transform;

            // �޸�Ѫ������� x ������
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
