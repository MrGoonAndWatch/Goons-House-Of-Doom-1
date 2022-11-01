using UnityEngine;

public class LoadGameData : MonoBehaviour
{
    private DataSaver.GameState _gameState;
    private static LoadGameData _instance;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;

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
