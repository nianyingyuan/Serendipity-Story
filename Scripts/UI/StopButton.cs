using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopButton : MonoBehaviour
{
    public void Show(bool isShow)
    {
        gameObject.SetActive(isShow);
    }
}
