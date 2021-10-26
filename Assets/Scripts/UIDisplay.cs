using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDisplay : MonoBehaviour
{
    //[SerializeField] private GameObject LvlSelectMenu;

    [SerializeField] private GameObject LevelCompletePanel;
    [SerializeField] private GameObject LevelLosePanel;
    [SerializeField] private RytmGameService rytmGameService;


    private void Start()
    {
        rytmGameService.PlayerWin += OpenLevelCompleteMenu;
        rytmGameService.PlayerLose += OpenLevelLoseMenu;
    }

    private void OnDestroy()
    {
        rytmGameService.PlayerWin -= OpenLevelCompleteMenu;
        rytmGameService.PlayerLose -= OpenLevelLoseMenu;
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
