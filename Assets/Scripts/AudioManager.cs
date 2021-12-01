using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

    public static AudioManager Singleton { get; private set; }

    public event Action<bool> SoundChanged = delegate { };

    private void Awake()
    {
        if (!Singleton)
        {
            Singleton = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [SerializeField] private AudioSource backgroundMusic;
    [SerializeField] private AudioSource playerSoundEffect;
    [SerializeField] private AudioSource enemySoundEffect;


    [SerializeField] private AudioClip arrowSound;
    [SerializeField] private AudioClip[] shieldSounds;
    [SerializeField] private AudioClip[] swordSounds;
    [SerializeField] private AudioClip[] playerDamageSounds;

    [SerializeField] private AudioClip combatMusic;
    [SerializeField] private AudioClip mainMenuMusic;
    [SerializeField] private AudioClip loseMusic;
    [SerializeField] private AudioClip wellDoneSound;
    [SerializeField] private AudioClip winSound; 
    [SerializeField] private AudioClip drawSound;
    [SerializeField] private AudioClip clickUI;

    [SerializeField] private AudioMixer audioMixer;

    private AudioClip enemyAttackSound;
    private AudioClip[] enemyHitSounds;


    public bool soundOn { get; private set; }

    void Start()
    {
        if (PlayerPrefs.GetInt("Sound", 1) == 1)
        {
            soundOn = true;
            AudioManager.Singleton.SoundTurnOn();
        }
        else
        {
            soundOn = false;
            AudioManager.Singleton.SoundTurnOff();
        }

        PlayerPrefs.Save();
        SoundChanged(soundOn);
        backgroundMusic.clip = mainMenuMusic;
        backgroundMusic.Play();
    }

    public void PlayClickSound()
    {
        playerSoundEffect.clip = clickUI;
        playerSoundEffect.Play();
    }

    public void PlayMenuMusic()
    {
        if (backgroundMusic.clip != mainMenuMusic)
        {
            backgroundMusic.clip = mainMenuMusic;
            backgroundMusic.Play();
        }
    }

    public void PlayCombatMusic()
    {
        backgroundMusic.clip = combatMusic;
        backgroundMusic.Play();
    }

    public void PlayLoseMusic()
    {
        backgroundMusic.clip = loseMusic;
        backgroundMusic.Play();
    }

    public void SetEnemySounds(AudioClip attack, AudioClip[] hit)
    {
        enemyAttackSound = attack;
        enemyHitSounds = hit;
    }


    public void PlayWinGameSound()
    {
        backgroundMusic.clip = winSound;
        backgroundMusic.Play();
    }

    public void Play3StarWinSound()
    {
        playerSoundEffect.clip = wellDoneSound;
        playerSoundEffect.Play();
    }

    public void PlayDrawSound()
    {
        enemySoundEffect.clip = drawSound;
        enemySoundEffect.Play();
    }

    public void PlayPlayerAttackSound(IconBaseClass playerAttack)
    {
        switch (playerAttack)
        {
            case IconBaseClass.Arrow:
                playerSoundEffect.clip = arrowSound;
                break;
            case IconBaseClass.Sword:
                playerSoundEffect.clip = swordSounds[UnityEngine.Random.Range(0, swordSounds.Length)];
                break;
            case IconBaseClass.Shield:
                playerSoundEffect.clip = shieldSounds[UnityEngine.Random.Range(0, shieldSounds.Length)];
                break;
        }

        playerSoundEffect.Play();
        PlayEnemyHitSound();
    }

    public void PlayPlayerHitSound()
    {
        if (playerDamageSounds.Length != 0)
        {
            playerSoundEffect.clip = playerDamageSounds[UnityEngine.Random.Range(0, playerDamageSounds.Length)];
            playerSoundEffect.Play();
        }
    }


    public void PlayEnemyAttackSound()
    {
        if (enemyAttackSound != null)
        {
            enemySoundEffect.clip = enemyAttackSound;
            enemySoundEffect.Play();
        }
        PlayPlayerHitSound();
    }

    public void PlayEnemyHitSound()
    {
        if (enemyHitSounds.Length != 0)
        {
            enemySoundEffect.clip = enemyHitSounds[UnityEngine.Random.Range(0, enemyHitSounds.Length)];
            enemySoundEffect.PlayDelayed(0.15f);
        }
    }


    public void ChangeSound()
    {
        if (soundOn)
        {
            SoundTurnOff();
            PlayerPrefs.SetInt("Sound", 0);
        }
        else
        {
            SoundTurnOn();
            PlayerPrefs.SetInt("Sound", 1);
        }
        soundOn = !soundOn;
        SoundChanged(soundOn);
    }

    private void SoundTurnOn()
    {
        audioMixer.SetFloat("MasterVolume", 0);
    }

    private void SoundTurnOff()
    {
        audioMixer.SetFloat("MasterVolume", -80);
    }
}
