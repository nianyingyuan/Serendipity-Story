using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBar : MonoBehaviour
{
    public GameObject energyBar;
    public void EnergyBarUpdate(float scaleX)
    {
        // ����Ƿ��Ѿ�������Ѫ������
        if (energyBar != null)
        {
            // ��ȡѪ������� Transform ���
            Transform barTransform = energyBar.transform;

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
