using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    public PlayerStatus PlayerStatus;
    public GameObject Vignette;
    
    private float _secondsLeftInCutsceneInstruction;
    private int? _nextCutsceneInstruction;
    
    private void Start()
    {
        if (PlayerStatus == null)
            PlayerStatus = FindObjectOfType<PlayerStatus>();
    }

    private void Update()
    {
        if (_secondsLeftInCutsceneInstruction <= 0)
            return;

        _secondsLeftInCutsceneInstruction -= Time.deltaTime;

        if (_secondsLeftInCutsceneInstruction <= 0)
            NextInstruction();
    }

    private void NextInstruction()
    {
        if (_nextCutsceneInstruction.HasValue)
            ProcessNextCutsceneInstruction();
        else
            EndCutscene();
    }

    public void PlaySelfDestructCutscene()
    {
        StartCutscene();

        SoundManager.PauseSong();

        _nextCutsceneInstruction = CutsceneInstructions.StartCountdownTimer;
        _secondsLeftInCutsceneInstruction = 8.0f;
        SoundManager.PlaySelfDestructVoiceLine();
    }

    private void StartCutscene()
    {
        PlayerStatus.LockMovement = true;
        Vignette.SetActive(true);
    }

    private void EndCutscene()
    {
        PlayerStatus.LockMovement = false;
        Vignette.SetActive(false);
    }

    private void ProcessNextCutsceneInstruction()
    {
        if (_nextCutsceneInstruction == CutsceneInstructions.StartCountdownTimer)
        {
            var countdown = FindObjectOfType<CountdownDisplay>();
            countdown.StartCountdown();
            _nextCutsceneInstruction = null;
            _secondsLeftInCutsceneInstruction = 0.1f;
            SoundManager.PlaySelfDestructSong();
        }
    }

    private static class CutsceneInstructions
    {
        public const int StartCountdownTimer = 1;
    }
}
