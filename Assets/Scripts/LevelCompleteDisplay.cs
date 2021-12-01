using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCompleteDisplay : MonoBehaviour
{
    [SerializeField] private List<Image> starsList ;
    [SerializeField] private Sprite starOn;
    [SerializeField] private Sprite starOff;
    [SerializeField] private ParticleSystem testParticle;
    private List<ParticleSystem> particleList= new List<ParticleSystem>() ;


    public void StarResults(int stars)
    {
        AudioManager.Singleton.PlayWinGameSound(); 
        StartCoroutine(DisplayStarResults(stars));
        
    }

    private IEnumerator DisplayStarResults(int stars)
    {
        yield return new WaitForSecondsRealtime(0.4f);
        for (int i = 0; i < starsList.Count; i++)
        {
            
            if (i < stars)
            {
                starsList[i].sprite = starOn;
                particleList.Add(Instantiate(testParticle, starsList[i].gameObject.transform));
                AudioManager.Singleton.PlayDrawSound();
            }
            else
            {
                starsList[i].sprite = starOff;
            }
            yield return new WaitForSecondsRealtime(0.6f);
        }
        if (stars == 3)
        {
            AudioManager.Singleton.Play3StarWinSound();
        }
        
    }
    private void OnDisable()
    {
        StopAllCoroutines();
        foreach (ParticleSystem ps in particleList)
        {
            Destroy(ps.gameObject);        
        }
        particleList.Clear();
        foreach (Image star in starsList)
        {
            star.sprite = starOff;
        }
    }
}
