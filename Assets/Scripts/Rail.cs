using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail : MonoBehaviour
{
    private IconFabric iconFabric;
    [Range(0,50)] public int itemSpeed;

    [SerializeField] private Transform endPosition;
    [SerializeField] private Transform spawnPoint;

    private List<Icon> itemsOnScene = new List<Icon>();
    [Range(0.2f, 2f)] public float countDown;
    public int repit;

    private void Awake()
    {
        iconFabric = GetComponent<IconFabric>();
    }
    void Start()
    {
    
        iconFabric = GetComponent<IconFabric>();

        /*StartCoroutine(Spawn(countDown, repit, itemSpeed));*/
         
    }

    public void SpawnStart (SpawnProperty spawnProperty) 
    {
        Debug.Log("Start Spawn");
        StartCoroutine(Spawn(spawnProperty.countDown, spawnProperty.repit, spawnProperty.itemSpeed, spawnProperty.id));
    }

    private IEnumerator Spawn(float countdown, int repit, int speed, List<int> idList)
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < repit; i++)
        {
            Icon icon = iconFabric.Create(idList[Random.Range(0, idList.Count)]);
            icon.transform.position = spawnPoint.position;
            itemsOnScene.Add(icon);
            StartCoroutine(ItemMove(icon, speed));
            yield return new WaitForSeconds(countdown);

        }
    }

    private IEnumerator Spawn(float countdown, int repit, int speed)
    {
       yield return new WaitForSeconds(1f);
        for (int i = 0; i < repit; i++)
        {   
            Icon icon = iconFabric.Create(Random.Range(0,3));
            icon.transform.position = spawnPoint.position;
            itemsOnScene.Add(icon);
            StartCoroutine(ItemMove(icon, speed));
            yield return new WaitForSeconds(countdown);
            
        }
    }

    private IEnumerator ItemMove(Icon item, int speed)
    {      
        while (item.transform.position != endPosition.position)
        {
            item.transform.position = Vector3.MoveTowards(item.transform.position, endPosition.position, speed * Time.deltaTime);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        if (item != null)
        {
            itemsOnScene.Remove(item);
            Destroy(item.gameObject);
        }
        
    }

}
