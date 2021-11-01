using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class IconMoveData
{
    public Icon Icon; 
    public Tween Tween; 

    public IconMoveData(Icon icon, Tween tween)
    {
        Icon = icon;
        Tween = tween;
    }
}
