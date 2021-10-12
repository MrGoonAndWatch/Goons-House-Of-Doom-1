using UnityEngine;
using UnityEngine.UI;

public class TextReader : MonoBehaviour
{
    public PlayerStatus PlayerStatus;
    public GameObject DescriptiveText;
    public Text TextBox;

    private int _currentLineIndex;
    private string[] _currentLines;

    public float AdvanceTextCooldown;
    private float _advanceTextCooldownRemaining;

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
        
        if (Input.GetButtonDown(GameConstants.Controls.Action))
            AdvanceText();
    }

    public void ReadText(string[] lines)
    {
        if (PlayerStatus.Reading)
            return;

        _advanceTextCooldownRemaining = AdvanceTextCooldown;
        _currentLineIndex = 0;
        _currentLines = lines;
        PlayerStatus.Reading = true;
        AdvanceText();
        DescriptiveText.SetActive(true);
    }

    private void AdvanceText()
    {
        if (_currentLineIndex >= _currentLines.Length)
        {
            CloseTextbox();
            return;
        }

        TextBox.text = _currentLines[_currentLineIndex];
        _currentLineIndex++;
    }

    private void CloseTextbox()
    {
        DescriptiveText.SetActive(false);
        _currentLineIndex = 0;
        _currentLines = null;
        PlayerStatus.Reading = false;
    }
}
