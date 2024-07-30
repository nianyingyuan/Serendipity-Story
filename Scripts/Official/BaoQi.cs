using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaoQi : MonoBehaviour
{
    private void Start()
    {
        Invoke("Destroy", 6f);
    }
    private void Destroy()
    {
        Destroy(gameObject);
    }
}
