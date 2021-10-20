using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail : MonoBehaviour
{
    private IconFabric iconFabric;
    [Range(0, 50)] public int itemSpeed;

    [SerializeField] private Transform endPosition;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform bottomBorder;
    [SerializeField] private Transform upperBorder;
    private List<Icon> itemsOnScene = new List<Icon>();
    [Range(0.2f, 2f)] public float countDown;
    public int repit;

    public static event Action RailClear = delegate { };
    public static event Action IconReachBottom = delegate { }; 

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
        StartCoroutine(Spawn(spawnProperty.countDown, spawnProperty.repit, spawnProperty.itemSpeed, spawnProperty.id));
    }

    private IEnumerator Spawn(float countdown, int repit, int speed, List<int> idList)
    {
        //yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < repit; i++)
        {
            Icon icon = iconFabric.Create(idList[UnityEngine.Random.Range(0, idList.Count)]);
            icon.transform.position = spawnPoint.position;
            itemsOnScene.Add(icon);
            StartCoroutine(IconMove(icon, speed));
            yield return new WaitForSeconds(countdown);
        }
        StartCoroutine(SpawnOver());
    }

    /*private IEnumerator Spawn(float countdown, int repit, int speed)
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < repit; i++)
        {
            Icon icon = iconFabric.Create(UnityEngine.Random.Range(0, 3));
            icon.transform.position = spawnPoint.position;
            itemsOnScene.Add(icon);
            StartCoroutine(ItemMove(icon, speed));
            yield return new WaitForSeconds(countdown);

        }
        StartCoroutine(SpawnOver());
    }*/

    private IEnumerator SpawnOver()
    {
        //Debug.Log("SpawnOver check started");
        while (itemsOnScene.Count != 0)
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
            yield return new WaitForSeconds(Time.deltaTime);
        }
        if (icon != null)
        {
            IconReachBottom();
            itemsOnScene.Remove(icon);
            Destroy(icon.gameObject);
        }
    }

    public int AreaCheck()
    {
        Icon targetIcon = itemsOnScene.Find(op => op.transform.localPosition.y > bottomBorder.localPosition.y && op.transform.localPosition.y < upperBorder.localPosition.y);
        if (targetIcon != null)
        {
            int i;
            switch (targetIcon.iconClass)
            {
                case IconClass.Sword:
                    i = 0;
                    break;
                case IconClass.Shield:
                    i = 1;
                    break;
                case IconClass.Arrow:
                    i = 2;
                    break;
                default:
                    i = 3;
                    break;
            }
            itemsOnScene.Remove(targetIcon);
            Destroy(targetIcon.gameObject);
            return i;
        }
        else return 3;

    }
}
