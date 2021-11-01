using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail : MonoBehaviour
{
    private IconFabric iconFabric;

    [SerializeField] private Transform endPosition;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform bottomBorder;
    [SerializeField] private Transform upperBorder;

    [SerializeField]  private List<IconMoveData> iconsMoveData = new List<IconMoveData>();

    private List<SpawnProperty> enemySpawnProperties;
    private Enemy currentEnemy;
    private int wave;


    public event Action IconReachBottom = delegate { };
    public event Action NoIconInArea = delegate { };
    public event Action<CompareResult> ResultsChecked = delegate { };


    private void Awake()
    {
        iconFabric = GetComponent<IconFabric>();
    }

    public void ClickAreaSetup(float bottomPos, float upperPos)
    {
        Vector3 botPos = bottomBorder.localPosition;
        botPos.y = bottomPos;
        bottomBorder.localPosition = botPos;

        upperBorder.localPosition = bottomBorder.localPosition + Vector3.up * upperPos;

    }

    public void LevelStart(Enemy enemy)
    {
        currentEnemy = enemy;
        enemySpawnProperties = currentEnemy.spawnProperties;
        wave = 0;

        Debug.Log("Start Spawn");
        StartCoroutine(Spawn(enemySpawnProperties[wave]));
    }

    private void NextWave()
    {
        wave++;
        if (wave >= enemySpawnProperties.Count)
        {
            wave = 0;
        }
        Debug.Log("Next wave " + wave);
        StartCoroutine(Spawn(enemySpawnProperties[wave]));
    }



    private IEnumerator Spawn(SpawnProperty spawn)
    {
        for (int i = 0; i < spawn.repit; i++)
        {
            Icon icon = iconFabric.Create(spawn.idClass[UnityEngine.Random.Range(0, spawn.idClass.Count)]);
            icon.transform.position = spawnPoint.position;

            yield return new WaitForEndOfFrame();

            if (currentEnemy.hasGeneralTimeToReachBottom == true)
            {
                TweenDOMove(icon, currentEnemy.generalTimeToReachBottom);
            }
            else
            {
                TweenDOMove(icon, spawn.timeToReachBottom);
            }
            yield return new WaitForSeconds(spawn.countDown);
        }

        if (currentEnemy.hasGeneralTimeToReachBottom == true)
        {
            NextWave();
        }
        else
        {
            StartCoroutine(SpawnOver());
        }

    }

    private IEnumerator SpawnOver()
    {
        //Debug.Log("SpawnOver check started");
        while (iconsMoveData.Count != 0)
        {
            yield return new WaitForSeconds(0.1f);
        }
        NextWave();
    }


    private void TweenDOMove (Icon icon, float duration)
    {   
        Tween moving = icon.transform.DOMove(endPosition.position, duration).SetEase(Ease.Linear);
        IconMoveData iconMoveData = new IconMoveData(icon, moving);
        iconsMoveData.Add(iconMoveData);
        moving.OnComplete(() => IconMoveDataReachBottom(iconMoveData));
    }

    private void IconMoveDataReachBottom(IconMoveData iconMoveData) 
    {
        IconReachBottom();
        DestroyIconMove(iconMoveData);
    }

    private void DestroyIconMove(IconMoveData iconMove)
    {
        iconMove.Tween.Kill();       
        iconsMoveData.Remove(iconMove);
        Destroy(iconMove.Icon.gameObject);
    }


    public void AreaCheck(IconBaseClass playerIcon)
    {      
        IconMoveData targetIconMove = iconsMoveData.Find(iconMove => iconMove.Icon.transform.localPosition.y > bottomBorder.localPosition.y
                                                                  && iconMove.Icon.transform.localPosition.y < upperBorder.localPosition.y);

        if (targetIconMove != null)
        {
            ResultsChecked(targetIconMove.Icon.Compare(playerIcon));
            DestroyIconMove(targetIconMove);
        }
        else
        {
            NoIconInArea();
        }
    }

    public void StopAndClearRail()
    {
        StopAllCoroutines();

        while (iconsMoveData.Count != 0)
        {
            IconMoveData targetIconMove = iconsMoveData[0];
            iconsMoveData.Remove(targetIconMove);
            DestroyIconMove(targetIconMove);
        }
    }
}
