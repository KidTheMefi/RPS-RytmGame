using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompletedLevelData 
{

    public event Action<string> SaveCompleteLevelData = delegate { };
    public event Action CompletedLevelDataReady = delegate { }; 

    CompletedLevelStarData levelStarData = new CompletedLevelStarData();

    public int[] currentLevelStarArray;  

    private class CompletedLevelStarData
    {
        public int[] loadedLevelStarArray;
        public int maxCompletedLevel = 0;
    }


    public void SetLevelsArrayLenght(int enemyCount)
    {
        currentLevelStarArray = new int[enemyCount];
    }

    public void SetUpNewCompleteLevelData(int enemyCount)
    {
        currentLevelStarArray = new int[enemyCount];
        PlayerPrefs.SetInt("LevelUnlocked", 0);
        levelStarData.maxCompletedLevel = 0 ;
        Debug.Log("SetUpNewCompleteLevelData");
        SaveToPlayFab();
        CompletedLevelDataReady();
    }
 
    public void LoadFromPlayFab(string comletedLevelString)   
    {   
        //Debug.Log("Test load  " + comletedLevelString);
        levelStarData = JsonUtility.FromJson<CompletedLevelStarData>(comletedLevelString);

        PlayerPrefs.SetInt("LevelUnlocked", levelStarData.maxCompletedLevel);

        if (currentLevelStarArray.Length <= levelStarData.loadedLevelStarArray.Length)
        {
            //Debug.Log("currentLevelStarArray.Length <= levelStarData.loadedLevelStarArray.Length");
            for (int i = 0; i < currentLevelStarArray.Length; i++)
            {
                currentLevelStarArray[i] = levelStarData.loadedLevelStarArray[i];
            }
        }
        else
        {
            //Debug.Log("currentLevelStarArray.Length > levelStarData.loadedLevelStarArray.Length");
            for (int i = 0; i < currentLevelStarArray.Length; i++)
            {
                if (i < levelStarData.loadedLevelStarArray.Length)
                {
                    currentLevelStarArray[i] = levelStarData.loadedLevelStarArray[i];
                }
                else
                {
                    currentLevelStarArray[i] = 0;
                }
            }
        }

        levelStarData.loadedLevelStarArray = currentLevelStarArray;

        CompletedLevelDataReady(); 
        string json = JsonUtility.ToJson(levelStarData);
        Debug.Log("Load from PlayFab " + json);
    }



    public void SaveToPlayFab()
    {
        levelStarData.loadedLevelStarArray = currentLevelStarArray;
        levelStarData.maxCompletedLevel = PlayerPrefs.GetInt("LevelUnlocked", 0);

        string json = JsonUtility.ToJson(levelStarData);
        Debug.Log("PlayFab Test save  " + json);
        SaveCompleteLevelData(json);
    }


    public void SetStarsResult(int level, int stars)
    {
        if (currentLevelStarArray[level] < stars)
        {
            currentLevelStarArray[level] = stars;
            SaveToPlayFab();
        }

        string json = JsonUtility.ToJson(levelStarData);
        //Debug.Log("Test set  " + json);
    }

    public void ResetLevels()
    {
        PlayerPrefs.SetInt("LevelUnlocked", 0);
        for (int i = 0; i < currentLevelStarArray.Length; i++)
        {
            currentLevelStarArray[i] = 0;
        }
        SaveToPlayFab();
    }



}