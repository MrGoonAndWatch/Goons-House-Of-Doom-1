using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public DataSaver DataSaver;

    private static SceneChanger _instance;

    void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;

        DontDestroyOnLoad(gameObject);
        if (DataSaver == null)
            DataSaver = FindAnyObjectByType<DataSaver>();
    }

    public void ChangeScene(SceneLoadData sceneLoadData, DoorLoadType doorScene = DoorLoadType.None)
    {
        var playerStatus = FindAnyObjectByType<PlayerStatus>();
        var playerInventory = FindAnyObjectByType<PlayerInventory>();
        DataSaver.SaveGameStateFromScene(playerStatus, playerInventory, sceneLoadData);

        if (doorScene == DoorLoadType.None)
            FinishSceneLoad();
        else
            SceneManager.LoadScene(doorScene.ToString());

        var useKey = FindAnyObjectByType<UseKey>();
        useKey.ResetState();
        var pickupItem = FindAnyObjectByType<PickupItem>();
        pickupItem.ResetState();
    }

    private void FinishSceneLoad()
    {
        var sceneLoadData = DataSaver.GetSceneLoadData();
        SceneManager.LoadScene(sceneLoadData.TargetScene);

        if(sceneLoadData.LoadPosition != null)
            transform.position = sceneLoadData.LoadPosition.ToVector3();
        if(sceneLoadData.LoadRotation != null)
            transform.eulerAngles = sceneLoadData.LoadRotation.ToVector3();

        var playerStatus = FindAnyObjectByType<PlayerStatus>();
        var playerInventory = FindAnyObjectByType<PlayerInventory>();
        DataSaver.LoadFromGameState(playerStatus, playerInventory);
    }
}
