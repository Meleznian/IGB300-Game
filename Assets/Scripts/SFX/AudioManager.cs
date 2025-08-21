using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
//using this tutorial "https://www.youtube.com/watch?v=g5WT91Sn3hg"
public enum SoundType
{
    JUMP,DOUBLEJUMP, DASH, SLASH,WALK,TAKE_DAMAGE, LANDED,MAIN_MUSIC, PAUSE_MUSIC, START_MUSIC, BOSS_MUSIC, BOLTS, CHARGER_WALK, CHARGER_CHARGING, CHARGER_DIE, 
    CHARGER_BLOCK, CHARGER_ATTACK, TURRET_DIE, TURRET_LOCKON, TURRET_SHOOT, CRAWLER_WALK, CRAWLER_DIE, CRAWLER_HIT, CRAWLER_ATTACK, UPGRADE_MUSIC, PLAYER_SHOOT,
    COLLECT_BOLT,ENEMY_DAMAGE, KAMIKAZE_WARNING, KAMIKAZE_EXPLODE, ENEMY_DEATH, PLAYER_DEATH, PLAYER_HEAL, STEAM_KING_THRUST, STEAM_KING_DASH, STEAM_KING_JUMP,
    STEAM_KING_LAND, STEAM_KING_CHAIN, STEAM_KING_DIE_1, STEAM_KING_DIE_2, STEAM_KING_LAUGH, STEAM_KING_SHOOT
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

        if (clips.Length == 0)
        {
            Debug.LogError("Error: Audio Clip Not Found");
            return;
        }

        int num = UnityEngine.Random.Range(0, clips.Length);
       
        AudioClip randomClip = clips[num];

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

    public static void PauseMusic()
    {
        instance.musicSource.Pause();
    }

    public static void resumeMusic()
    {
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




