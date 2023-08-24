using System;
using TMPro;
using UnityEngine;

public class TimePresenter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI displayTime;

    private void Start()
    {
        Timer.Instance.onTimerChange += UpdateDisplay;
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        var time = TimeSpan.FromSeconds(Timer.Instance.CurrentTimeLeft);
        displayTime.text = string.Format("{0:D1}:{1:D2}", (int)time.TotalMinutes, time.Seconds);
    }
}