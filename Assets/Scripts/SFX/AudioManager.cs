using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
//using this tutorial "https://www.youtube.com/watch?v=g5WT91Sn3hg"
public enum SoundType
{
    JUMP,DOUBLEJUMP, DASH, SLASH,WALK,TAKE_DAMAGE, LANDED,MAIN_MUSIC, PAUSE_MUSIC, START_MUSIC, BOSS_MUSIC, BOLTS, TURRET_WALK, TURRET_DIE, TURRET_SHOOT, 
}

[RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
public class AudioManager : MonoBehaviour
{
    [SerializeField] private SoundList[] soundList;


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
        AudioClip[] clips = instance.soundList[(int)sound].Sounds;
        AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];
        instance.effectsSource.PlayOneShot(randomClip, volume);
    }

    public static void PlayMusic(SoundType sound, float volume = 1)
    {
        AudioClip[] clips = instance.soundList[(int)sound].Sounds;
        AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];
        instance.musicSource.clip = randomClip;
        instance.musicSource.volume = volume;
        instance.musicSource.Play();
    }

#if UNITY_EDITOR
    private void OnEnable()
    {
        string[] effectNames = Enum.GetNames(typeof(SoundType));
        Array.Resize(ref soundList, effectNames.Length);
        for (int i = 0; i < soundList.Length; i++)
        {
            soundList[i].name = effectNames[i];
        }

    }

#endif

    [Serializable]
    public struct SoundList
    {
        public AudioClip[] Sounds { get => sounds; }
        [HideInInspector] public string name;
        [SerializeField] private AudioClip[] sounds;
    }
}




