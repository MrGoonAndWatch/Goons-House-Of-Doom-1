using UnityEngine;

public class PlaySongOnStart : MonoBehaviour
{
    private bool _playing;

    public SongType Song;
    public bool OverrideCurrentSong = false;

    void Update()
    {
        if (_playing)
            return;

        _playing = true;

        switch (Song)
        {
            case SongType.Ambient1:
                SoundManager.PlayAmbientSong1(OverrideCurrentSong);
                break;
            case SongType.Ambient2:
                SoundManager.PlayAmbientSong2(OverrideCurrentSong);
                break;
            case SongType.Shock:
                SoundManager.PlayShockSong(OverrideCurrentSong);
                break;
            case SongType.Boss:
                SoundManager.PlayAmbientBossSong(OverrideCurrentSong);
                break;
            case SongType.SelfDestruct:
                SoundManager.PlaySelfDestructSong(OverrideCurrentSong);
                break;
        }
    }

    public enum SongType
    {
        None = 0,
        Ambient1,
        Ambient2,
        Shock,
        Boss,
        SelfDestruct,
    }
}
