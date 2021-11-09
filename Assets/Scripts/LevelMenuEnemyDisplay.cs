using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;



public class LevelMenuEnemyDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI enemyName;
    //[SerializeField] private TextMeshProUGUI enemyMoveSpeed;
    [SerializeField] private TextMeshProUGUI enemyHP;
    [SerializeField] private TextMeshProUGUI enemyArea;

    [SerializeField] private EnemiesList enemiesList;

    public void ShowEnemyStats(int i)
    {
        if (i >= enemiesList.enemyList.Count)
        {
            Debug.Log("Wrong input to enemyList");
            return;
        }
        enemyName.text = enemiesList.enemyList[i].enemyName;
        enemyHP.text = "HP: " + enemiesList.enemyList[i].HP;
        enemyArea.text = "Area: " + enemiesList.enemyList[i].areaRange;
    }
}
