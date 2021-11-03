using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RytmGameService : MonoBehaviour
{
    [SerializeField] private GameObject railPrefab;
    public List<SpawnProperty> enemySpawnProperties = new List<SpawnProperty>();

    [SerializeField] private List<Enemy> enemyList;

    [SerializeField] public EnemiesList enemies;

    [SerializeField] private Canvas gameCanvas;
    [SerializeField] private Slider playerHP;
    [SerializeField] private Slider enemyHP;

    private PointsCounter pointsCounter;
  
    Rail railScript;


    public event Action<int, int> LevelCompleteResults = delegate { };
    public delegate void LevelEnded();
    public event LevelEnded PlayerWin;
    public event LevelEnded PlayerLose;

    public int playerMaxHP;
    private int curentLvl;
    
    void Start()
    {   
        railPrefab = Instantiate(railPrefab, Vector3.up * -4, Quaternion.identity);
        railScript = railPrefab.GetComponent<Rail>();

        pointsCounter = GetComponent<PointsCounter>();

        enemyHP = Instantiate(enemyHP, gameCanvas.transform);
        playerHP = Instantiate(playerHP, gameCanvas.transform);

        enemyList = enemies.enemyList;

        railScript.IconReachBottom += IconReachBottom;
        railScript.ResultsChecked += ResultsApply;
        railScript.NoIconInArea += WrongTiming;
    }


    public void StartLevel(int lvl)
    {
        railScript.StopAndClearRail();

        if (lvl >= enemyList.Count)
        {
            Debug.LogWarning("Wrong start Lvl int!");
            return;
        }

        curentLvl = lvl;
        enemySpawnProperties = enemyList[curentLvl].spawnProperties;

        railScript.ClickAreaSetup(enemyList[curentLvl].bottomBorder, enemyList[curentLvl].areaRange);

        playerHP.maxValue = playerMaxHP;
        playerHP.value = playerMaxHP;
        enemyHP.maxValue = enemyList[curentLvl].HP;
        enemyHP.value = enemyHP.maxValue;       

        StopAllCoroutines();
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

        StopAllCoroutines();
        StartCoroutine(StartGame());
    }

    private IEnumerator StartGame()
    {
        pointsCounter.StartCounting(enemyList[curentLvl].HP);
        yield return new WaitForSeconds(0.1f);
        if (enemyList.Count > 0)
        {
            railScript.LevelStart(enemyList[curentLvl]);
        }
        else Debug.LogWarning("No enemy in the List");
    }


    private void OnDestroy()
    {
        railScript.IconReachBottom -= IconReachBottom;
        railScript.ResultsChecked -= ResultsApply;
        railScript.NoIconInArea -= WrongTiming;
    }

    private void IconReachBottom()
    {
        pointsCounter.AddPoints(CompareResult.Lose);
        ChangePlayerHP(-1);
    }

    private void WrongTiming()
    {
        pointsCounter.AddPoints(CompareResult.Lose);
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


   

    private void ResultsApply(CompareResult compareResult)
    {
        pointsCounter.AddPoints(compareResult);

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
        playerHP.value += change;
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
        if (PlayerPrefs.GetInt("LevelUnlocked", 0) < curentLvl + 1)
        {
            PlayerPrefs.SetInt("LevelUnlocked", curentLvl + 1);
            PlayerPrefs.Save();
        }


        //Debug.Log(pointsCounter.GetPointsStarResult());
        LevelCompleteResults(curentLvl, pointsCounter.GetPointsStarResult());
        PlayerWin();
        Debug.Log("Player Win!");
        //Debug.Log(pointsCounter.GetPoints());
    }



    private void LoseGame()
    {
        PlayerLose();
        Debug.Log("Enemy Win!");
    }

}
