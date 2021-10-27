using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class LevelPropertiesDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI enemyName;
    [SerializeField] private TextMeshProUGUI enemyMoveSpeed;
    [SerializeField] private TextMeshProUGUI enemyHP;
    [SerializeField] private TextMeshProUGUI enemyArea;

    private EnemiesList enemiesList;

    [SerializeField] private List<Button> buttons;
    [SerializeField] private List<Button> levelButtons;

    public delegate void LevelSelect(int level);
    public event LevelSelect LevelSelected;
    public event LevelSelect LevelSelectedStart;

    private int selectedLevel = 0;

    
    public void ShowEnemyStats(int i)
    {
        if (i >= enemiesList.enemyList.Count)
        {
            Debug.Log("Wrong input to enemyList");
            return;
        } 

        selectedLevel = i;
        LevelSelected(selectedLevel);
        enemyName.text = enemiesList.enemyList[i].enemyName;
        enemyHP.text = "HP: " + enemiesList.enemyList[i].HP;
        enemyArea.text = "Area: " + enemiesList.enemyList[i].areaRange;
    }


    public void Start() 
    {
        GetComponentsInChildren<Button>(true, buttons);

        var selectButtons = from button in buttons
                            where button.tag == "LevelButtons"
                            select button;
        foreach (Button button in selectButtons)
        {
            levelButtons.Add(button);
        }

        AvailableLevelUpdate();
        ShowEnemyStats(0);

    }

    public void SetEnemiesList(EnemiesList enemiesList)
    {
        this.enemiesList = enemiesList;
    }

    public void AvailableLevelUpdate()
    {
        for (int i = 0; i < levelButtons.Count; i++)
        {
            
            if (i <= PlayerPrefs.GetInt("LevelUnlocked", 0))
            {
                levelButtons[i].interactable = true;              
            }
            else
            {
                levelButtons[i].interactable = false;
            }
        }
    }


    public void StartLevel()
    { 
        LevelSelectedStart(selectedLevel);
    }

}
