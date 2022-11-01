using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HordeModeManager : MonoBehaviour
{
    [HideInInspector]
    public int KillCount;

    [Tooltip("How frequently the spawn rate should be increased")]
    public float IncreaseSpawnRatePerSeconds;

    [Tooltip("How much to increase the spawn rate every time it increases")]
    public float IncreaseSpawnRateByPerTick;

    private PlayerStatus _playerStatus;
    private float _nextSpawnRateTime;

    private EnemySpawner[] _enemySpawners;

    public Canvas ResultScreen;
    public Text KillCountText;
    public Text SurvivalTimeText;
    public Text TotalScore;
    private CountupDisplay _survivalTimer;
    
    private void Start()
    {
        _playerStatus = FindObjectOfType<PlayerStatus>();
        _enemySpawners = FindObjectsOfType<EnemySpawner>();
        _nextSpawnRateTime = IncreaseSpawnRatePerSeconds;
        _survivalTimer = GetComponent<CountupDisplay>();
    }
    
    private void Update()
    {
        HandleSpawnRateIncrease();
    }

    private void HandleSpawnRateIncrease()
    {
        if (_playerStatus.Paused) return;

        _nextSpawnRateTime -= Time.deltaTime;

        if (_nextSpawnRateTime <= 0)
        {
            for (var i = 0; i < _enemySpawners.Length; i++)
            {
                _enemySpawners[i].IncreaseSpawnRate(IncreaseSpawnRateByPerTick);
            }
            _nextSpawnRateTime = IncreaseSpawnRatePerSeconds;
        }
    }

    public void OnGameEnd()
    {
        _survivalTimer.Hide();

        for (var i = 0; i < _enemySpawners.Length; i++)
        {
            _enemySpawners[i].Pause = true;
        }

        var survivalTimeInSeconds = _survivalTimer.GetTime().TotalSeconds;
        KillCountText.text = KillCount.ToString("D");
        SurvivalTimeText.text = GetTimerString(_survivalTimer.GetTime());
        var totalScore = (int) (survivalTimeInSeconds * KillCount);
        TotalScore.text = totalScore.ToString("D");
        ResultScreen.gameObject.SetActive(true);
    }

    private static string GetTimerString(TimeSpan countdown)
    {
        return countdown.ToString(@"mm\:ss");
    }

    public void AddKill()
    {
        KillCount++;
    }
}
