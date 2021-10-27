using System;
using UnityEngine;
using UnityEngine.UI;

public class CountdownDisplay : MonoBehaviour
{
    public GameObject CountdownUi;
    public Text CountdownTextbox;

    private bool _countdownActive;
    private TimeSpan _countdown;

    public float CountdownTimeInMinutes = 10.0f;

    void Start()
    {
        CountdownUi.SetActive(false);
        _countdown = TimeSpan.FromMinutes(CountdownTimeInMinutes);
    }
    
    void Update()
    {
        if (_countdownActive)
        {
            _countdown = _countdown.Subtract(TimeSpan.FromSeconds(Time.deltaTime));
            if (_countdown.TotalMinutes < 0)
            {
                _countdown = TimeSpan.Zero;
                // TODO: Blow up.
            }

            CountdownTextbox.text = GetTimerString(_countdown);
        }
    }

    private static string GetTimerString(TimeSpan countdown)
    {
        return countdown.ToString(@"mm\:ss\.fff");
    }

    public bool StartCountdown()
    {
        if (_countdownActive)
        {
            if (_countdown.TotalMinutes > 2.0f)
            {
                _countdown = TimeSpan.FromMilliseconds(_countdown.TotalMilliseconds / 2);
                return true;
            }
            else
                return false;
        }
        else
        {
            CountdownUi.SetActive(true);
            _countdown = TimeSpan.FromMinutes(CountdownTimeInMinutes);
            _countdownActive = true;
            return true;
        }
    }

    public void StopCountdown()
    {
        _countdownActive = false;
        CountdownUi.SetActive(false);
    }
}
