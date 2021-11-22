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
    [SerializeField] private GameObject LoadingPanel;
    [SerializeField] private GameObject MainMenuPanel;

    [SerializeField] private LevelCompleteDisplay levelCompleteDisplay;

    [SerializeField] private RytmGameService rytmGameService;
    [SerializeField] private DialogueService dialogueService;

    [SerializeField] private LevelMenu levelSelectMenuPrefab;
    private LevelMenu levelSelectMenu;

    [SerializeField] private Canvas uiDisplayCanvas;
    [SerializeField] private Canvas gameCanvas;

    private CompletedLevelData completedLevelData = new CompletedLevelData();

    [SerializeField] private PlayFabScript playFabScript;

  

    private void Start()
    {

        completedLevelData.SetLevelsArrayLenght(rytmGameService.enemies.enemyList.Count);

        completedLevelData.CompletedLevelDataReady += LoadingEnded;
        completedLevelData.SaveCompleteLevelData += SaveCompletedLevelData;
        playFabScript.CompletedLevelDataLoaded += SetLoadedCompletedLevelData;
        playFabScript.NoCompletedLevelData += SetUpNewComletedLevelData;

        rytmGameService.LevelCompleteResults += SetStarsResult;
        rytmGameService.PlayerWin += OpenLevelCompleteMenu;
        rytmGameService.PlayerLose += OpenLevelLoseMenu;
        rytmGameService.EnemyHasDialogue += OpenDialogue;
        dialogueService.DialogueEnded += Resume;
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
        rytmGameService.EnemyHasDialogue -= OpenDialogue;
        dialogueService.DialogueEnded -= Resume;
    }

    public void StartLevel(int sl)
    {
        AudioManager.Singleton.PlayClickSound();
        gameCanvas.enabled = true;
        uiDisplayCanvas.enabled = true;
        Resume();
        rytmGameService.StartLevel(sl);
    }


    public void ResetLevels()
    {
        AudioManager.Singleton.PlayClickSound();
        completedLevelData.ResetLevels();
        PlayerPrefs.SetInt("LevelUnlocked", 0);

    }

    public void OpenAllLevels()
    {
        AudioManager.Singleton.PlayClickSound();
        PlayerPrefs.SetInt("LevelUnlocked", rytmGameService.enemies.enemyList.Count - 1);
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
        
        //audioManager.PlayClickSound();
        rytmGameService.gameStateMachine = GameState.pauze;
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        AudioManager.Singleton.PlayClickSound();
        rytmGameService.gameStateMachine = GameState.gameOn;
        Time.timeScale = 1f;
    }

    public void QuitGame()

    {
        Application.Quit();
    }

    public void OpenLevelMenu()
    {
        if (levelSelectMenu != null)
        {
            Destroy(levelSelectMenu);
        }

        AudioManager.Singleton.PlayMenuMusic();
        AudioManager.Singleton.PlayClickSound();

        levelSelectMenu = Instantiate(levelSelectMenuPrefab);
        levelSelectMenu.SetUpLevelMenu(completedLevelData.currentLevelStarArray);
        levelSelectMenu.StartSelectedLevel += StartLevel;
        levelSelectMenu.BackToMainMenuEvent += BackToMainMenu;

        uiDisplayCanvas.enabled = false;
        gameCanvas.enabled = false;
        Pause();

    }

    public void BackToMainMenu()
    {
        AudioManager.Singleton.PlayClickSound();
        AudioManager.Singleton.PlayMenuMusic();
        uiDisplayCanvas.enabled = true;
        MainMenuPanel.SetActive(true);
    }

    public void OpenDialogue(Dialogue dialogue)
    {
        Pause();
        dialogueService.gameObject.SetActive(true);
        dialogueService.StartDialogue(dialogue);

    }

    public void ChangeSound()
    {
        AudioManager.Singleton.ChangeSound();
    }
}
