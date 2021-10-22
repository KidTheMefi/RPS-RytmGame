using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RytmGameService : MonoBehaviour
{
    [SerializeField] private GameObject railPrefab;
    public List<SpawnProperty> spawnProperties = new List<SpawnProperty>();

    public List<Enemy> enemyList = new List<Enemy>();
    
    //[SerializeField, Range(0, 13)] private float bottomBorder;
    //[SerializeField, Range(1, 13)] private float areaRange;
    [SerializeField] private Canvas gameCanvas;
    [SerializeField] private Slider playerHP;
    [SerializeField] private Slider enemyHP;

    Rail railScript;

    public int playerMaxHP;
    private int curentLvl;
     
    private int wave = 0;

    void Start()
    {     
        railPrefab = Instantiate(railPrefab, Vector3.up * -3, Quaternion.identity);
        railScript = railPrefab.GetComponent<Rail>();

        enemyHP = Instantiate(enemyHP, gameCanvas.transform);
        playerHP = Instantiate(playerHP, gameCanvas.transform);

        Rail.IconReachBottom += IconReachBottom;
        Rail.RailClear += NextWave;
        Rail.ResultsChecked += ResultsApply;
        Rail.NoIconInArea += WrongTiming;
    }

    //public void SetLvl(int lvl)

    public void StartLevel(int lvl)
    {
        railScript.StopAndClearRail();

        if (lvl >= enemyList.Count)
        {
            Debug.LogWarning("Wrong start Lvl int!");
            return;
        }
        
            curentLvl = lvl;
            spawnProperties = enemyList[curentLvl].spawnProperties;

            railScript.ClickAreaSetup(enemyList[curentLvl].bottomBorder, enemyList[curentLvl].areaRange);

            playerHP.maxValue = playerMaxHP;
            playerHP.value = playerMaxHP;
            enemyHP.maxValue = enemyList[curentLvl].HP;
            enemyHP.value = enemyHP.maxValue;

            StartCoroutine(StartGame());
        
    }

    public void NextLevel()
    {
            StartLevel(curentLvl + 1);  
    }

    public void RestartLevel()
    {
        railScript.StopAndClearRail();
        playerHP.value = playerMaxHP;
        enemyHP.value = enemyHP.maxValue;
        wave = 0;

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

    private void NextWave()
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


    private void WrongTiming()
    {
        ChangePlayerHP(-1);
    }

    private void ResultsApply(CompareResult compareResult)
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


    private void ChangePlayerHP(int change)
    {
        playerHP.value +=  change;
        if (playerHP.value <= 0)
        {
            LoseGame();
        }
    }

    private void ChangeEnemyHP(int change)
    {
        enemyHP.value += change;
        if (enemyHP.value <= 0)
        {
            WinGame();
        }
    }

    private void DrawSituation()
    {
    }

    private void WinGame()
    {
        NextLevel();
        Debug.Log("Player Win!");
    }

    private void LoseGame()
    {
        Debug.Log("Enemy Win!");
    }

}
