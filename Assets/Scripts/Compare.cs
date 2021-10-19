using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compare : MonoBehaviour
{
    IconClass playerIcon;

    private bool CheckInput (int playerI)  
    {
        if (playerI >= 0 && playerI < 3)
        {
            playerIcon = (IconClass)playerI;
            return true;
        }
        else
        {
            Debug.LogWarning("Wrong input in CompareMethod!");
            return false;
        }

    }

    public void enemyIconSword(int playerI)
    {
        if(CheckInput(playerI))
        {
            switch (playerIcon)
            {
                case IconClass.Sword:
                    break;

            }
        }
        
    }
}
