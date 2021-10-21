using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="New Enemy", menuName = "Enemy")]
public class Enemy : ScriptableObject
{
    public string enemyName = "default name";
    public int HP = 0;
    public List<SpawnProperty> spawnProperties = new List<SpawnProperty>();
}
