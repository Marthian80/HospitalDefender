using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private static Timer instance;
    private bool timerStopped;

    private LevelManager levelManager;
    [SerializeField] private float timeToSurviveLevel = 180f;
    public float CurrentTimeLeft { get { return instance.timeToSurviveLevel; } }

    public event Action onTimerChange;

    private void Awake()
    {
        ManageSingleton();
        levelManager = FindObjectOfType<LevelManager>();
        levelManager.onGameEnded += StopTimer;
    }    

    private void Update()
    {
        if (timeToSurviveLevel > 0 && !timerStopped)
        {
            UpdateTimer();
        }        
    }

    private void ManageSingleton()
    {
        if (instance != null)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
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

    private void StopTimer()
    {
        timerStopped = true;
    }
}
