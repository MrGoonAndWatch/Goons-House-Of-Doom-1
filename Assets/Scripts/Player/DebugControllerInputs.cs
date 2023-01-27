using UnityEngine;

public class DebugControllerInputs : MonoBehaviour
{
    private ControllerType[] _connectedControllers;

    void Start()
    {
        RefreshDevices();
    }

    private void RefreshDevices()
    {
        var joysticks = Input.GetJoystickNames();
        _connectedControllers = new ControllerType[joysticks.Length];
        for (var i = 0; i < joysticks.Length; i++)
        {
            var joy = joysticks[i].ToLower();
            if (joy.Contains("xbox"))
                _connectedControllers[i] = ControllerType.Xbox;
            else if (joy.Contains("wireless controller"))
                _connectedControllers[i] = ControllerType.PlayStation;
            else
                _connectedControllers[i] = ControllerType.Unknown;
            
            Debug.Log(joy);
        }
    }

    void Update()
    {
        for (var i = 0; i < _connectedControllers.Length; i++)
        {
            var btn1 = Input.GetButtonDown($"joystick {i+1} button 1");
            if (btn1)
            {
                Debug.Log($"Joystick {i} is pressed button 1!");
            }
        }
    }
}
