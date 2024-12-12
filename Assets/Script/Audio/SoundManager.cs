using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource heartBeatSound;
    public AudioSource whisperSound;
    public AudioSource laughSound;
    public AudioSource breathingSound;
    public AudioSource beachWaves;
    public AudioSource beachWind;
    public AudioSource suspence;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        StopAllSounds();
    }

    public void PlayHeartBeatSound()
    {
        heartBeatSound.Play();
    }

    public void PlayWhisperSound()
    {
        whisperSound.Play();
    }
    public void PlayLaughSound()
    {
        laughSound.Play();

    }
    public void PlayBreathingSound()
    {
        breathingSound.Play();

    }
    public void StopBreathingSound()
    {
        breathingSound.Stop();

    }

    public void PlayWaveSound()
    {
        beachWaves.Play();
    }

    public void PlayWindSound()
    {
        beachWind.Play();
    }

    public void PlaySuspenceSound()
    {
        suspence.Play();
    }

    public void StopAllSounds()
    {
        heartBeatSound.Stop();
        whisperSound.Stop();
        laughSound.Stop();
        breathingSound.Stop();
        beachWaves.Stop();
        beachWind.Stop();
        suspence.Stop();
    }
}
