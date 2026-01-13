using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Serializable]
    public struct Sfx
    {
        public string name;
        public AudioClip audioFile;
    }

    [Serializable]
    public struct Music
    {
        public string name;
        public AudioClip audioFile;
    }

    [Header("sound dictionary's")] public Sfx[] sfx;
    public Music[] music;
    public static SoundManager instance;
    [Header("sound dictionary's")] public AudioSource sfxSource;
    public AudioSource musicSource;

    public Dictionary<string, AudioClip> MusicDictionary = new Dictionary<string, AudioClip>();
    public Dictionary<string, AudioClip> SfxDictionary = new Dictionary<string, AudioClip>();

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (Sfx s in sfx)
        {
            SfxDictionary.Add(s.name, s.audioFile);
        }

        foreach (Music m in music)
        {
            MusicDictionary.Add(m.name, m.audioFile);
        }
    }

    public void PlaySfx(string audioName)
    {
        sfxSource.PlayOneShot(SfxDictionary[audioName]);
    }

    public void SwitchMusic(string audioName)
    {
        musicSource.clip = MusicDictionary[audioName];
        musicSource.Play();
    }
}