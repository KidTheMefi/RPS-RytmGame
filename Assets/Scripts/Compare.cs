using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Compare : MonoBehaviour
{
    IconClass playerIcon;

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
            playerIcon = (IconClass)playerI;
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
                case IconClass.Sword:
                    NoneDamage(0);
                    break;
                case IconClass.Shield:
                    PlayerDamage(-enemySwordAttack);
                    break;
                case IconClass.Arrow:
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
                case IconClass.Sword:
                    EnemyDamage(-playerSwordAttack);
                    break;
                case IconClass.Shield:                
                    NoneDamage(0);
                    break;
                case IconClass.Arrow:
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
                case IconClass.Sword:
                    PlayerDamage(-enemyArrowAttack);
                    break;
                case IconClass.Shield:
                    EnemyDamage(-playerShieldAttack);
                    break;
                case IconClass.Arrow:
                    NoneDamage(0);                
                    break;
            }
        }
    }
}
