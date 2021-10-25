using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Enemy List", menuName = "EnemiesList")]
public class EnemiesList : ScriptableObject
{
    public List<Enemy> enemyList = new List<Enemy>();

}
