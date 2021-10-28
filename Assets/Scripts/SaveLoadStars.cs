using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoadStars
{

    LevelStarData levelStarData = new LevelStarData();

    public int[] currentLevelStarArray;

    private class LevelStarData
    {
        public int[] loadedLevelStarArray;
    }

    public void SetUp(int enemyCount)
    {
        currentLevelStarArray = new int[enemyCount];
        

        if (!File.Exists(Application.dataPath + "/TestStarSaveFile.json"))
        {
            levelStarData.loadedLevelStarArray = new int[enemyCount];
        }
        else
        {
            LoadStars();

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
        }
        string json = JsonUtility.ToJson(levelStarData);
        Debug.Log("Test set  " + json);

    }


    public void SetStarsResult(int level, int stars)
    {
        if (currentLevelStarArray[level] < stars)
        {
            currentLevelStarArray[level] = stars;
            SaveStars();
        }

        string json = JsonUtility.ToJson(levelStarData);
        Debug.Log("Test set  " + json);
    }

    public void LoadStars()
    {
        string json = File.ReadAllText(Application.dataPath + "/TestStarSaveFile.json");
        Debug.Log("Test load  " + json);
        levelStarData = JsonUtility.FromJson<LevelStarData>(json);
    }

    public void SaveStars()
    {
        levelStarData.loadedLevelStarArray = currentLevelStarArray;

        string json = JsonUtility.ToJson(levelStarData);
        Debug.Log("Test save  " + json);
        File.WriteAllText(Application.dataPath + "/TestStarSaveFile.json", json);
    }

    public void ResetLevels()
    {

        for (int i = 0; i < currentLevelStarArray.Length; i++)
        {
            currentLevelStarArray[i] = 0;
        }
        SaveStars();
    }

}
