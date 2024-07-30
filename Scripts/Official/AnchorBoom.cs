using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorBoom : MonoBehaviour
{
    private float timer;

    private void Start()
    {
        Quaternion rotation = Quaternion.Euler(0, 0, 90);
        transform.rotation = rotation;
    }
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 0.311f)
        {
            Destroy(gameObject);
        }
    }
}
