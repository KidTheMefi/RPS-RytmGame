using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Speach  
{
    public string name;
    public bool isEnemy;
    public Sprite portraite;
    [TextArea(2,5)]
    public string[] sentences;

}
