using UnityEngine;

public class TogglePauseMenu : MonoBehaviour
{
    public PlayerStatus PlayerStatus;
    public GameObject PauseMenu;

    private bool _paused;

    private void Start()
    {
        PauseMenu.SetActive(false);
    }

    private void Update()
    {
        if (PlayerStatus.CanPause() && (Input.GetButtonDown(GameConstants.Controls.Pause) || ControllerInputProcessor.PressedPause()))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        _paused = !_paused;
        PlayerStatus.Paused = _paused;

        PauseMenu.SetActive(_paused);

        if(_paused)
            SoundManager.PauseSong();
        else
            SoundManager.ResumeSong();

        SoundManager.SetMuteAll(_paused);
    }
}
