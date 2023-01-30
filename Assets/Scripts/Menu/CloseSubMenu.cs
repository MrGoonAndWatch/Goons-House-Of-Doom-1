using UnityEngine;
using UnityEngine.UI;

public class CloseSubMenu : MonoBehaviour
{
    private Button _currentClose;
    private bool _enabled;

    public void StartCloseListener(Button closeButton)
    {
        _currentClose = closeButton;
        _enabled = true;
    }

    void Update()
    {
        if (!_enabled) return;

        if (/*Input.GetKeyDown(GameConstants.Controls.Aim) ||*/ ControllerInputProcessor.PressedAim())
        {
            _currentClose.onClick.Invoke();
            _enabled = false;
        }
    }
}
