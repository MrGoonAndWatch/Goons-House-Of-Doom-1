using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartup : MonoBehaviour
{
    private bool _loading = false;

    private DataSaver.GameState _gameState;
    private SceneLoadData _sceneLoadData;
    public GameObject Player;

    private void Start()
    {
        LoadSaveGameData();
    }

    private void LoadSaveGameData()
    {
        var loadGameData = FindObjectOfType<LoadGameData>();
        if (loadGameData == null)
            return;

        var gameState = loadGameData.GetGameState();
        _gameState = gameState;

        if (gameState == null)
            return;

        _sceneLoadData = gameState.SceneLoadData;
    }

    private void Update()
    {
        if (_loading)
            return;

        _loading = true;

        var targetScene = _sceneLoadData == null ? SceneNames.MainHall : _sceneLoadData.TargetScene;

        if (_sceneLoadData != null && _sceneLoadData.LoadPosition.HasValue)
        {
            Player.transform.position = _sceneLoadData.LoadPosition.Value;
        }

        if (_sceneLoadData != null && _sceneLoadData.LoadRotation.HasValue)
        {
            Player.transform.eulerAngles = _sceneLoadData.LoadRotation.Value;
        }

        if (_gameState != null)
        {
            var dataSaver = FindObjectOfType<DataSaver>();
            dataSaver.LoadGameStateFromFileData(_gameState);
        }

        SceneManager.LoadScene(targetScene);
    }
}
