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
    private List<Icon> iconsOnScene = new List<Icon>();

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
    void Start()
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



    private IEnumerator Spawn(SpawnProperty Spawn)
    {
        for (int i = 0; i < Spawn.repit; i++)
        {
            Icon icon = iconFabric.Create(Spawn.idClass[UnityEngine.Random.Range(0, Spawn.idClass.Count)]);
            icon.transform.position = spawnPoint.position;
            iconsOnScene.Add(icon);
            yield return new WaitForEndOfFrame();

            if (currentEnemy.hasGeneralSpeed == true)
            {
                StartCoroutine(IconMove(icon, currentEnemy.generalSpeed));
            }
            else
            {
                StartCoroutine(IconMove(icon, Spawn.itemSpeed));
            }
            yield return new WaitForSeconds(Spawn.countDown);
        }

        if (currentEnemy.hasGeneralSpeed == true)
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
        while (iconsOnScene.Count != 0)
        {
            yield return new WaitForSeconds(0.1f);
        }
        NextWave();
    }

    private IEnumerator IconMove(Icon icon, int speed)
    {

        while (icon != null && icon.transform.position != endPosition.position)
        {
            icon.transform.position = Vector3.MoveTowards(icon.transform.position, endPosition.position, speed * Time.deltaTime);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        if (icon != null)
        {
            IconReachBottom();
            iconsOnScene.Remove(icon);
            Destroy(icon.gameObject);
        }
    }

    public void AreaCheck(IconBaseClass playerIcon)
    {
        Icon targetIcon = iconsOnScene.Find(icon => icon.transform.localPosition.y > bottomBorder.localPosition.y && icon.transform.localPosition.y < upperBorder.localPosition.y);
        if (targetIcon != null)
        {

            ResultsChecked(targetIcon.Compare(playerIcon));
            iconsOnScene.Remove(targetIcon);
            Destroy(targetIcon.gameObject);
        }
        else
        {
            NoIconInArea();
        }
    }

    public void StopAndClearRail()
    {
        StopAllCoroutines();

        while (iconsOnScene.Count != 0)
        {
            Icon targetIcon = iconsOnScene[0];
            iconsOnScene.Remove(targetIcon);
            Destroy(targetIcon.gameObject);
        }
    }
}
