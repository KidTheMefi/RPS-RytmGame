using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PointsCounter : MonoBehaviour
{
    int points = 0;
    int maxPoints;


    public void StartCounting(int enemyHP)
    {
        points = 0;
        maxPoints = enemyHP * 5;
    }

    public void AddPoints(CompareResult result)

    {
        switch (result)
        {
            case CompareResult.Win:
                points += 5;
                break;
            case CompareResult.Draw:
                points -= 1;
                break;
            case CompareResult.Lose:
                points -= 5;
                break;
        }
    }

    public int GetPointsStarResult() 
    {
       if (points >= maxPoints*0.85f)
        {
            return 3;
        }
       else if (points >= maxPoints*0.5f)
        {
            return 2;
        }
       else
        {
            return 1;
        }
    }

    public int GetPoints()
    {

        return points;
    }
}
