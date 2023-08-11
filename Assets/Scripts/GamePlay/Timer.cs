using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private LevelManager levelManager;
    [SerializeField] private float timeToSurviveLevel = 180f;
    public float CurrentTimeLeft { get { return timeToSurviveLevel; } }

    public event Action onTimerChange;

    private void Update()
    {
        UpdateTimer();        
    }

    private void Awake()
    {
        levelManager = FindObjectOfType<LevelManager>();
    }

    private void UpdateTimer()
    {
        var previousTime = timeToSurviveLevel;

        timeToSurviveLevel -= Time.deltaTime;

        if (previousTime != timeToSurviveLevel && onTimerChange != null)
        {
            onTimerChange();
        }

        if (timeToSurviveLevel <= 0)
        {
            levelManager.LoadLevelWon();
        }
    }
}
