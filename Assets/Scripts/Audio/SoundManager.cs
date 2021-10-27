using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

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

        Music.Source = gameObject.AddComponent<AudioSource>();

        Cutscene.Source = gameObject.AddComponent<AudioSource>();

        Stings.Source = gameObject.AddComponent<AudioSource>();
        Stings.PlayerWeaponSource = gameObject.AddComponent<AudioSource>();
        Stings.FootstepSource = gameObject.AddComponent<AudioSource>();

        Stings.Source.outputAudioMixerGroup = Stings.Mixer;
        Stings.PlayerWeaponSource.outputAudioMixerGroup = Stings.Mixer;
        Stings.FootstepSource.outputAudioMixerGroup = Stings.Mixer;
        Cutscene.Source.outputAudioMixerGroup = Cutscene.Mixer;
        Music.Source.outputAudioMixerGroup = Music.Mixer;
    }

    public static void PlayHitSfx()
    {
        if (Instance == null)
            return;

        PlaySound(Instance.Stings.Source, Instance.Stings.HitSfx);
    }

    public static void PlayHandgunSfx()
    {
        if (Instance == null)
            return;

        PlaySound(Instance.Stings.PlayerWeaponSource, Instance.Stings.HandgunSfx);
    }

    public static void PlayFootstepSfx()
    {
        if (Instance == null || !Instance.Stings.FootstepClips.Any())
            return;

        var i = Random.Range(0, Instance.Stings.FootstepClips.Length);
        var selectedClip = Instance.Stings.FootstepClips[i];
        PlaySound(Instance.Stings.FootstepSource, selectedClip);
    }

    public static void PlaySelfDestructVoiceLine()
    {
        if (Instance == null)
            return;

        PlaySound(Instance.Cutscene.Source, Instance.Cutscene.SelfDestruct);
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
        public AudioSource PlayerWeaponSource;
        public AudioSource FootstepSource;

        public AudioClip HitSfx;
        public AudioClip HandgunSfx;

        public AudioClip[] FootstepClips;
    }

    [Serializable]
    public class CutsceneClips
    {
        public AudioMixerGroup Mixer;
        public AudioSource Source;

        public AudioClip SelfDestruct;
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
