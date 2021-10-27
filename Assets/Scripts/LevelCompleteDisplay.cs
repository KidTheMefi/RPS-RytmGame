using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCompleteDisplay : MonoBehaviour
{
    [SerializeField] private List<Image> starsList ;
    public Color enableColor;
    public Color disableColor;




    public void StarResults(int stars)
    {
       // Debug.Log("Changing color before cycle");
        for (int i = 0; i < starsList.Count; i++)
        {

            if (i < stars)
            {
                starsList[i].color = enableColor;
            }
            else
            {
                starsList[i].color = disableColor;
            }
            
        }
    }
}
