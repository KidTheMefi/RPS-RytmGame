using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMenu : MonoBehaviour
{
    
    private LevelPoint[] levelPoints;

    [SerializeField] private Transform pointer;

    [SerializeField] private LevelMenuStarsDisplay starsDisplay;
    [SerializeField] private LevelMenuEnemyDisplay enemyPropertieDisplay;

    public event Action<int> StartSelectedLevel = delegate { };
    public event Action BackToMainMenuEvent = delegate { }; 

    private int[] currentLevelStarArray;
    private int selectedLevel = 0;

    private void OnDestroy()
    {
        foreach (LevelPoint LP in levelPoints)
        {
            LP.pointClicked -= LevelSelected;
        }
    }

    private void LevelSelected(int selectedLevel, Vector3 selectedLevelPosition)
    {
        
        pointer.position = selectedLevelPosition;
        enemyPropertieDisplay.ShowEnemyStats(selectedLevel);
        starsDisplay.StarResults(currentLevelStarArray[selectedLevel]);

        if (this.selectedLevel != selectedLevel)
        {
            AudioManager.Singleton.PlayClickSound();
        }
        this.selectedLevel = selectedLevel;
      
    }

    public void SetUpLevelMenu(int[] array)
    {
        currentLevelStarArray = array;
        UpdateLevelMenu();
    }

    private void UpdateLevelMenu()
    {
        levelPoints = GetComponentsInChildren<LevelPoint>();
        foreach (LevelPoint LP in levelPoints)
        {
            LP.pointClicked += LevelSelected;
            LP.SetPointState(PlayerPrefs.GetInt("LevelUnlocked"));
        }

    }

    public void StartLevel()
    {
        StartSelectedLevel(selectedLevel);
        Destroy(gameObject);
    }

    public void BackToMainMenu()
    {
        BackToMainMenuEvent(); 
        Destroy(gameObject);
    }

}
