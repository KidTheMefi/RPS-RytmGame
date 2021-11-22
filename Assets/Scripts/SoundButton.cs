using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundButton : MonoBehaviour
{
    [SerializeField] private Image buttonImage; 
    [SerializeField] private Sprite soundOnSprite;
    [SerializeField] private Sprite soundOffSprite;


    private void SetSprite(bool soundOn) 
    {
        if (soundOn)
        {
            buttonImage.sprite = soundOnSprite;
        }
        else
        {
            buttonImage.sprite = soundOffSprite;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
      
        SetSprite(AudioManager.Singleton.soundOn);
        AudioManager.Singleton.SoundChanged += SetSprite;
    }

    private void OnDestroy()
    {
        AudioManager.Singleton.SoundChanged -= SetSprite;
    }
}
