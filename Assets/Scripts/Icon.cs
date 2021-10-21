using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum IconBaseClass
{
    Sword,
    Shield,
    Arrow
}

public enum IconSpawnIdClass
{
    Sword,
    Shield,
    Arrow,
    StrongSword,
    StrongShield,
    StrongArrow,
    InvertedSword,
    InvertedShield,
    InvertedArrow,
    RhythmSword,
    RhytmShield,
    RhytmArrow
}

public enum CompareResult
{
    Win,
    Draw,
    Lose
}

public abstract class Icon : MonoBehaviour
{
    public IconSpawnIdClass iconSpawnClass;
    public IconBaseClass iconBaseClass;


    public CompareResult Compare(IconBaseClass playerClass)
    {
        switch (iconBaseClass)
        {
            case IconBaseClass.Arrow:
                return CompareWithArrow(playerClass);
            case IconBaseClass.Sword:
                return CompareWithSword(playerClass);
            case IconBaseClass.Shield:
                return CompareWithSchield(playerClass);
            default: return CompareResult.Draw;
        }
    }

    protected virtual CompareResult CompareWithArrow(IconBaseClass playerClass)
    {
        switch (playerClass)
        {
            case IconBaseClass.Sword:
                return CompareResult.Lose;
            case IconBaseClass.Shield:
                return CompareResult.Win;
            case IconBaseClass.Arrow:
                return CompareResult.Draw;
            default: return CompareResult.Draw;
        }
    }

    protected virtual CompareResult CompareWithSchield(IconBaseClass playerClass)
    {
        switch (playerClass)
        {
            case IconBaseClass.Sword:
                return CompareResult.Win;
            case IconBaseClass.Shield:
                return CompareResult.Draw;
            case IconBaseClass.Arrow:
                return CompareResult.Lose;

            default: return CompareResult.Draw;
        }
    }

    protected virtual CompareResult CompareWithSword(IconBaseClass playerClass)
    {
        switch (playerClass)
        {
            case IconBaseClass.Sword:
                return CompareResult.Draw;
            case IconBaseClass.Shield:
                return CompareResult.Lose;
            case IconBaseClass.Arrow:
                return CompareResult.Win;

            default: return CompareResult.Draw;
        }
    }
}
