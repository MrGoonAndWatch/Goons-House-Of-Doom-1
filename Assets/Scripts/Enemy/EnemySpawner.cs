using System;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject Enemy;

    [Tooltip("How faster in seconds this spawner should spawn its first enemy than SpawnRateInSeconds")]
    public float FirstSpawnOffsetInSeconds;
    public float SpawnRateInSeconds;
    public bool Pause;

    public float MinSpawnRateInSeconds;

    private float _secondsUntilSpawn;

    private PlayerStatus _playerStatus;

    private void Start()
    {
        _secondsUntilSpawn = SpawnRateInSeconds - FirstSpawnOffsetInSeconds;
        _playerStatus = FindObjectOfType<PlayerStatus>();
    }

    private void Update()
    {
        if (_playerStatus.Paused || Pause) return;

        _secondsUntilSpawn -= Time.deltaTime;
        if (_secondsUntilSpawn <= 0)
            SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        Instantiate(Enemy, gameObject.transform.position, Quaternion.identity);
        _secondsUntilSpawn = SpawnRateInSeconds;
    }

    public void IncreaseSpawnRate(float increaseBy)
    {
        if (SpawnRateInSeconds == MinSpawnRateInSeconds) return;

        var newValue = SpawnRateInSeconds - increaseBy;
        newValue = Math.Max(newValue, MinSpawnRateInSeconds);
        SpawnRateInSeconds = newValue;
        _secondsUntilSpawn -= increaseBy;
    }
}
