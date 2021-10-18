using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum IconClass
{
    Sword,
    Shield,
    Arrow
}

public class Icon : MonoBehaviour
{   
    public IconClass iconClass;
    public int id;
}
