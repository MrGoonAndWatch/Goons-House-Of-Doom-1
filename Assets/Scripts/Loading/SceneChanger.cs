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
            DataSaver = FindObjectOfType<DataSaver>();
    }

    public void ChangeScene(SceneLoadData sceneLoadData, DoorLoadType doorScene = DoorLoadType.None)
    {
        var playerStatus = FindObjectOfType<PlayerStatus>();
        var playerInventory = FindObjectOfType<PlayerInventory>();
        DataSaver.SaveGameStateFromScene(playerStatus, playerInventory, sceneLoadData);

        if (doorScene == DoorLoadType.None)
            FinishSceneLoad();
        else
            SceneManager.LoadScene(doorScene.ToString());

        var useKey = FindObjectOfType<UseKey>();
        useKey.ResetState();
        var pickupItem = FindObjectOfType<PickupItem>();
        pickupItem.ResetState();
    }

    private void FinishSceneLoad()
    {
        var sceneLoadData = DataSaver.GetSceneLoadData();
        SceneManager.LoadScene(sceneLoadData.TargetScene);

        if(sceneLoadData.LoadPosition.HasValue)
            transform.position = sceneLoadData.LoadPosition.Value;
        if(sceneLoadData.LoadRotation.HasValue)
            transform.eulerAngles = sceneLoadData.LoadRotation.Value;

        var playerStatus = FindObjectOfType<PlayerStatus>();
        var playerInventory = FindObjectOfType<PlayerInventory>();
        DataSaver.LoadFromGameState(playerStatus, playerInventory);
    }
}
