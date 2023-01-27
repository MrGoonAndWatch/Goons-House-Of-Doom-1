using UnityEngine;

public class GameWin : MonoBehaviour
{
    void Update()
    {
        if (Input.GetButtonDown(GameConstants.Controls.Pause) || ControllerInputProcessor.PressedPause())
        {
            Application.Quit();
        }
    }
}
