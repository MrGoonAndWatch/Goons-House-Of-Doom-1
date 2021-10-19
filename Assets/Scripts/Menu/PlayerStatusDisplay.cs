﻿using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusDisplay : MonoBehaviour
{
    public PlayerStatus PlayerStatus;
    public Image StatusIcon;
    public Text StatusText;

    private HealthStatus _currentStatus;

    void Start()
    {
        PlayerStatus = FindObjectOfType<PlayerStatus>();
    }

    void Update()
    {
        var newStatus = PlayerStatus.GetHealthStatus();
        if (newStatus != _currentStatus)
        {
            UpdateUi(newStatus);
            _currentStatus = newStatus;
        }
    }

    void UpdateUi(HealthStatus status)
    {
        // TODO: Use actual sprites here instead of just changing color!
        switch (status)
        {
            case HealthStatus.Dead:
                StatusText.text = "Dead Lole";
                StatusText.color = Color.red;
                StatusIcon.color = Color.red;
                break;
            case HealthStatus.Special:
                StatusText.text = "Special";
                StatusText.color = Color.red;
                StatusIcon.color = Color.red;
                break;
            case HealthStatus.BadTummyAche:
                StatusText.text = "Really Bad Tummy Ache";
                StatusText.color = new Color(191, 127, 0);
                StatusIcon.color = new Color(191, 127, 0);
                break;
            case HealthStatus.TummyAche:
                StatusText.text = "Tummy Ache";
                StatusText.color = Color.yellow;
                StatusIcon.color = Color.yellow;
                break;
            case HealthStatus.Healthy:
                StatusText.text = "Healthy";
                StatusText.color = Color.green;
                StatusIcon.color = Color.green;
                break;
        }
    }
}