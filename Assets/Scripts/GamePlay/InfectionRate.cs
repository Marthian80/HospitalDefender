using System;
using UnityEngine;

public class InfectionRate : MonoBehaviour
{
    [SerializeField] private int allowedNumberofInfectedPatients = 5;

    private LevelManager levelManager;
    private int currentInfectedPatients = 0;
    public int CurrentInfectedPatients { get { return currentInfectedPatients; } }

    public event Action<Vector2> patientInfected;

    private void Awake()
    {
        levelManager = FindObjectOfType<LevelManager>();
    }

    public void InfectPatientAtLocation(Vector2 location)
    {
        currentInfectedPatients++;

        if (patientInfected != null)
        {
            patientInfected(location);
        }

        if (currentInfectedPatients >= allowedNumberofInfectedPatients)
        {
            levelManager.LoadGameOver();
        }
    }
}
