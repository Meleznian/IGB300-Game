using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
//using this tutorial "https://www.youtube.com/watch?v=g5WT91Sn3hg"
public enum SoundType
{
    JUMP,SLASH
}

public enum MusicType
{
    MAIN
}
[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] soundList;
    [SerializeField] private AudioClip[] musicList;
    

    public static AudioManager instance;
    private AudioSource effectsSource;
    public AudioSource musicSource;

    private void Awake()
    {
        instance = this;
    }    

    private void Start()
    {
        effectsSource = GetComponent<AudioSource>();
    }

    public static void PlayEffect(SoundType sound, float volume = 1)
    {
        instance.effectsSource.PlayOneShot(instance.soundList[(int)sound]);
    }

    public static void PlayMusic(MusicType sound, float volume = 1)
    {
        instance.musicSource.PlayOneShot(instance.musicList[(int)sound]);
    }
}



