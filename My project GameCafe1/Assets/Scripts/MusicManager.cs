using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance { get; private set; }

    private const string MUSIC_VOLUME_PLAYER_PREFS = "MusicVolume";

    [SerializeField] private MusicClipRefsSO musicClipRefsSO;
    [SerializeField] private bool playOnStart;
    [SerializeField] private AnimationCurve fadeInCurve;
    [SerializeField] private AnimationCurve fadeoutCurve;
    private AudioSource musicSource;
    private float currentFadeInTime;
    private float currentFadeOutTime;
    private float volume;
    private Action afterStop;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        musicSource = GetComponent<AudioSource>();
        if (playOnStart)
        {
            StartMusic();
        }

        volume = PlayerPrefs.GetFloat(MUSIC_VOLUME_PLAYER_PREFS, 1f);
        musicSource.volume = volume;
    }
    private void Update()
    {
        if (currentFadeInTime > 0)
        {
            currentFadeInTime-=Time.deltaTime;
            musicSource.volume = fadeoutCurve.Evaluate(currentFadeInTime) * volume ;
            if (currentFadeInTime < 0)
            {
                musicSource.volume = 1;
            }
        }else if (currentFadeOutTime > 0)
        {
            currentFadeOutTime-=Time.deltaTime;
            musicSource.volume  = fadeInCurve.Evaluate(currentFadeOutTime) * volume;
            if(currentFadeOutTime < 0)
            {
                musicSource.volume = 0;
                musicSource.Stop();
                if(afterStop != null)
                {
                    afterStop();
                }
            }
        }
    }
    private AudioClip GetRandomMusic()
    {
        return musicClipRefsSO.music[UnityEngine.Random.Range(0,musicClipRefsSO.music.Length)];
    }
    public void StartMusic()
    {
        if (musicSource.isPlaying)
        {
            StopMusic();
            afterStop = StartMusic;
            return;
        }
        else
        {
            afterStop = null;
        }
        musicSource.clip = GetRandomMusic();
        currentFadeInTime = fadeInCurve[1].time;
        musicSource.volume = 0;
        musicSource.Play();
    }
    public void StartMusic(int musicNumber)
    {
        if (musicSource.isPlaying)
        {
            StopMusic();
            afterStop = StartMusic;
            return;
        }
        else
        {
            afterStop = null;
        }
        musicSource.clip = musicClipRefsSO.music[musicNumber];
        currentFadeInTime = fadeInCurve[1].time;
        musicSource.volume = 0;
        musicSource.Play();
    }
    public void StopMusic()
    {
        if (!musicSource.isPlaying)
        {
            Debug.LogWarning("the music is already stopped");
            return;
        }

        currentFadeOutTime = fadeoutCurve[1].time;
        currentFadeInTime = -1;
        musicSource.volume = 1;
    }
    public void SetVolume(float volume)
    {
        this.volume = volume;
        PlayerPrefs.SetFloat(MUSIC_VOLUME_PLAYER_PREFS, volume);
    }
}
