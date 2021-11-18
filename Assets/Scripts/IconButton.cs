using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class IconButton : MonoBehaviour, IPointerClickHandler
{   

    [SerializeField] private IconBaseClass buttonIcon;

    private SpriteRenderer iconButtonSprite;

    public Color regularColor;
    public Color pressedColor;

    public event Action<IconBaseClass> ButtonPressed = delegate { };

    private void Start()
    {
        iconButtonSprite = GetComponent<SpriteRenderer>();
    }


    /*private void OnMouseDown()
    {
        StopCoroutine(ChangeButtonColor());
        StartCoroutine(ChangeButtonColor());

        ButtonPressed(buttonIcon);
    }*/

    private IEnumerator ChangeButtonColor()
    {
        iconButtonSprite.color = pressedColor;
        yield return new WaitForSeconds(0.1f);
        iconButtonSprite.color = regularColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("click");
        StopCoroutine(ChangeButtonColor());
        StartCoroutine(ChangeButtonColor());

        ButtonPressed(buttonIcon);
    }
}
