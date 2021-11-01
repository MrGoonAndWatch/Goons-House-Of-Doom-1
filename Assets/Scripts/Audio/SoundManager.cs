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
    public AudioMixer MainMixer;

    public static SoundManager Instance;

    private float _mainVolume;
    private float _sfxVolume;
    private float _bgmVolume;
    private float _voiceVolume;

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
        Stings.ExplosionSource  = gameObject.AddComponent<AudioSource>();
        Stings.MenuMoveSource = gameObject.AddComponent<AudioSource>();
        Stings.MenuSelectSource = gameObject.AddComponent<AudioSource>();

        Stings.Source.outputAudioMixerGroup = Stings.Mixer;
        Stings.PlayerWeaponSource.outputAudioMixerGroup = Stings.Mixer;
        Stings.FootstepSource.outputAudioMixerGroup = Stings.Mixer;
        Stings.ExplosionSource.outputAudioMixerGroup = Stings.Mixer;
        Stings.MenuMoveSource.outputAudioMixerGroup = Stings.Mixer;
        Stings.MenuSelectSource.outputAudioMixerGroup = Stings.Mixer;
        Cutscene.Source.outputAudioMixerGroup = Cutscene.Mixer;
        Music.Source.outputAudioMixerGroup = Music.Mixer;
    }

    public static void PlayMenuMoveSfx()
    {
        if (Instance == null)
            return;

        PlaySound(Instance.Stings.MenuMoveSource, Instance.Stings.MenuMoveClip);
    }

    public static void PlayMenuSelectSfx()
    {
        if (Instance == null)
            return;

        PlaySound(Instance.Stings.MenuSelectSource, Instance.Stings.MenuSelectClip);
    }

    public static void PlayHitSfx()
    {
        if (Instance == null)
            return;

        PlaySound(Instance.Stings.Source, Instance.Stings.HitSfx);
    }

    public static void PlayDeathSfx()
    {
        if (Instance == null)
            return;

        PlaySound(Instance.Stings.Source, Instance.Stings.DeathSfx);
    }

    public static void PlayExplosionSfx()
    {
        if (Instance == null)
            return;

        PlaySound(Instance.Stings.ExplosionSource, Instance.Stings.ExplosionSfx);
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

    public static float PlayGameStartSfx()
    {
        if (Instance == null)
            return 0;

        PlaySound(Instance.Cutscene.Source, Instance.Cutscene.GameStart);
        return Instance.Cutscene.GameStart.length;
    }

    public static void PlayAmbientSong1(bool forcePlay = true)
    {
        PlaySong(Instance.Music.AmbientSong1, forcePlay);
    }

    public static void PlayAmbientSong2(bool forcePlay = true)
    {
        PlaySong(Instance.Music.AmbientSong2, forcePlay);
    }

    public static void PlayShockSong(bool forcePlay = true)
    {
        PlaySong(Instance.Music.ShockingSong, forcePlay);
    }

    public static void PlayAmbientBossSong(bool forcePlay = true)
    {
        PlaySong(Instance.Music.BossSong, forcePlay);
    }

    public static void PlaySelfDestructSong(bool forcePlay = true)
    {
        PlaySong(Instance.Music.SelfDestructSong, forcePlay);
    }

    private static void PlaySong(AudioClip song, bool forcePlay)
    {
        if (Instance == null || 
            (!forcePlay && Instance.Music.Source.isPlaying) || 
            (Instance.Music.Source.clip != null && Instance.Music.Source.clip.GetInstanceID() == song.GetInstanceID()))
            return;

        PlaySound(Instance.Music.Source, song, true);
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

    public static void SetMainVolume(float volume)
    {
        if (Instance == null)
            return;

        if (volume <= -20.0f)
            volume = -80.0f;
        Instance._mainVolume = volume;
        if(!Instance._globallyMuted)
            Instance.MainMixer.SetFloat("Master", volume);
    }

    public static void SetSfxVolume(float volume)
    {
        if (Instance == null)
            return;

        if (volume <= -20.0f)
            volume = -80.0f;
        Instance._sfxVolume = volume;
        Instance.MainMixer.SetFloat("Stings", volume);
    }

    public static void SetBgmVolume(float volume)
    {
        if (Instance == null)
            return;

        if (volume <= -20.0f)
            volume = -80.0f;
        Instance._bgmVolume = volume;
        Instance.MainMixer.SetFloat("Music", volume);
    }

    public static void SetVoiceVolume(float volume)
    {
        if (Instance == null)
            return;

        if (volume <= -20.0f)
            volume = -80.0f;
        Instance._voiceVolume = volume;
        Instance.MainMixer.SetFloat("Voice", volume);
    }

    private bool _globallyMuted;

    public static void SetMuteAll(bool mute)
    {
        if (Instance == null)
            return;

        Instance._globallyMuted = mute;

        var targetVolume = mute ? -80.0f : Instance._mainVolume;
        Instance.MainMixer.SetFloat("Master", targetVolume);
    }

    [Serializable]
    public class AudioStings
    {
        public AudioMixerGroup Mixer;
        public AudioSource Source;
        public AudioSource ExplosionSource;
        public AudioSource PlayerWeaponSource;
        public AudioSource FootstepSource;
        public AudioSource MenuMoveSource;
        public AudioSource MenuSelectSource;

        public AudioClip HitSfx;
        public AudioClip DeathSfx;

        public AudioClip HandgunSfx;
        public AudioClip ExplosionSfx;

        public AudioClip MenuMoveClip;
        public AudioClip MenuSelectClip;

        public AudioClip[] FootstepClips;
    }

    [Serializable]
    public class CutsceneClips
    {
        public AudioMixerGroup Mixer;
        public AudioSource Source;

        public AudioClip SelfDestruct;
        public AudioClip GameStart;
    }

    [Serializable]
    public class MusicClips
    {
        public AudioMixerGroup Mixer;
        public AudioSource Source;

        public AudioClip AmbientSong1;
        public AudioClip AmbientSong2;
        public AudioClip SelfDestructSong;
        public AudioClip ShockingSong;
        public AudioClip BossSong;
    }
}
