using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconButtonService : MonoBehaviour
{
    private IconButton[] iconButtons;


    public event Action<IconBaseClass> ButtonPressed = delegate { };



    void Start()
    {
     
            iconButtons = GetComponentsInChildren<IconButton>();
            
        for (int i = 0; i < iconButtons.Length; i++)
        {
            iconButtons[i].ButtonPressed += OneButtonPressed;
        }
    }

    private void OneButtonPressed(IconBaseClass iconBase)
    {
        ButtonPressed(iconBase);
    }

    private void OnDestroy()
    {
        for (int i = 0; i < iconButtons.Length; i++)
        {
            iconButtons[i].ButtonPressed -= OneButtonPressed;
        }
    }

}
