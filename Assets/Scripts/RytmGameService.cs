using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    gameOn, pauze
}

public class RytmGameService : MonoBehaviour
{
    [SerializeField] private GameObject railPrefab;

    [SerializeField] private List<Enemy> enemyList;

    [SerializeField] public EnemiesList enemies;

    [SerializeField] private Canvas gameCanvas;
    [SerializeField] private Slider playerHP;
    [SerializeField] private Slider enemyHP;
    [SerializeField] private DamageShow damageShow;
    [SerializeField] private SpriteRenderer gameBackground; 

    [SerializeField] private Animator playerAnimator;
    [SerializeField] private BloodParticleFabric playerBloodFabric;
    [SerializeField] private ParticleSystem fireworkParticle;

    private EnemyModel enemyModel;

    private PointsCounter pointsCounter;

    public GameState gameStateMachine = GameState.gameOn;

    Rail railScript;

    public event Action<Dialogue> EnemyHasDialogue = delegate { };
    public event Action<int, int> LevelCompleteResults = delegate { };
    public delegate void LevelEnded();
    public event LevelEnded PlayerWin;
    public event LevelEnded PlayerLose;

    [SerializeField] private int playerMaxHP;
    private int curentLvl;

    [SerializeField] private IconButtonService iconButtonServicePrefab;
    private IconButtonService iconButtonService;

    void Start()
    {
        railPrefab = Instantiate(railPrefab, Vector3.up * -10, Quaternion.identity);
        railScript = railPrefab.GetComponent<Rail>();


        InstatiateIconButtonService();
        pointsCounter = GetComponent<PointsCounter>();

        enemyHP = Instantiate(enemyHP, gameCanvas.transform);
        playerHP = Instantiate(playerHP, gameCanvas.transform);

        enemyList = enemies.enemyList;

        railScript.NewAttackWave += NewAttackWave;
        railScript.IconReachBottom += IconReachBottom;
        railScript.ResultsChecked += ResultsApply;
        railScript.NoIconInArea += WrongTiming;
    }

    private void OnDestroy()
    {
        iconButtonService.ButtonPressed -= ButtonPressed;
        railScript.IconReachBottom -= IconReachBottom;
        railScript.ResultsChecked -= ResultsApply;
        railScript.NoIconInArea -= WrongTiming;
        railScript.NewAttackWave -= NewAttackWave;
    }


    private void InstatiateIconButtonService()
    {
        if (iconButtonService != null)
        {
            Destroy(iconButtonService.gameObject);
        }
        iconButtonService = Instantiate(iconButtonServicePrefab, gameObject.transform);
        iconButtonService.ButtonPressed += ButtonPressed;

    }

    public void StartLevel(int lvl)
    {
        if (lvl >= enemyList.Count)
        {
            Debug.LogWarning("Wrong start Lvl int!");
            return;
        }

        InstatiateIconButtonService();

       
        railScript.StopAndClearRail();

        curentLvl = lvl;

        AudioManager.Singleton.SetEnemySounds(enemyList[curentLvl].enemyAttackSounds, enemyList[curentLvl].enemyHitSounds);

        railScript.ClickAreaSetup(enemyList[curentLvl].bottomBorder, enemyList[curentLvl].areaRange);

        playerHP.maxValue = playerMaxHP;
        playerHP.value = playerMaxHP;
        enemyHP.maxValue = enemyList[curentLvl].HP;
        enemyHP.value = enemyHP.maxValue;


        if (enemyModel != null)
        {
            Destroy(enemyModel.gameObject);
        }

        if (enemyList[lvl].enemyCharacterPrefab != null)
        {
            enemyModel = Instantiate(enemyList[lvl].enemyCharacterPrefab, gameObject.transform);
        }

        if (enemyList[lvl].dialogue != null)
        {
            EnemyHasDialogue(enemyList[lvl].dialogue);
        }

        if (enemyList[lvl].enemyBackground != null)
        {
            gameBackground.sprite = enemyList[lvl].enemyBackground;
        }

        StopAllCoroutines();
        StartCoroutine(StartGame());

    }

    public void NextLevel()
    {
        if (curentLvl + 1 < enemyList.Count)
        {
            StartLevel(curentLvl + 1);
        }
        else
        {
            Debug.Log("No more enemies start last one");
            StartLevel(curentLvl);
        }
    }

