using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class UIDisplay : MonoBehaviour
{
    [SerializeField] private GameObject LevelCompletePanel;
    [SerializeField] private LevelCompleteDisplay levelCompleteDisplay;
    [SerializeField] private GameObject LevelLosePanel;
    [SerializeField] private RytmGameService rytmGameService;
  
    //[SerializeField] private LevelPropertiesDisplay levelSelectionDisplay;
    //[SerializeField] private LevelCompleteDisplay selectedLevelStarsDisplay;
    [SerializeField] private GameObject LoadingPanel;
    [SerializeField] private GameObject MainMenuPanel;
 
    [SerializeField] private LevelMenu levelSelectMenuPrefab;
    private LevelMenu levelSelectMenu;

    [SerializeField] private Canvas uiDisplayCanvas;
    [SerializeField] private Canvas gameCanvas;  

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
        //rytmGameService.PlayerWin += AvailableLevelUpdate;
        rytmGameService.PlayerWin += OpenLevelCompleteMenu;
        rytmGameService.PlayerLose += OpenLevelLoseMenu;
    }


    private void LoadingEnded()
    {

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


    private void OnDestroy()
    {

        completedLevelData.CompletedLevelDataReady -= LoadingEnded;
        completedLevelData.SaveCompleteLevelData -= SaveCompletedLevelData;
        playFabScript.CompletedLevelDataLoaded -= SetLoadedCompletedLevelData;
        playFabScript.NoCompletedLevelData -= SetUpNewComletedLevelData;


        rytmGameService.LevelCompleteResults -= SetStarsResult;
        rytmGameService.PlayerWin -= OpenLevelCompleteMenu;
        rytmGameService.PlayerLose -= OpenLevelLoseMenu;
    }

    public void StartLevel(int sl)
    {
        gameCanvas.enabled = true;
        uiDisplayCanvas.enabled = true; 
        Resume();
        rytmGameService.StartLevel(sl);
    }


    public void ResetLevels()
    {

        completedLevelData.ResetLevels();
        PlayerPrefs.SetInt("LevelUnlocked", 0);
  
    }

    private void SetStarsResult(int level, int stars)
    {
        completedLevelData.SetStarsResult(level, stars);
        

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
        rytmGameService.gameStateMachine = GameState.pauze;
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        rytmGameService.gameStateMachine = GameState.gameOn;
        Time.timeScale = 1f;
    }

    public void QuitGame()

    {
        Application.Quit();
    }

    public void OpenLevelMenu()
    {     
        levelSelectMenu = Instantiate(levelSelectMenuPrefab);
        levelSelectMenu.SetUpLevelMenu(completedLevelData.currentLevelStarArray);
        levelSelectMenu.StartSelectedLevel += StartLevel;
        levelSelectMenu.BackToMainMenuEvent += BackToMainMenu;
        //gameDisplay
        uiDisplayCanvas.enabled = false;
        gameCanvas.enabled = false;
        Pause();

    }

    public void BackToMainMenu()
    {
        uiDisplayCanvas.enabled = true;
        MainMenuPanel.SetActive(true);
    }

}
