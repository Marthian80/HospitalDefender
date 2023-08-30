using System;
using UnityEngine;

public class InfectionRate : Singleton<InfectionRate>
{
    [SerializeField] private int allowedNumberofInfectedPatients = 5;
        
    private int currentInfectedPatients = 0;
    public int CurrentInfectedPatients { get { return currentInfectedPatients; } }

    public event Action patientInfected;

    protected override void Awake()
    {
        base.Awake();        
    }

    public void ResetInfectedPatients()
    {
        currentInfectedPatients = 0;
        if (patientInfected != null)
        {
            patientInfected();
        }
    }

    public void InfectPatientAtLocation(Vector2 location)
    {
        currentInfectedPatients++;
        AudioPlayer.Instance.PlayPatientInfectedClip();

        if (patientInfected != null)
        {
            patientInfected();
        }

        if (currentInfectedPatients >= allowedNumberofInfectedPatients)
        {
            LevelManager.Instance.LoadGameOver();
        }
    }
}
