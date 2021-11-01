using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="New Enemy", menuName = "Enemy")]
public class Enemy : ScriptableObject
{
    public string enemyName = "default name";
    public int HP = 10;
    public bool hasGeneralTimeToReachBottom = false;
    [RangeExtension(0.4f, 5f, 0.2f)] public float generalTimeToReachBottom = 3; 
    [SerializeField, Range(0, 13)] public float bottomBorder = 0;
    [SerializeField, Range(1, 13)] public float areaRange = 4;
    public List<SpawnProperty> spawnProperties = new List<SpawnProperty>();
}
