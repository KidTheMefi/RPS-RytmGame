using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RytmGameService : MonoBehaviour
{
    [SerializeField] private GameObject railPrefab;
    public List<SpawnProperty> spawnProperties = new List<SpawnProperty>();

    public List<Enemy> enemyList = new List<Enemy>();
    
    [SerializeField, Range(0, 13)] private float bottomBorder;
    [SerializeField, Range(1, 13)] private float areaRange;
    [SerializeField] private Canvas gameCanvas;
    [SerializeField] private Slider playerHP;
    [SerializeField] private Slider enemyHP;

    Rail railScript;
    Compare compareScript;

    public int playerMaxHP;
    public int enemyMaxHP;
     
    private int wave = 0;

    void Start()
    {
        spawnProperties = enemyList[0].spawnProperties;
        enemyHP = Instantiate(enemyHP, gameCanvas.transform);
        playerHP = Instantiate(playerHP, gameCanvas.transform);
        railPrefab = Instantiate(railPrefab, Vector3.up * -3, Quaternion.identity);
        railScript = railPrefab.GetComponent<Rail>();
        railScript.ClickAreaSetup(bottomBorder, areaRange);

        playerHP.maxValue = playerMaxHP;
        playerHP.value = playerMaxHP;
        enemyHP.maxValue = enemyMaxHP;
        enemyHP.value = enemyMaxHP;


        Rail.IconReachBottom += IconReachBottom;
        Rail.RailClear += NextWave;
        Rail.ResultsChecked += ResultsApply;

        StartCoroutine(StartGame());
    }

    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(1f);
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
        IconBaseClass playerIcon;
        if (playerChoise >= 0 && playerChoise < 3)
        {
            playerIcon = (IconBaseClass)playerChoise;
            railScript.AreaCheck(playerIcon);
        }
        else
        {
            Debug.LogWarning("Wrong input in CompareMethod!");
        } 
    }



    public void ResultsApply(CompareResult compareResult)
    {
        switch (compareResult)
        {
            case CompareResult.Win:
                ChangeEnemyHP(-1);
                break;
            case CompareResult.Lose:
                ChangePlayerHP(-1);
                break;
            case CompareResult.Draw:
                DrawSituation();
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

    public void DrawSituation()
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
