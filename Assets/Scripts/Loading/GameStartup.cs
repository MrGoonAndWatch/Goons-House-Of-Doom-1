using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartup : MonoBehaviour
{
    private bool _loading = false;

    private void Update()
    {
        if (_loading)
            return;

        _loading = true;
        SceneManager.LoadScene("Main Hall");
    }
}
