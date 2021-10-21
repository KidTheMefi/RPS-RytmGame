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


    public static event Action RailClear = delegate { };
    public static event Action IconReachBottom = delegate { };

    public delegate void ResultsHandler(CompareResult result);
    public static event ResultsHandler ResultsChecked; 

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

    public void SpawnStart(SpawnProperty spawnProperty)
    {
        Debug.Log("Start Spawn");
        StartCoroutine(Spawn(spawnProperty.countDown, spawnProperty.repit, spawnProperty.itemSpeed, spawnProperty.idClass));
    }

    private IEnumerator Spawn(float countdown, int repit, int speed, List<IconSpawnIdClass> idClassList)
    {
        for (int i = 0; i < repit; i++)
        {
            Icon icon = iconFabric.Create(idClassList[UnityEngine.Random.Range(0, idClassList.Count)]);
            icon.transform.position = spawnPoint.position;
            iconsOnScene.Add(icon);
            yield return new WaitForEndOfFrame();
            StartCoroutine(IconMove(icon, speed));
            yield return new WaitForSeconds(countdown);
        }
        StartCoroutine(SpawnOver());
    }



    private IEnumerator SpawnOver()
    {
        //Debug.Log("SpawnOver check started");
        while (iconsOnScene.Count != 0)
        {
            yield return new WaitForSeconds(0.1f);
        }
        RailClear();
    }

    private IEnumerator IconMove(Icon icon, int speed)
    {

        while (icon != null && icon.transform.position != endPosition.position )
        {
            icon.transform.position = Vector3.MoveTowards(icon.transform.position, endPosition.position, speed * Time.deltaTime);
            Debug.Log($"{icon.ToString()} {Time.deltaTime}");
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
    }
}
