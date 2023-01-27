using System;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInputProcessor : MonoBehaviour
{
    private static ControllerInputProcessor _instance;

    private ControllerType[] _connectedControllers;

    private static Dictionary<ControllerType, Dictionary<LogicalControllerButtons, int>> ControllerMappings =
        new Dictionary<ControllerType, Dictionary<LogicalControllerButtons, int>>
        {
            {ControllerType.PlayStation, new Dictionary<LogicalControllerButtons, int>
            {
                {LogicalControllerButtons.TopButton, 3},
                {LogicalControllerButtons.LeftButton, 0},
                {LogicalControllerButtons.RightButton, 2},
                {LogicalControllerButtons.BottomButton, 1},
                {LogicalControllerButtons.StartButton, 9}
            }},
            {ControllerType.Xbox, new Dictionary<LogicalControllerButtons, int>
            {
                {LogicalControllerButtons.TopButton, 3},
                {LogicalControllerButtons.LeftButton, 2},
                {LogicalControllerButtons.RightButton, 1},
                {LogicalControllerButtons.BottomButton, 0},
                {LogicalControllerButtons.StartButton, 7}
            }},
            // TODO: Find best generic set of controls to use here.
            {ControllerType.Unknown, new Dictionary<LogicalControllerButtons, int>
            {
                {LogicalControllerButtons.TopButton, 3},
                {LogicalControllerButtons.LeftButton, 2},
                {LogicalControllerButtons.RightButton, 1},
                {LogicalControllerButtons.BottomButton, 0},
                {LogicalControllerButtons.StartButton, 7}
            }}
        };
    
    void Start()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        RefreshDevices();
    }

    private static bool CheckInstance()
    {
        if (_instance == null)
        {
            Debug.LogWarning("No ControllerInputProcessor instance found!");
            return true;
        }
        return false;
    }

    public static bool PressedMenu()
    {
        if (CheckInstance())
            return false;

        return _instance.WasButtonPressed(LogicalControllerButtons.TopButton);
    }

    public static bool PressedPause()
    {
        if (CheckInstance())
            return false;

        return _instance.WasButtonPressed(LogicalControllerButtons.StartButton);
    }

    public static bool PressedAction()
    {
        if (CheckInstance())
            return false;

        return _instance.WasButtonPressed(LogicalControllerButtons.RightButton);
    }

    public static bool PressedAim()
    {
        if (CheckInstance())
            return false;

        return _instance.WasButtonPressed(LogicalControllerButtons.BottomButton);
    }

    public static bool IsPressingAim()
    {
        if (CheckInstance())
            return false;

        return _instance.IsButtonHeld(LogicalControllerButtons.BottomButton);
    }

    public static bool ReleasedAim()
    {
        if (CheckInstance())
            return false;

        return _instance.WasButtonReleased(LogicalControllerButtons.BottomButton);
    }

    public static bool PressedRun()
    {
        if (CheckInstance())
            return false;

        return _instance.WasButtonPressed(LogicalControllerButtons.LeftButton);
    }

    public static bool IsPressingRun()
    {
        if (CheckInstance())
            return false;

        return _instance.IsButtonHeld(LogicalControllerButtons.LeftButton);
    }

    private bool WasButtonReleased(LogicalControllerButtons button)
    {
        return CheckControllersForButton(button, Input.GetButtonUp);
    }

    private bool WasButtonPressed(LogicalControllerButtons button)
    {
        return CheckControllersForButton(button, Input.GetButtonDown);
    }

    private bool IsButtonHeld(LogicalControllerButtons button)
    {
        return CheckControllersForButton(button, Input.GetButton);
    }

    private bool CheckControllersForButton(LogicalControllerButtons button, Func<string, bool> inputCall)
    {
        for (var i = 0; i < _connectedControllers.Length; i++)
        {
            var inputName = $"J{i + 1}B{ControllerMappings[_connectedControllers[i]][button]}";
            if (inputCall(inputName))
                return true;
        }
        return false;
    }

    // TODO: Call this when controllers connect/disconnect.
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
        }
    }
}
