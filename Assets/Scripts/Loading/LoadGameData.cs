using UnityEngine;

public class LoadGameData : MonoBehaviour
{
    private DataSaver.GameState _gameState;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void SetGameState(DataSaver.GameState gameState)
    {
        _gameState = gameState;
    }

    public DataSaver.GameState GetGameState()
    {
        return _gameState;
    }
}
