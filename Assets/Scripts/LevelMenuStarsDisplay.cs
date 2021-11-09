using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMenuStarsDisplay : MonoBehaviour
{
    [SerializeField] private List<SpriteRenderer> starsList;
    [SerializeField] private Sprite starOn;
    [SerializeField] private Sprite starOff;
   
    public void StarResults(int stars)
    {
        // Debug.Log("Changing color before cycle");
        for (int i = 0; i < starsList.Count; i++)
        {

            if (i < stars)
            {
                starsList[i].sprite = starOn;
            }
            else
            {
                starsList[i].sprite = starOff;
            }

        }
    }
}
