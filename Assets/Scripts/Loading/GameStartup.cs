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
        var loadGameData = FindAnyObjectByType<LoadGameData>();
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

        if (_sceneLoadData != null && _sceneLoadData.LoadPosition != null)
        {
            Player.transform.position = _sceneLoadData.LoadPosition.ToVector3();
        }

        if (_sceneLoadData != null && _sceneLoadData.LoadRotation != null)
        {
            Player.transform.eulerAngles = _sceneLoadData.LoadRotation.ToVector3();
        }

        if (_gameState != null)
        {
            var dataSaver = FindAnyObjectByType<DataSaver>();
            dataSaver.LoadGameStateFromFileData(_gameState);
        }

        SceneManager.LoadScene(targetScene);
    }
}
