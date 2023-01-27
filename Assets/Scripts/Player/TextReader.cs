using System;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class TextReader : MonoBehaviour
{
    public PlayerStatus PlayerStatus;
    public GameObject DescriptiveText;
    public Text TextBox;

    private int _currentLineIndex;
    private string[] _currentLines;
    private string[] _currentChoices;
    private int _currentChoiceSelection;
    private bool _makingChoice;
    private Action _onChoiceConfirmed;
    private bool _justMovedChoice;

    public float AdvanceTextCooldown;
    private float _advanceTextCooldownRemaining;

    private bool _queuedText = false;
    private string[] _queuedLines = null;
    private string[] _queuedChoices = null;
    private Action _queuedOnChoiceConfirmed = null;

    void Start()
    {
        DescriptiveText.SetActive(false);
    }

    void Update()
    {
        if (_advanceTextCooldownRemaining > 0)
            _advanceTextCooldownRemaining -= Time.deltaTime;

        if (!PlayerStatus.Reading || _advanceTextCooldownRemaining > 0)
            return;

        if (_makingChoice)
        {
            if (Input.GetButtonDown(GameConstants.Controls.Action) || ControllerInputProcessor.PressedAction())
                ConfirmChoice();
            else if (Input.GetButtonDown(GameConstants.Controls.Aim) || ControllerInputProcessor.PressedAim())
                CloseTextbox();
            else
                ProcessChoiceMovement();
        }
        else if (Input.GetButtonDown(GameConstants.Controls.Action) || ControllerInputProcessor.PressedAction())
            AdvanceText();
    }

    public void QueueReadText(string[] lines, string[] choices = null, Action onChoiceConfirmed = null)
    {
        if (_queuedText)
            return;

        _queuedText = true;
        _queuedLines = lines;
        _queuedChoices = choices;
        _queuedOnChoiceConfirmed = onChoiceConfirmed;
    }

    public void ReadText(string[] lines, string[] choices = null, Action onChoiceConfirmed = null)
    {
        if (PlayerStatus.LockMovement || PlayerStatus.Reading || _advanceTextCooldownRemaining > 0)
            return;

        _advanceTextCooldownRemaining = AdvanceTextCooldown;
        _currentLineIndex = 0;
        _currentLines = lines;
        _currentChoices = choices;
        _currentChoiceSelection = 0;
        _makingChoice = false;
        _onChoiceConfirmed = onChoiceConfirmed;
        PlayerStatus.Reading = true;
        AdvanceText();
        DescriptiveText.SetActive(true);
    }

    private void AdvanceText()
    {
        _advanceTextCooldownRemaining = AdvanceTextCooldown;
        if (_currentLineIndex >= _currentLines.Length)
        {
            if (_currentChoices == null || !_currentChoices.Any())
                CloseTextbox();
            else
            {
                _makingChoice = true;
                _currentChoiceSelection = 0;
                DisplayChoices();
            }

            return;
        }

        TextBox.text = _currentLines[_currentLineIndex];
        _currentLineIndex++;
    }

    private void DisplayChoices()
    {
        var choicesWithSelection = new StringBuilder();
        //choicesWithSelection.AppendLine(_currentLines[_currentLines.Length - 1]);
        for (int i = 0; i < _currentChoices.Length; i++)
        {
            if (_currentChoiceSelection == i)
                choicesWithSelection.Append(">");
            choicesWithSelection.Append(_currentChoices[i]);
            if (i + 1 < _currentChoices.Length)
                choicesWithSelection.Append("   ");
        }
        TextBox.text = choicesWithSelection.ToString();
    }

    public void ForceCloseTextbox()
    {
        if(_queuedText)
            CloseTextbox();
        CloseTextbox();
    }

    private void CloseTextbox()
    {
        DescriptiveText.SetActive(false);
        _currentLineIndex = 0;
        _currentLines = null;
        _currentChoices = null;
        _currentChoiceSelection = 0;
        _makingChoice = false;
        _onChoiceConfirmed = null;
        PlayerStatus.Reading = false;

        if (_queuedText)
        {
            ReadText(_queuedLines, _queuedChoices, _queuedOnChoiceConfirmed);
            _queuedLines = null;
            _queuedChoices = null;
            _queuedOnChoiceConfirmed = null;
            _queuedText = false;
        }
    }

    private void ProcessChoiceMovement()
    {
        var horizontalMovement = Input.GetAxis(GameConstants.Controls.HorizontalMovement);

        if (!_justMovedChoice && horizontalMovement < 0)
        {
            _justMovedChoice = true;
            _advanceTextCooldownRemaining = AdvanceTextCooldown;
            _currentChoiceSelection--;
            if (_currentChoiceSelection < 0)
                _currentChoiceSelection = _currentChoices.Length - 1;
            DisplayChoices();
        }
        else if (!_justMovedChoice && horizontalMovement > 0)
        {
            _justMovedChoice = true;
            _advanceTextCooldownRemaining = AdvanceTextCooldown;
            _currentChoiceSelection = (_currentChoiceSelection + 1) % _currentChoices.Length;
            DisplayChoices();
        }
        else if (_justMovedChoice && horizontalMovement == 0)
            _justMovedChoice = false;
    }

    private void ConfirmChoice()
    {
        // TODO: Can do something more intricate with this system if we pass back which selection was made and let it handle things from there.
        //       For now it just assumes that the last choice is no and everything else is yes.
        if (_currentChoiceSelection == _currentChoices.Length - 1)
        {
            CloseTextbox();
        }
        else
        {
            if (_onChoiceConfirmed != null)
                _onChoiceConfirmed();
            CloseTextbox();
        }
    }
}
