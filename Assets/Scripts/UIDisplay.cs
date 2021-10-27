using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class UIDisplay : MonoBehaviour
{
    //[SerializeField] private GameObject LvlSelectMenu;

    [SerializeField] private GameObject LevelCompletePanel;
    [SerializeField] private GameObject LevelLosePanel;
    [SerializeField] private RytmGameService rytmGameService;
    [SerializeField] private LevelCompleteDisplay levelCompleteDisplay;
    [SerializeField] private LevelPropertiesDisplay levelSelectionDisplay;
    [SerializeField] private LevelCompleteDisplay selectedLevelStarsDisplay;


    LevelStarData levelStarData = new LevelStarData();

    private class LevelStarData
    {
        public int[] levelStarArray;

    }

    private void Awake()
    {
        levelSelectionDisplay.SetEnemiesList(rytmGameService.enemies);
    }

    private void Start()
    {    
        levelStarData.levelStarArray = new int[rytmGameService.enemies.enemyList.Count];

        if (File.Exists(Application.dataPath + "/StarSaveFile.json"))
        {
            LoadStars();
        }     

        string json = JsonUtility.ToJson(levelStarData);
        Debug.Log(json);

        rytmGameService.LevelCompleteResults += SetStarsResult;
        rytmGameService.PlayerWin += AvailableLevelUpdate;
        rytmGameService.PlayerWin += OpenLevelCompleteMenu;
        rytmGameService.PlayerLose += OpenLevelLoseMenu;
        levelSelectionDisplay.LevelSelected += ShowStarsLevel;
        levelSelectionDisplay.LevelSelectedStart += StartLevel;
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
        levelSelectionDisplay.LevelSelected -= ShowStarsLevel;
        levelSelectionDisplay.LevelSelectedStart -= StartLevel;
    }


    private void StartLevel(int i)
    {
        rytmGameService.StartLevel(i);
    }

    private void LoadStars()
    {
        string json = File.ReadAllText(Application.dataPath + "/StarSaveFile.json");
        levelStarData = JsonUtility.FromJson<LevelStarData>(json);
    }

    private void SaveStars()
    {
        string json = JsonUtility.ToJson(levelStarData);
        Debug.Log(json);
        File.WriteAllText(Application.dataPath + "/StarSaveFile.json", json);
    }

    public void ResetLevels()
    {
        PlayerPrefs.SetInt("LevelUnlocked", 0);
        AvailableLevelUpdate();

        for (int i = 0; i < levelStarData.levelStarArray.Length; i++ )
        {
            levelStarData.levelStarArray[i] = 0;
        }

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
