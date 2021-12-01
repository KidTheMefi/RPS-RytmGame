using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class IconButtonService : MonoBehaviour
{
    private IconButton[] iconButtons;
    [SerializeField] private List<Vector3> iconButtonsPosition;
    private List<Tween> tweens = new List<Tween>();

    public event Action<IconBaseClass> ButtonPressed = delegate { };



    void Start()
    {
     
            iconButtons = GetComponentsInChildren<IconButton>();

        SaveNewPositions();

        for (int i = 0; i < iconButtons.Length; i++)
        {
            iconButtons[i].ButtonPressed += OneButtonPressed;
        }
    }

    public void MoveIconButtons()
    {
        int j;
        for (int i = 0; i < iconButtons.Length; i++)
        {
            Tween moving;

            j = (i + 1) % iconButtons.Length;
            moving = iconButtons[i].transform.DOMove(iconButtonsPosition[j], 0.5f).SetEase(Ease.Linear);
            moving.OnComplete(SaveNewPositions);
            tweens.Add(moving);
        }

    }

    private void SaveNewPositions() 
    {
        iconButtonsPosition.Clear();
        foreach (IconButton iconButton in iconButtons)
        {
            iconButtonsPosition.Add(iconButton.gameObject.transform.position);
        }
    }

    private void OneButtonPressed(IconBaseClass iconBase)
    {
        ButtonPressed(iconBase);
    }

    private void OnDestroy()
    {
        foreach (Tween tween in tweens)
        {
            tween.Kill();
        }
        for (int i = 0; i < iconButtons.Length; i++)
        {
            iconButtons[i].ButtonPressed -= OneButtonPressed;
        }
    }

}
