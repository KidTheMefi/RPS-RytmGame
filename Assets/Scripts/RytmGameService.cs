using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RytmGameService : MonoBehaviour
{
    [SerializeField] private GameObject railPrefab;
    public List<SpawnProperty> spawnProperties = new List<SpawnProperty>();
    Rail railScript;
    //IconFabric iconFabric;
 
    void Start()
    {
        railPrefab = Instantiate(railPrefab, Vector3.up*7 , Quaternion.identity);
        railScript = railPrefab.GetComponent<Rail>();

        railScript.SpawnStart(spawnProperties[0]);
        railScript.SpawnStart(spawnProperties[1]);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
