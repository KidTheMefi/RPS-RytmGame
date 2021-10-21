using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SpawnProperty  
{   
    [Range(0, 50)] public int itemSpeed;
    [RangeExtension(0.25f, 2f, 0.25f)] public float countDown;
    public List<IconSpawnIdClass> idClass = new List<IconSpawnIdClass>();
    public int repit;
    
}
