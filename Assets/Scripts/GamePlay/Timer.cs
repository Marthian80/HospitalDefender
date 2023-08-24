using System;
using UnityEngine;

public class Timer : Singleton<Timer>
{    
    private bool timerStopped;
        
    [SerializeField] private float timeToSurviveLevel = 150f;
    private float currentTimeLeft;
    public float CurrentTimeLeft { get { return currentTimeLeft; } }

    public event Action onTimerChange;
    public event Action onFullMinutePassed;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        currentTimeLeft = timeToSurviveLevel;
        LevelManager.Instance.onGameEnded += StopTimer;
    }

    private void Update()
    {
        if (currentTimeLeft > 0 && !timerStopped)
        {
            UpdateTimer();
        }        
    }

    public void StopTimer()
    {
        timerStopped = true;
    }

    public void ResetTimer()
    {
        currentTimeLeft = timeToSurviveLevel;
        timerStopped = false;
        if (onTimerChange!= null) 
        {
            onTimerChange();
        }
    }

    private void UpdateTimer()
    {
        var previousTime = currentTimeLeft;

        currentTimeLeft -= Time.deltaTime;

        if (previousTime != currentTimeLeft && onTimerChange != null)
        {
            onTimerChange();
        }

        if (currentTimeLeft % 60 == 0 && onFullMinutePassed != null)
        {
            onFullMinutePassed();
        }

        if (currentTimeLeft <= 0)
        {
            LevelManager.Instance.LoadLevelWon();
        }
    }    
}
