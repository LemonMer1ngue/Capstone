using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource heartBeatSound;
    public AudioSource whisperSound;
    public AudioSource laughSound;

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

}
