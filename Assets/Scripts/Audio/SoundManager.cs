using System;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public AudioStings Stings;
    public CutsceneClips Cutscene;
    public MusicClips Music;

    public static SoundManager Instance;

    void Start()
    {
        if (Instance != null && Instance != this)
        {
            var old = Instance;
            Destroy(old);
        }

        Instance = this;

        Stings.Source.outputAudioMixerGroup = Stings.Mixer;
        Cutscene.Source.outputAudioMixerGroup = Cutscene.Mixer;
        Music.Source.outputAudioMixerGroup = Music.Mixer;
    }

    public static void PlayHitSfx()
    {
        if (Instance == null)
            return;

        PlaySound(Instance.Stings.Source, Instance.Stings.HitSfx);
    }

    public static void PlaySong()
    {
        if (Instance == null)
            return;

        PlaySound(Instance.Music.Source, Instance.Music.CurrentSong, true);
    }

    public static void PauseSong()
    {
        if (Instance == null)
            return;
        
        Instance.Music.Source.Pause();
    }

    public static void ResumeSong()
    {
        if (Instance == null)
            return;

        Instance.Music.Source.UnPause();
    }

    private static void PlaySound(AudioSource source, AudioClip clip, bool looping = false)
    {
        source.Stop();
        source.clip = clip;
        source.loop = looping;
        source.Play();
    }

    [Serializable]
    public class AudioStings
    {
        public AudioMixerGroup Mixer;
        public AudioSource Source;

        public AudioClip HitSfx;
    }

    [Serializable]
    public class CutsceneClips
    {
        public AudioMixerGroup Mixer;
        public AudioSource Source;

        public AudioClip Yell1;
    }

    [Serializable]
    public class MusicClips
    {
        public AudioMixerGroup Mixer;
        public AudioSource Source;
        public AudioClip CurrentSong;
    }
}
