using System;
using TMPro;
using UnityEngine;

public class TimePresenter : MonoBehaviour
{
    [SerializeField] private Timer timer;

    [SerializeField] TextMeshProUGUI displayTime;

    private void Start()
    {
        timer.onTimerChange += UpdateDisplay;
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        var time = TimeSpan.FromSeconds(timer.CurrentTimeLeft);
        displayTime.text = string.Format("{0:D1}:{1:D2}", (int)time.TotalMinutes, time.Seconds);
    }
}