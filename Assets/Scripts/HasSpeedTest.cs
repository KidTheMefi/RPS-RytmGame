using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class HasSpeedTest 
{
    public bool hasGeneralSpeed = false;
    [Range(0, 50)] public int generalSpeed = 10;
}
