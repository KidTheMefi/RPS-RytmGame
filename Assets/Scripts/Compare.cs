using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Compare : MonoBehaviour
{
    IconBaseClass playerIcon;

    public delegate void Damage(int damage); 

    public static event Damage PlayerDamage;
    public static event Damage EnemyDamage;
    public static event Damage NoneDamage;

    private int enemySwordAttack = 1;
    private int enemyShieldAttack = 1;
    private int enemyArrowAttack = 1 ;

    private int playerSwordAttack = 1;
    private int playerShieldAttack = 1;
    private int playerArrowAttack = 1; 

    private bool CheckInput (int playerI)  
    {
        if (playerI >= 0 && playerI < 3)
        {
            playerIcon = (IconBaseClass)playerI;
            return true;
        }
        else
        {
            Debug.LogWarning("Wrong input in CompareMethod!");
            return false;
        }
    }

    public void EnemyIconSword(int playerI)
    {
        if(CheckInput(playerI))
        {
            switch (playerIcon)
            {
                case IconBaseClass.Sword:
                    NoneDamage(0);
                    break;
                case IconBaseClass.Shield:
                    PlayerDamage(-enemySwordAttack);
                    break;
                case IconBaseClass.Arrow:
                    EnemyDamage(-playerArrowAttack);
                    break;
            }
        }       
    }

    public void EnemyIconSchield(int playerI) 
    {
        if (CheckInput(playerI))
        {
            switch (playerIcon)
            {
                case IconBaseClass.Sword:
                    EnemyDamage(-playerSwordAttack);
                    break;
                case IconBaseClass.Shield:                
                    NoneDamage(0);
                    break;
                case IconBaseClass.Arrow:
                    PlayerDamage(-enemyShieldAttack);
                    break;
            }
        }
    }

    public void EnemyIconArrow(int playerI)
    {
        if (CheckInput(playerI))
        {
            switch (playerIcon)
            {
                case IconBaseClass.Sword:
                    PlayerDamage(-enemyArrowAttack);
                    break;
                case IconBaseClass.Shield:
                    EnemyDamage(-playerShieldAttack);
                    break;
                case IconBaseClass.Arrow:
                    NoneDamage(0);                
                    break;
            }
        }
    }
}
