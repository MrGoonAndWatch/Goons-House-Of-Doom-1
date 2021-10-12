using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    private string _targetScene;
    private Vector3 _loadPosition;
    private Vector3 _loadRotation;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void ChangeScene(string targetScene, Vector3 loadPosition, Vector3 loadRotation, DoorLoadType doorScene = DoorLoadType.None)
    {
        _targetScene = targetScene;
        _loadPosition = loadPosition;
        _loadRotation = loadRotation;
        
        if (doorScene == DoorLoadType.None)
        {
            FinishSceneLoad();
        }
        // TODO: Door shit.
        else
        {
            SceneManager.LoadScene(doorScene.ToString());
        }
    }

    private void FinishSceneLoad()
    {
        SceneManager.LoadScene(_targetScene);

        transform.position = _loadPosition;
        transform.eulerAngles = _loadRotation;
    }
}
