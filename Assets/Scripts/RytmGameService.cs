using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RytmGameService : MonoBehaviour
{
    [SerializeField] private GameObject railPrefab;
    public List<SpawnProperty> spawnProperties = new List<SpawnProperty>();

    [SerializeField, Range(0, 13)] private float bottomBorder;
    [SerializeField, Range(1, 13)] private float areaRange;

    [SerializeField] private Slider playerHP;
    [SerializeField] private Slider enemyHP;

    Rail railScript;
    Compare compareScript;

    public int playerMaxHP;
    public int enemyMaxHP;
     
    private int wave = 0;

    void Start()
    {

        railPrefab = Instantiate(railPrefab, Vector3.up * -3, Quaternion.identity);
        railScript = railPrefab.GetComponent<Rail>();
        railScript.ClickAreaSetup(bottomBorder, areaRange);

        playerHP.maxValue = playerMaxHP;
        playerHP.value = playerMaxHP;
        enemyHP.maxValue = enemyMaxHP;
        enemyHP.value = enemyMaxHP;

        compareScript = GetComponent<Compare>();

        Rail.IconReachBottom += IconReachBottom;
        Rail.RailClear += NextWave;
        Compare.EnemyDamage += ChangeEnemyHP;
        Compare.PlayerDamage += ChangePlayerHP;
        Compare.NoneDamage += DrawSituation;


 
        if (spawnProperties.Count > 0)
        {
            railScript.SpawnStart(spawnProperties[wave]);
        }
        else Debug.LogWarning("No spawnProperties in the List");

    }

    public void NextWave()
    {
        wave++;
        if (wave >= spawnProperties.Count)
        {
            wave = 0;
        }
        Debug.Log("Next wave " + wave);
        railScript.SpawnStart(spawnProperties[wave]);
    }

    private void OnDestroy()
    {
        Rail.RailClear -= NextWave;
        Rail.IconReachBottom -= IconReachBottom;
    }

    private void IconReachBottom()
    {
        ChangePlayerHP(-1);
    }

    public void ButtonPressed(int playerChoise)
    {
        switch (railScript.AreaCheck())
        {
            case 0:
                compareScript.EnemyIconSword(playerChoise);
                //Debug.Log("Sword");
                break;
            case 1:
                compareScript.EnemyIconSchield(playerChoise);
                //Debug.Log("Schield");
                break;
            case 2:
                compareScript.EnemyIconArrow(playerChoise);
                //Debug.Log("Arrow");
                break;
            default:
                ChangePlayerHP(-1);
                Debug.Log("Not in Area");
                break;
        }
    }

    public void ChangePlayerHP(int change)
    {
        playerHP.value +=  change;
        if (playerHP.value <= 0)
        {
            LoseGame();
        }
    }

    public void ChangeEnemyHP(int change)
    {
        enemyHP.value += change;
        if (enemyHP.value <= 0)
        {
            WinGame();
        }
    }

    public void DrawSituation(int change)
    {
        Debug.Log("DRAW!");
    }

    public void WinGame()
    {
        Debug.Log("Player Win!");
    }

    public void LoseGame()
    {
        Debug.Log("Enemy Win!");
    }



}
