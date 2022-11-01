using System;
using UnityEngine;
using UnityEngine.UI;

public class CountupDisplay : MonoBehaviour
{
    public GameObject CountdownUi;
    public Text CountdownTextbox;
    public PlayerStatus PlayerStatus;

    private TimeSpan _countdown;
    private bool _initialized;
    
    void Start()
    {
        _countdown = new TimeSpan(0);
    }

    void Update()
    {
        if (!_initialized)
        {
            CountdownUi.SetActive(true);
            _initialized = true;
        }

        if (PlayerStatus.Paused || PlayerStatus.GetHealthStatus() == HealthStatus.Dead)
            return;

        _countdown = _countdown.Add(TimeSpan.FromSeconds(Time.deltaTime));
        CountdownTextbox.text = GetTimerString(_countdown);
    }

    private static string GetTimerString(TimeSpan countdown)
    {
        return countdown.ToString(@"mm\:ss\.fff");
    }

    public TimeSpan GetTime()
    {
        return _countdown;
    }

    public void Hide()
    {
        CountdownUi.SetActive(false);
    }
}