    public void RestartLevel()
    {

        railScript.StopAndClearRail();

        InstatiateIconButtonService();

        playerHP.value = playerMaxHP;
        enemyHP.value = enemyHP.maxValue;

        StopAllCoroutines();
        StartCoroutine(StartGame());
    }

    private IEnumerator StartGame()
    {
        AudioManager.Singleton.PlayCombatMusic();
        yield return new WaitForSeconds(0.1f);
        gameStateMachine = GameState.gameOn;
        pointsCounter.StartCounting(enemyList[curentLvl].HP);
        
        if (enemyList.Count > 0)
        {
            railScript.LevelStart(enemyList[curentLvl]);
        }
        else Debug.LogWarning("No enemy in the List");
    }

    private void NewAttackWave()
    {
        switch (enemyList[curentLvl].ability)
        {
            case EnemySpecialAbility.changeButtonsPosition:
                iconButtonService.MoveIconButtons();
                enemyModel.AnimationSetTrigger("SpecAttack");
                break;
            default:
                return;
        }

    }

    private void IconReachBottom()
    {
        if (gameStateMachine == GameState.gameOn)
        {
            pointsCounter.AddPoints(CompareResult.Lose);
            playerAnimator.SetTrigger("Damage");
            ChangePlayerHP(-1);
        }
    }

    private void WrongTiming()
    {
        if (gameStateMachine == GameState.gameOn)
        {
            pointsCounter.AddPoints(CompareResult.Lose);
            playerAnimator.SetTrigger("Damage");
            ChangePlayerHP(-1);          
        }
    }

    public void ButtonPressed(IconBaseClass playerChoise)
    {
            railScript.AreaCheck(playerChoise);
    }

    private void PlayerAnimationPlay(IconBaseClass playerIcon)
    {
        if (gameStateMachine == GameState.gameOn)
        {
            switch (playerIcon)
            {
                case IconBaseClass.Arrow:
                    playerAnimator.SetTrigger("Arrow");
                    break;
                case IconBaseClass.Sword:
                    playerAnimator.SetTrigger("Sword");
                    break;
                case IconBaseClass.Shield:
                    playerAnimator.SetTrigger("Shield");
                    break;

            }
        }
    }


    private void ResultsApply(CompareResult compareResult, IconBaseClass playerChoise)
    {
        pointsCounter.AddPoints(compareResult);

        switch (compareResult)
        {
            case CompareResult.Win:
                PlayerAnimationPlay(playerChoise);
                AudioManager.Singleton.PlayPlayerAttackSound(playerChoise);
                ChangeEnemyHP(-1);
                break;
            case CompareResult.Lose:
                playerAnimator.SetTrigger("Damage");
                ChangePlayerHP(-1);
                break;
            case CompareResult.Draw:
                DrawSituation();
                break;
        }
    }


    private void ChangePlayerHP(int change)
    {
        if (enemyModel != null)
        {
            enemyModel.AnimationSetTrigger("Attack");
        }
        AudioManager.Singleton.PlayEnemyAttackSound();
        playerBloodFabric.CreateBlood();
        damageShow.DamageDeal();
        playerHP.value += change;
        if (playerHP.value <= 0)
        {
            LoseGame();
        }
    }

    private void ChangeEnemyHP(int change)
    {
        if (enemyModel != null)
        {
            enemyModel.AnimationSetTrigger("Damage");
            enemyModel.PlayBloodParticle();
        }

        enemyHP.value += change;
        if (enemyHP.value <= 0)
        {
            WinGame();
        }
    }

    private void DrawSituation()
    {
        AudioManager.Singleton.PlayDrawSound();
    }

    private void WinGame()
    {
        if (PlayerPrefs.GetInt("LevelUnlocked", 0) < curentLvl + 1
            && PlayerPrefs.GetInt("LevelUnlocked", 0) < enemyList.Count - 1)
        {
            PlayerPrefs.SetInt("LevelUnlocked", curentLvl + 1);
            PlayerPrefs.Save();
        }


        //Debug.Log(pointsCounter.GetPointsStarResult());
        fireworkParticle.Play();
        
        PlayerWin();
        LevelCompleteResults(curentLvl, pointsCounter.GetPointsStarResult());
        gameStateMachine = GameState.pauze;
        Debug.Log("Player Win!");

        //Debug.Log(pointsCounter.GetPoints());
    }



    private void LoseGame()
    {
        gameStateMachine = GameState.pauze;
        PlayerLose();
        Debug.Log("Enemy Win!");
    }

}
