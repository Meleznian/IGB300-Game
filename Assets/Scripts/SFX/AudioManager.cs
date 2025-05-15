using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
//using this tutorial "https://www.youtube.com/watch?v=g5WT91Sn3hg"
public enum SoundType
{

}
[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] soundList;
    public AudioSource effectsSource;
    public AudioSource musicSource;

    public static AudioManager instance;
    private AudioSource audioSource;

    private void Awake()
    {
        instance = this;
    }    

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public static void PlayEffect(SoundType sound, float volume = 1)
    {
        instance.audioSource.PlayOneShot(instance.soundList[(int)sound]);
    }
}



