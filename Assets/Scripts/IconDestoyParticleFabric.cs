using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconDestoyParticleFabric : MonoBehaviour
{
    [SerializeField] private ParticleSystem boomParticlePrefab;

    [SerializeField] private Sprite particleSpriteWin;
    [SerializeField] private Sprite particleSpriteLose; 
    //[SerializeField] private Sprite particleSpriteSword; 

    public void CreateBlast(Vector3 toPosition, CompareResult compareResult) 
    {
        
        if (boomParticlePrefab != null)
        {
            switch (compareResult)
            {
                case CompareResult.Win:
                    boomParticlePrefab.textureSheetAnimation.SetSprite(0, particleSpriteWin);
                    Instantiate(boomParticlePrefab, toPosition, Quaternion.identity);
                    break;
                case CompareResult.Draw:
                    goto case CompareResult.Win;                 
                case CompareResult.Lose:
                    //boomParticlePrefab.textureSheetAnimation.SetSprite(0, particleSpriteLose);
                    break;
            }
            
           
        }
    }
}
