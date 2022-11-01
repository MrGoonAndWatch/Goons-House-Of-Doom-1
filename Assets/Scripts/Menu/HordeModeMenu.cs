using UnityEngine;
using UnityEngine.SceneManagement;

public class HordeModeMenu : MonoBehaviour
{
    public void ReturnToTitleScreen()
    {
        // TODO: Going back to title screen breaks horribly!
        //SceneManager.LoadScene(SceneNames.TitleScreen);
        Application.Quit();
    }
}
