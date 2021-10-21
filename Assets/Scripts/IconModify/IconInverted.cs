using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconInverted : Icon
{

    protected override CompareResult CompareWithArrow(IconBaseClass playerClass)
    { 
        switch (playerClass)
        {
            case IconBaseClass.Sword:
                return CompareResult.Win;
            case IconBaseClass.Shield:          
                return CompareResult.Lose;
            case IconBaseClass.Arrow:
                return CompareResult.Draw;
            default: return CompareResult.Draw;
        }
    }

    protected override CompareResult CompareWithSchield(IconBaseClass playerClass)
    {
        switch (playerClass)
        {
            case IconBaseClass.Sword:
                return CompareResult.Lose;
            case IconBaseClass.Shield:
                return CompareResult.Draw;
            case IconBaseClass.Arrow:
                return CompareResult.Win;               

            default: return CompareResult.Draw;
        }
    }

    protected override CompareResult CompareWithSword(IconBaseClass playerClass)
    {
        switch (playerClass)
        {
            case IconBaseClass.Sword:
                return CompareResult.Draw;
            case IconBaseClass.Shield:
                return CompareResult.Win;
            case IconBaseClass.Arrow:
                return CompareResult.Lose;

            default: return CompareResult.Draw;
        }
    }
}
