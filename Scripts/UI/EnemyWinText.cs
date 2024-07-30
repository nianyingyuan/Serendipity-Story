using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyWinText : MonoBehaviour
{
    public static EnemyWinText Instance;
    private TextMeshProUGUI enemyWinText;
    // Start is called before the first frame update

    private void Awake()
    {
        Instance = this;
        enemyWinText = transform.GetComponent<TextMeshProUGUI>();
    }
    public void UpdateEnemyWinText(int num)
    {
        enemyWinText.text = num.ToString();
    }
}
