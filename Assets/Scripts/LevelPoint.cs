using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PointState
{
    open, closed, completed
}

public class LevelPoint : MonoBehaviour
{
    [SerializeField] private Sprite pointOpen;
    [SerializeField] private Sprite pointClosed;
    [SerializeField] private Sprite pointCompleted;

    [SerializeField] private int pointLevelNumber;
    private PointState currentState;
    private SpriteRenderer spriteRenderer;

    public event Action<int, Vector3> pointClicked = delegate { }; 


    public void SetPointState(int levelOpened)
    {
        //Debug.Log(pointLevelNumber);
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (pointLevelNumber < levelOpened)
        {
            currentState = PointState.completed;
            spriteRenderer.sprite = pointCompleted;
        }
        else if (pointLevelNumber == levelOpened)
        {
            currentState = PointState.open;
            spriteRenderer.sprite = pointOpen;
            pointClicked(pointLevelNumber, gameObject.transform.position);
        }
        else
        {
            currentState = PointState.closed;
            spriteRenderer.sprite = pointClosed;
        }
    }

    public void OnMouseDown()
    {   if (currentState != PointState.closed)
        {
            pointClicked(pointLevelNumber, gameObject.transform.position);
        }
    }
}
