using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SpawnProperty  
{   
    [Range(0, 50)] public int itemSpeed = 10;

    [RangeExtension(0.4f, 5f, 0.2f)] public float timeToReachBottom = 1f;
    [RangeExtension(0.25f, 2f, 0.25f)] public float countDown = 0.25f;
    public List<IconSpawnIdClass> idClass = new List<IconSpawnIdClass>();
    public int repit = 10;
    
}
