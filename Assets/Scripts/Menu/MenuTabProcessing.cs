using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuTabProcessing : MonoBehaviour
{
    [Serializable]
    public struct ButtonTabInfo
    {
        public Button Button;
        public int X;
        public int Y;
    }

    [SerializeField]
    private EventSystem _eventSystem;
    [SerializeField]
    private ButtonTabInfo[] _buttons;
    [SerializeField]
    private ColorBlock _colors;
    private Button[][] _buttonsArray;

    private int _currentMenuX;
    private int _currentMenuY;

    [SerializeField]
    private bool _enabled;
    
    private bool _moving;

    [SerializeField]
    private float _cooldownAfterEnabledInSeconds = 0.3f;
    private bool _inCooldown;
    private float _timeUntilInputsEnabled;

    public void StartProcessing()
    {
        _inCooldown = true;
        _timeUntilInputsEnabled = _cooldownAfterEnabledInSeconds;
    }

    public void StopProcessing()
    {
        _enabled = false;
    }

    void Start()
    {
        var buttonGroupings = _buttons.GroupBy(b => b.Y).OrderBy(b => b.Key).ToArray();
        _buttonsArray = new Button[buttonGroupings.Length][];
        for (var i = 0; i < buttonGroupings.Length; i++)
        {
            var buttonGrouping = buttonGroupings[i].OrderBy(g => g.X).ToArray();
            _buttonsArray[i] = new Button[buttonGrouping.Length];
            for (var j = 0; j < buttonGrouping.Length; j++)
            {
                var buttonTabInfo = buttonGrouping[j];
                _buttonsArray[i][j] = buttonTabInfo.Button;
            }
        }

        for (var i = 0; i < _buttonsArray.Length; i++)
        {
            for(var j = 0; j < _buttonsArray[i].Length; j++)
                RefreshUi(j,i);
        }
    }
    
    void Update()
    {
        if (!_enabled)
        {
            if (_inCooldown) ProcessCooldown();
            if(!_enabled)
                return;
        }

        CheckForMenuTab();
        CheckForMenuSelection();
    }

    private void ProcessCooldown()
    {
        _timeUntilInputsEnabled -= Time.deltaTime;
        if (_timeUntilInputsEnabled <= 0)
        {
            _inCooldown = false;
            _enabled = true;
        }
    }

    private void CheckForMenuTab()
    {
        var horizontal = Input.GetAxis(GameConstants.Controls.HorizontalMovement);
        var vertical = Input.GetAxis(GameConstants.Controls.VerticalMovement);

        if (!_moving)
        {
            if(horizontal != 0)
                _moving = true;
            if (horizontal > 0)
                MoveMenuRight();
            else if (horizontal < 0)
                MoveMenuLeft();
        }

        if (!_moving)
        {
            if(vertical != 0)
                _moving = true;
            if (vertical < 0)
                MoveMenuDown();
            else if (vertical > 0)
                MoveMenuUp();
        }
        
        if (horizontal == 0 && vertical == 0)
            _moving = false;
    }

    private void CheckForMenuSelection()
    {
        if (Input.GetButtonDown(GameConstants.Controls.Action) || ControllerInputProcessor.PressedAction())
        {
            _buttonsArray[_currentMenuY][_currentMenuX].onClick.Invoke();
        }
    }

    private void MoveMenuLeft()
    {
        var lastX = _currentMenuX;
        _currentMenuX = (_currentMenuX - 1);
        if (_currentMenuX < 0)
            _currentMenuX = _buttonsArray[_currentMenuY].Length - 1;
        RefreshUi(lastX, _currentMenuY);
    }

    private void MoveMenuRight()
    {
        var lastX = _currentMenuX;
        _currentMenuX = (_currentMenuX + 1) % _buttonsArray[_currentMenuY].Length;
        RefreshUi(lastX, _currentMenuY);
    }

    private void MoveMenuUp()
    {
        var lastY = _currentMenuY;
        _currentMenuY = (_currentMenuY - 1);
        if (_currentMenuY < 0)
            _currentMenuY = _buttonsArray.Length - 1;
        RefreshUi(_currentMenuX, lastY);
    }

    private void MoveMenuDown()
    {
        var lastY = _currentMenuY;
        _currentMenuY = (_currentMenuY + 1) % _buttonsArray.Length;
        RefreshUi(_currentMenuX, lastY);
    }

    private void RefreshUi(int lastX, int lastY)
    {
        _buttonsArray[lastY][lastX].image.color = _colors.normalColor;
        _buttonsArray[_currentMenuY][_currentMenuX].image.color = _colors.highlightedColor;
        _eventSystem.SetSelectedGameObject(_buttonsArray[_currentMenuY][_currentMenuX].gameObject);
    }
}
