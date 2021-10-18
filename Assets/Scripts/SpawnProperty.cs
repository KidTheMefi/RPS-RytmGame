using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SpawnProperty  
{
    [Range(0, 50)] public int itemSpeed;
    [Range(0.2f, 2f)] public float countDown;
    public int repit;
    public List<int> id = new List<int>();
}
