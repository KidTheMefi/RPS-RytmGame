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

    private int selectedLevel = 0;

    private SaveLoadStars saveLoadStars = new SaveLoadStars();

    LevelStarData levelStarData = new LevelStarData();

    private class LevelStarData
    {
        public int[] levelStarArray;
    }

    private void Start()
    {

        levelStarData.levelStarArray = new int[rytmGameService.enemies.enemyList.Count];

        saveLoadStars.SetUp(rytmGameService.enemies.enemyList.Count);

        if (File.Exists(Application.dataPath + "/StarSaveFile.json"))
        {
            //LoadStars();
        }

        string json = JsonUtility.ToJson(levelStarData);
        Debug.Log(json);
        SelectLevel(0);

        rytmGameService.LevelCompleteResults += SetStarsResult;
        rytmGameService.PlayerWin += AvailableLevelUpdate;
        rytmGameService.PlayerWin += OpenLevelCompleteMenu;
        rytmGameService.PlayerLose += OpenLevelLoseMenu;
    }

    public void SelectLevel (int level)
    {
        selectedLevel = level;
        levelSelectionDisplay.ShowEnemyStats(level);
        ShowStarsLevel(level);
    }

    private void ShowStarsLevel(int level)
    {
        selectedLevelStarsDisplay.StarResults(levelStarData.levelStarArray[level]);
    }

    private void OnDestroy()
    {
        rytmGameService.LevelCompleteResults -= SetStarsResult;
        rytmGameService.PlayerWin -= AvailableLevelUpdate;
        rytmGameService.PlayerWin -= OpenLevelCompleteMenu;
        rytmGameService.PlayerLose -= OpenLevelLoseMenu;
    }


    public void StartLevel()
    {
        rytmGameService.StartLevel(selectedLevel);
    }

    private void LoadStars()
    {
        saveLoadStars.LoadStars();
        string json = File.ReadAllText(Application.dataPath + "/StarSaveFile.json");
        Debug.Log(json);
        
        levelStarData = JsonUtility.FromJson<LevelStarData>(json);
    }

    private void SaveStars()
    {
        saveLoadStars.SaveStars();
        string json = JsonUtility.ToJson(levelStarData);
        Debug.Log(json);
        File.WriteAllText(Application.dataPath + "/StarSaveFile.json", json);
    }

    public void ResetLevels()
    {
        saveLoadStars.ResetLevels();

        PlayerPrefs.SetInt("LevelUnlocked", 0);
        SelectLevel(0);
        AvailableLevelUpdate();

        for (int i = 0; i < levelStarData.levelStarArray.Length; i++ )
        {
            levelStarData.levelStarArray[i] = 0;
        }

        SaveStars();

        ShowStarsLevel(0);
        levelSelectionDisplay.ShowEnemyStats(0);
    }

    private void AvailableLevelUpdate ()
    {
        levelSelectionDisplay.AvailableLevelUpdate();
    }

    private void SetStarsResult(int level, int stars)
    {
        if (levelStarData.levelStarArray[level] < stars)
        {
            levelStarData.levelStarArray[level] = stars;
            SaveStars();
        }

        saveLoadStars.SetStarsResult(level, stars);

        string json = JsonUtility.ToJson(levelStarData);
        Debug.Log(json);

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
