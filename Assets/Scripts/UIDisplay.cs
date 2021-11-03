using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class UIDisplay : MonoBehaviour
{
    [SerializeField] private GameObject LevelCompletePanel;
    [SerializeField] private GameObject LevelLosePanel;
    [SerializeField] private RytmGameService rytmGameService;
    [SerializeField] private LevelCompleteDisplay levelCompleteDisplay;
    [SerializeField] private LevelPropertiesDisplay levelSelectionDisplay;
    [SerializeField] private LevelCompleteDisplay selectedLevelStarsDisplay;
    [SerializeField] private GameObject LoadingPanel; 

    private int selectedLevel = 0;

    private CompletedLevelData completedLevelData = new CompletedLevelData();
    //private SaveLoadStars saveLoadStars = new SaveLoadStars();
    [SerializeField] private PlayFabScript playFabScript;

    private void Start()
    {



        //saveLoadStars.SetUp(rytmGameService.enemies.enemyList.Count);
        completedLevelData.SetLevelsArrayLenght(rytmGameService.enemies.enemyList.Count);
        //playFabScript.GetCompletedLevelData();

        


        completedLevelData.CompletedLevelDataReady += LoadingEnded;
        completedLevelData.SaveCompleteLevelData += SaveCompletedLevelData;
        playFabScript.CompletedLevelDataLoaded += SetLoadedCompletedLevelData;
        playFabScript.NoCompletedLevelData += SetUpNewComletedLevelData;

        rytmGameService.LevelCompleteResults += SetStarsResult;
        rytmGameService.PlayerWin += AvailableLevelUpdate;
        rytmGameService.PlayerWin += OpenLevelCompleteMenu;
        rytmGameService.PlayerLose += OpenLevelLoseMenu;
    }


    private void LoadingEnded()
    {
        SelectLevel(0);
        AvailableLevelUpdate();
        LoadingPanel.SetActive(false);
    }

    private void SetUpNewComletedLevelData()
    {
        completedLevelData.SetUpNewCompleteLevelData(rytmGameService.enemies.enemyList.Count);
    }

    private void SetLoadedCompletedLevelData(string completedLevelDataJSON)
    {
        
        completedLevelData.LoadFromPlayFab(completedLevelDataJSON);
    }

    private void SaveCompletedLevelData(string completedLevelDataJSON)
    {
        playFabScript.SetComlpetedLevelData(completedLevelDataJSON);
    }

    public void SelectLevel (int level)
    {
        selectedLevel = level;
        levelSelectionDisplay.ShowEnemyStats(level);
        ShowStarsLevel(level);
    }

    private void ShowStarsLevel(int level)
    {
        selectedLevelStarsDisplay.StarResults(completedLevelData.currentLevelStarArray[level]);
        //selectedLevelStarsDisplay.StarResults(saveLoadStars.currentLevelStarArray[level]);
    }

    private void OnDestroy()
    {

        completedLevelData.CompletedLevelDataReady -= LoadingEnded;
        completedLevelData.SaveCompleteLevelData -= SaveCompletedLevelData;
        playFabScript.CompletedLevelDataLoaded -= SetLoadedCompletedLevelData;
        playFabScript.NoCompletedLevelData -= SetUpNewComletedLevelData;


        rytmGameService.LevelCompleteResults -= SetStarsResult;
        rytmGameService.PlayerWin -= AvailableLevelUpdate;
        rytmGameService.PlayerWin -= OpenLevelCompleteMenu;
        rytmGameService.PlayerLose -= OpenLevelLoseMenu;
    }


    public void StartLevel()
    {
        rytmGameService.StartLevel(selectedLevel);
    }

   /* private void LoadStars()
    {
        saveLoadStars.LoadStars();
    }

    private void SaveStars()
    {
        saveLoadStars.SaveStars();
    }
    */
    public void ResetLevels()
    {
        //saveLoadStars.ResetLevels();
        completedLevelData.ResetLevels();
        PlayerPrefs.SetInt("LevelUnlocked", 0);
        SelectLevel(0);
        AvailableLevelUpdate();

        ShowStarsLevel(0);
        levelSelectionDisplay.ShowEnemyStats(0);
    }

    private void AvailableLevelUpdate ()
    {
        levelSelectionDisplay.AvailableLevelUpdate();
    }

    private void SetStarsResult(int level, int stars)
    {
        completedLevelData.SetStarsResult(level, stars);
        //saveLoadStars.SetStarsResult(level, stars);

        levelCompleteDisplay.StarResults(stars);
    }

   
    private void OpenLevelCompleteMenu()
    { 
        LevelCompletePanel.SetActive(true);
        Pause();
    }

    private void OpenLevelLoseMenu()
    {
        LevelLosePanel.SetActive(true);
        Pause();
    }

    public void Pause()
    {

        Time.timeScale = 0f;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
    }

    public void QuitGame()

    {
        Application.Quit();
    }

   
}
