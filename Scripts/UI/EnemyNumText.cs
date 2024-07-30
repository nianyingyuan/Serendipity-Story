using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyNumText : MonoBehaviour
{
    public static EnemyNumText Instance;
    private TextMeshProUGUI enemyNumText;
    // Start is called before the first frame update

    private void Awake()
    {
        Instance = this;
        enemyNumText = transform.GetComponent<TextMeshProUGUI>();
    }
    public void UpdateEnemyNumText(int currNum, int AllNum)
    {
        enemyNumText.text = (currNum + " / " + AllNum).ToString();
    }
}
